//#define MB_DEBUG

using UnityEngine;
using MenteBacata.ScivoloCharacterController.Internal;
using static MenteBacata.ScivoloCharacterController.Internal.Math;
using static MenteBacata.ScivoloCharacterController.Internal.MathUtils;
using static MenteBacata.ScivoloCharacterController.Internal.StepDetectionUtils;
using static MenteBacata.ScivoloCharacterController.Internal.MovementSurfaceUtils;
using static MenteBacata.ScivoloCharacterController.Internal.GeometricTests;
using static MenteBacata.ScivoloCharacterController.Internal.OverlapUtils;
using static MenteBacata.ScivoloCharacterController.Internal.SweepTestsWithPadding;
using Plane = MenteBacata.ScivoloCharacterController.Internal.Plane;

namespace MenteBacata.ScivoloCharacterController
{
    [RequireComponent(typeof(CharacterCapsule))]
    public partial class CharacterMover : MonoBehaviour
    {
        [Tooltip("Simple Slide: it moves doing simple collide and slide. Suitable for air/water movement.\nWalk: it climbs steps, clamps to floor and prevents climbing steep slope. Suitable for walking/running or any movement on feet.")]
        public Mode mode = Mode.SimpleSlide;

        [Range(30f, 75f)]
        [Tooltip("Maximum angle a slope can have in order to be considered floor.")]
        public float maxFloorAngle = 45f;

        [Header("Walk Mode Only")]
        [Min(0f)]
        [Tooltip("Maximum height of climbable step.")]
        public float maxStepHeight = 0.3f;

        [Header("Simple Slide Mode Only")]
        [Tooltip("Allows the character to climb slopes which exceed the maximum floor angle.")]
        public bool canClimbSteepSlope = true;

        private const int maxMoveLoopIterationsSimpleSlide = 5;
        
        private const int maxMoveLoopIterationsWalk = 3;

        private const int maxMoveLoopIterationsStepClimb = 2;

        private const int maxStepDownLoopIterations = 2;

        private const int maxStepClimbAttempts = 1;

        /// <summary>
        /// Maximum distance between two contacts for them to be considered near in relation to the height of the capsule.
        /// </summary>
        private const float maxNearContactDistanceOverHeight = 0.03f;

        /// <summary>
        /// How far it can move forward in the step down loop relative to the movement length.
        /// </summary>
        private const float maxStepDownForwardDistanceOverMovement = 0.5f;

        /// <summary>
        /// How far it can move sideways in the step down loop relative to the movement length.
        /// </summary>
        private const float maxStepDownLateralDistanceOverMovement = 1.5f;

        /// <summary>
        /// Minimum scaling that can be applied to the movement when overlapping other colliders.
        /// </summary>
        private const float minMovementScalingOnOverlap = 0.05f;

        private CharacterCapsule capsule;

        private CapsuleCollider capsuleCollider;

        #region Cached Values
        /// <summary>
        /// Minimum up component of the ground normal for it to be considered floor. 
        /// </summary>
        private float minFloorUp;

        /// <summary>
        /// Tangent of the max floor angle.
        /// </summary>
        private float tanMaxFloorAngle;

        private LayerMask collisionMask;

        private float capsuleHeight;

        private float capsuleRadius;

        private float capsuleContactOffset;

        private float capsuleVerticalOffset;

        private Vector3 upDirection;

        private Vector3 toCapsuleLowerCenter;

        private Vector3 toCapsuleUpperCenter;

        private Quaternion capsuleRotation;
        #endregion


        /// <summary>
        /// Returns a new instance of a MoveContact array that is of optimal length to contain all possible contacts that can occur during a 
        /// single call to the Move method.
        /// </summary>
        public static MoveContact[] NewMoveContactArray => new MoveContact[MaxContactsCount];

        /// <summary>
        /// Maximum number of contacts which can occur during a Move call.
        /// </summary>
        private static int MaxContactsCount => Mathf.Max(
            maxMoveLoopIterationsSimpleSlide,
            maxMoveLoopIterationsWalk + maxMoveLoopIterationsStepClimb + maxStepDownLoopIterations);

        private void Awake()
        {
            capsule = GetComponent<CharacterCapsule>();
        }

        /// <summary>
        /// Moves the character according to the movement settings trying to fulfill the desired movement as close as it can, if a 
        /// MoveContact array is provided it populates it with the contact informations it has found during the movement.
        /// </summary>
        /// <param name="hasGroundInfo">If true, the ground info is considered valid otherwise it is ignored.</param>
        public void Move(Vector3 desiredMovement, bool hasGroundInfo, in GroundInfo groundInfo, int overlapCount, Collider[] overlaps, MoveContact[] moveContacts, out int contactCount)
        {
            UpdateCachedValues();

            Vector3 position = capsule.Position;

            if (mode == Mode.Walk)
            {
                bool hasInitialFloor = hasGroundInfo && GetMovementSurface(groundInfo.tangentNormal, upDirection, minFloorUp) == MovementSurface.Floor;
                Vector3? initialFloorNormal = hasInitialFloor ? groundInfo.tangentNormal : (Vector3?)null;

                MoveForWalkMode(ref position, desiredMovement, initialFloorNormal, overlapCount, overlaps, moveContacts, out contactCount);
            }
            else
            {
                MoveForSimpleSlide(ref position, desiredMovement, overlapCount, overlaps, moveContacts, out contactCount);
            }

            capsule.Position = position;
        }

        private void UpdateCachedValues()
        {
            maxFloorAngle = Mathf.Clamp(maxFloorAngle, 25f, 75f);
            minFloorUp = Mathf.Cos(Mathf.Deg2Rad * maxFloorAngle);
            tanMaxFloorAngle = Mathf.Tan(Mathf.Deg2Rad * maxFloorAngle);

            capsuleCollider = capsule.CapsuleCollider;
            collisionMask = capsule.CollisionMask;
            capsuleHeight = capsule.Height;
            capsuleRadius = capsule.Radius;
            capsuleContactOffset = capsule.contactOffset;
            capsuleVerticalOffset = capsule.VerticalOffset;
            upDirection = capsule.UpDirection;
            capsuleRotation = capsule.Rotation;
            toCapsuleLowerCenter = capsuleRotation * capsule.LocalLowerHemisphereCenter;
            toCapsuleUpperCenter = capsuleRotation * capsule.LocalUpperHemisphereCenter;
        }

        private void MoveForWalkMode(ref Vector3 position, Vector3 desiredMovement, Vector3? initialFloorNormal, int overlapCount, Collider[] overlaps, MoveContact[] moveContacts, out int contactCount)
        {
            Vector3 initialPosition = position;
            Vector3 desiredMovementHorizontal = ProjectOnPlane(desiredMovement, upDirection);
            Vector3 progressDirection = Normalized(desiredMovementHorizontal);
            float maxFloorDescent = tanMaxFloorAngle * Magnitude(desiredMovementHorizontal);
            bool hasInitialOverlap = overlapCount > 0;

            if (initialFloorNormal.HasValue)
            {
                MovementResolver movementResolver = new MovementResolver(upDirection, minFloorUp);
                desiredMovement = movementResolver.GetMovementOneSurface(desiredMovement, initialFloorNormal.Value, false, true);
            }
            else
            {
                // Makes desired movement to go down at max floor angle.
                desiredMovement = desiredMovementHorizontal - maxFloorDescent * upDirection;
            }

            if (hasInitialOverlap && overlaps != null)
            {
                AdjustMovementToOverlaps(ref desiredMovement, position, overlapCount, overlaps, false);
            }

            contactCount = 0;

            MoveLoopOptions moveLoopOptions = new MoveLoopOptions
            {
                canClimbSteepSlope = false,
                canClampToFloor = true,
                canClimbStep = !hasInitialOverlap,
                breakOnSweepOverlap = !hasInitialOverlap
            };

            DoMoveLoop(ref position, desiredMovement, progressDirection, initialFloorNormal, moveContacts, ref contactCount, moveLoopOptions, maxMoveLoopIterationsWalk, out LoopBreakInfo breakInfo);

            if (hasInitialOverlap)
            {
                // Skips step down because it could push even deeper into an overlapping collider.
                return;
            }

            if (breakInfo != LoopBreakInfo.None)
            {
                return;
            }

            Vector3 movementMade = position - initialPosition;

            float maxStepDownDistanceNoFloor = Mathf.Max(Dot(movementMade, upDirection) + maxFloorDescent, 0f);
            float maxStepDownDistance = maxStepDownDistanceNoFloor + maxStepHeight;

            BoundingRectangle stepDownBounds = GetStepDownBounds(position, desiredMovement, movementMade, progressDirection);

            DoStepDownLoop(ref position, maxStepDownDistance, maxStepDownDistanceNoFloor, stepDownBounds, moveContacts, ref contactCount, !hasInitialOverlap, maxStepDownLoopIterations, out _);
        }

        private BoundingRectangle GetStepDownBounds(Vector3 position, Vector3 desiredMovement, Vector3 movementMade, Vector3 progressDirection)
        {
            // The length of the horizontal desired movement is used as a reference for the size of the bounds.
            float desiredLength = Magnitude(ProjectOnPlane(desiredMovement, upDirection));

            // Distance from position to the bounds front side.
            float frontSideDistance = maxStepDownForwardDistanceOverMovement * desiredLength;

            // Distance from position to the bounds back side. It should pass through the initial position to prevent moving behind it.
            float backSideDistance = Mathf.Max(Dot(movementMade, progressDirection), 0f);

            BoundingRectangle bounds = new BoundingRectangle
            {
                center = position + 0.5f * (frontSideDistance - backSideDistance) * progressDirection,
                forwardDirection = progressDirection,
                forwardExtent = 0.5f * (frontSideDistance + backSideDistance),
                rightDirection = Cross(upDirection, progressDirection),
                rightExtent = maxStepDownLateralDistanceOverMovement * desiredLength
            };

            return bounds;
        }

        private void MoveForSimpleSlide(ref Vector3 position, Vector3 desiredMovement, int overlapCount, Collider[] overlaps, MoveContact[] moveContacts, out int contactCount)
        {
            Vector3 progressDirection = Normalized(desiredMovement);

            if (overlapCount > 0 && overlaps != null)
            {
                AdjustMovementToOverlaps(ref desiredMovement, position, overlapCount, overlaps, canClimbSteepSlope);
            }

            contactCount = 0;

            MoveLoopOptions moveLoopOptions = new MoveLoopOptions
            {
                canClimbSteepSlope = canClimbSteepSlope,
                canClampToFloor = false,
                canClimbStep = false,
                breakOnSweepOverlap = overlapCount == 0
            };

            DoMoveLoop(ref position, desiredMovement, progressDirection, null, moveContacts, ref contactCount, moveLoopOptions, maxMoveLoopIterationsSimpleSlide, out _);
        }

        /// <summary>
        /// Adjusts the movement so that it doesn't move too much into the overlapping colliders.
        /// </summary>
        private void AdjustMovementToOverlaps(ref Vector3 movement, Vector3 position, int overlapCount, Collider[] overlaps, bool canClimbSteepSlope)
        {
            Vector3 directionAverage = GetSeparationDirectionAverage(position, capsuleRotation, capsuleCollider, CharacterCapsule.overlapMargin, overlapCount, overlaps);

            MagnitudeAndDirection(directionAverage, out float magnitude, out Vector3 direction);

            // The direction average has magnitude between 0 and 1, it is 1 when all the separations have the same direction. Then its
            // magnitude can be used to modulate the movement.
            movement *= Mathf.Max(minMovementScalingOnOverlap, magnitude);

            if (IsCircaZero(magnitude))
            {
                return;
            }

            MovementResolver movementResolver = new MovementResolver(upDirection, minFloorUp);

            // The direction average is treated as a surface normal. It’s not an exact solution as it doesn’t prevent moving into the
            // overlapping colliders, but it helps to limit the movement into them.
            movement = movementResolver.GetMovementOneSurface(movement, direction, canClimbSteepSlope, false);
        }

        /// <summary>
        /// Main move loop which performs movement and handles collision and step climbing.
        /// </summary>
        /// <param name="progressDirection">Direction which it can't move against on each iteration.</param>
        private void DoMoveLoop(ref Vector3 position, Vector3 desiredMovement, Vector3 progressDirection, Vector3? initialFloorNormal, MoveContact[] moveContacts, ref int contactCount, MoveLoopOptions options, int maxIterations, out LoopBreakInfo breakInfo)
        {
            breakInfo = LoopBreakInfo.None;

            if (Dot(desiredMovement, progressDirection) < 0f)
            {
                return;
            }

            Vector3 movementToMake = desiredMovement;

            bool hasCurrentContact = false;
            ContactInfo currentContact = default;

            Vector3? lastFloorNormal = initialFloorNormal;

            int stepClimbAttempts = 0;

            for (int i = 0; i < maxIterations; i++)
            {
                if (IsCircaZero(movementToMake))
                {
                    break;
                }

                SweepCapsuleAndUpdateContact(position, movementToMake, ref hasCurrentContact, ref currentContact, out Vector3 sweepMovement, out SweepResult sweepResult);
                
                if (sweepResult.startedWithOverlap && options.breakOnSweepOverlap)
                {
                    break;
                }

                position += sweepMovement;

                if (!hasCurrentContact)
                {
                    break;
                }

                AddMoveContact(new MoveContact(currentContact.position, currentContact.normal, currentContact.collider), moveContacts, ref contactCount);

                if (currentContact.surface == MovementSurface.Floor)
                    lastFloorNormal = currentContact.normal;

                movementToMake -= sweepMovement;

                if (options.canClimbStep && stepClimbAttempts < maxStepClimbAttempts && CanTryClimbStep(position, movementToMake, currentContact))
                {
                    stepClimbAttempts++;

                    if (TryClimbStep(ref position, movementToMake, progressDirection, currentContact.normal, moveContacts, ref contactCount))
                    {
                        breakInfo = LoopBreakInfo.ClimbedStep;
                        break;
                    }
                }

                movementToMake = GetMovementOnContact(movementToMake, currentContact, progressDirection, lastFloorNormal, options.canClimbSteepSlope, options.canClampToFloor);
            }
        }

        private bool CanTryClimbStep(Vector3 position, Vector3 movement, in ContactInfo contact)
        {
            if (IsCircaZero(movement))
                return false;

            Vector3 movementHorizontal = ProjectOnPlane(movement, upDirection);

            // The movement is to actually climb up the step, not descending it.
            if (Dot(movementHorizontal, contact.normal) >= 0)
                return false;

            return IsClimbableStepCandidate(position, contact.position, contact.surface);
        }

        private bool IsClimbableStepCandidate(Vector3 position, Vector3 contactPosition, in MovementSurface contactSurface)
        {
            if (contactSurface != MovementSurface.SteepSlope && contactSurface != MovementSurface.Wall)
                return false;

            if (GetPointHeightFromCapsuleBottom(contactPosition, position) >= maxStepHeight)
                return false;

            return true;
        }

        /// <summary>
        /// Tries to climb a step by first stepping up the max step height, then moving horizontally and lastly stepping down. If it steps 
        /// down on floor it returns true and updates the position.
        /// </summary>
        /// <param name="stepNormal">Normal of the step contact, facing away from the step.</param>
        private bool TryClimbStep(ref Vector3 position, Vector3 desiredMovement, Vector3 progressDirection, Vector3 stepNormal, MoveContact[] moveContacts, ref int contactCount)
        {
            Vector3 tempPosition = position;
            float maxStepUpDistance = maxStepHeight + capsuleContactOffset;

            // Step up.
            SweepTest(tempPosition, upDirection, maxStepUpDistance, out SweepResult stepUpResult);

            tempPosition += stepUpResult.safeDistance * upDirection;

            if (stepUpResult.startedWithOverlap)
            {
                return false;
            }

            if (stepUpResult.safeDistance < epsilon)
            {
                return false;
            }

            int tempContactCount = contactCount;

            Vector3 horizontalMovement = ProjectOnPlane(desiredMovement, upDirection);

            MoveLoopOptions moveLoopOptions = new MoveLoopOptions
            {
                canClimbSteepSlope = false,
                canClampToFloor = false,
                canClimbStep = false,
                breakOnSweepOverlap = true
            };

            DoMoveLoop(ref tempPosition, horizontalMovement, progressDirection, null, moveContacts, ref tempContactCount, moveLoopOptions, maxMoveLoopIterationsStepClimb, out LoopBreakInfo breakInfo);

            if (breakInfo == LoopBreakInfo.SweepOverlap)
            {
                return false;
            }

            Vector3 movementMade = tempPosition - position;

            if (IsCircaZero(ProjectOnPlane(movementMade, upDirection)))
            {
                return false;
            }

            float maxStepDownDistance = Mathf.Max(Dot(movementMade, upDirection), 0f) + capsuleContactOffset;

            BoundingRectangle stepDownBounds = GetStepDownBounds(tempPosition, desiredMovement, movementMade, progressDirection);

            DoStepDownLoop(ref tempPosition, maxStepDownDistance, 0f, stepDownBounds, moveContacts, ref tempContactCount, true, maxStepDownLoopIterations, out bool hasFoundFloor);

            if (!hasFoundFloor)
            {
                return false;
            }

            movementMade = tempPosition - position;

            // Checks if it has actually moved past the step and not slided back down to the floor where it started. It’s not perfect because
            // it assumes the edge of the step is horizontal, but it helps to prevent many common cases and in general it limits the damage.
            if (Dot(ProjectOnPlane(movementMade, upDirection), stepNormal) > 0f)
            {
                return false;
            }

            position = tempPosition;
            contactCount = tempContactCount;
            return true;
        }

        /// <summary>
        /// Iteratively slides down until it completes the movement or it reaches the floor. If at the end it hasn't reached the floor then 
        /// it goes down using at most the max no floor distance.
        /// </summary>
        /// <param name="maxDistance">Max reachable distance in the downward direction.</param>
        /// <param name="maxDistanceNoFloor">Max downward distance if at the end of the loop no floor has been found.</param>
        /// <param name="hasFoundFloor">True if it has found floor, false otherwise.</param>
        private void DoStepDownLoop(ref Vector3 position, float maxDistance, float maxDistanceNoFloor, BoundingRectangle bounds, MoveContact[] moveContacts, ref int contactCount, bool breakOnSweepOverlap, int maxIterations, out bool hasFoundFloor)
        {
            Vector3 initialPosition = position;
            Vector3 desiredMovement = -maxDistance * upDirection;
            Vector3 remainingMovement = desiredMovement;
            Vector3 movementToMake = remainingMovement;

            // It can happen when the desired movement passed to the Move method is zero.
            bool hasValidBounds = bounds.forwardDirection != new Vector3(0f, 0f, 0f);
            bool hasCrossedBounds = false;

            bool hasCurrentContact = false;
            ContactInfo currentContact = default;

            hasFoundFloor = false;
            
            Plane maxDistanceNoFloorPlane = new Plane(upDirection, initialPosition - maxDistanceNoFloor * upDirection, true);
            bool isPastMaxDistanceNoFloor = false;
            Vector3 positionAtMaxDistanceNoFloor = default;
            int contactCountAtMaxDistanceNoFloor = 0;
            for (int i = 0; i < maxIterations; i++)
            {
                if (!CheckMovementForStepDownLoop(movementToMake, remainingMovement))
                {
                    break;
                }

                if (i > 0)
                {
                    if (!hasValidBounds || hasCrossedBounds || !bounds.IsPointInside(position))
                    {
                        break;
                    }
                }

                Vector3 positionBeforeSweep = position;

                SweepCapsuleAndUpdateContact(position, movementToMake, ref hasCurrentContact, ref currentContact, out Vector3 movementMadeSweep, out SweepResult sweepResult);

                if (sweepResult.startedWithOverlap && breakOnSweepOverlap)
                {
                    break;
                }

                position += movementMadeSweep;

                // It checks the intersection with the bounds only after the first iteration, so it at least does the vertical movement.
                if (i > 0)
                {
                    if (bounds.CheckLineIntersectionFromInsideToOutside(positionBeforeSweep, position, out Vector3 boundsIntersection))
                    {
                        position = boundsIntersection;
                        hasCurrentContact = false;
                        hasCrossedBounds = true;
                    }
                }

                if (!isPastMaxDistanceNoFloor)
                {
                    if (CheckLineAndPlaneIntersection(positionBeforeSweep, position, maxDistanceNoFloorPlane, out positionAtMaxDistanceNoFloor))
                    {
                        contactCountAtMaxDistanceNoFloor = contactCount;
                        isPastMaxDistanceNoFloor = true;
                    }
                }

                movementToMake = remainingMovement = desiredMovement - (position - initialPosition);

                if (hasCurrentContact)
                {
                    AddMoveContact(new MoveContact(currentContact.position, currentContact.normal, currentContact.collider), moveContacts, ref contactCount);

                    // After the first iteration, the contact could be generated by a non vertical sweep so it has to check that the point
                    // is right below the capsule.
                    if (i == 0 || IsPointWithinDistanceFromLine(currentContact.position, position, upDirection, capsuleRadius))
                    {
                        if (CheckFloorOnContact(currentContact))
                        {
                            hasFoundFloor = true;
                            break;
                        }
                    }

                    movementToMake = GetMovementOnContact(remainingMovement, currentContact, false, false);

                    // Resizes movement so that it has the same downward component as remaining movement.
                    if (TryResizeToTargetVerticalComponent(movementToMake, remainingMovement, upDirection, out Vector3 resizedMovement))
                        movementToMake = resizedMovement;
                }
            }

            if (isPastMaxDistanceNoFloor && !hasFoundFloor)
            {
                // Reverts back to the state it was in when it reached the max no floor distance.
                position = positionAtMaxDistanceNoFloor;
                contactCount = contactCountAtMaxDistanceNoFloor;
            }
        }

        private bool CheckMovementForStepDownLoop(Vector3 movementToMake, Vector3 remainingMovement)
        {
            if (IsCircaZero(movementToMake))
            {
                return false;
            }

            if (Dot(movementToMake, upDirection) > 0f)
            {
                return false;
            }

            if (Dot(remainingMovement, upDirection) > 0f)
            {
                return false;
            }

            return true;
        }

        private void SweepCapsuleAndUpdateContact(Vector3 position, Vector3 movement, ref bool hasContact, ref ContactInfo contact, out Vector3 sweepMovement, out SweepResult sweepResult)
        {
            MagnitudeAndDirection(movement, out float maxDistance, out Vector3 direction);

            SweepTest(position, direction, maxDistance, out sweepResult);

            if (sweepResult.hasHit)
            {
                bool hasPreviousContact = hasContact;
                Vector3 previousNormal = contact.normal;
                MovementSurface previousSurface = contact.surface;

                ref RaycastHit hit = ref sweepResult.hit;

                contact = new ContactInfo
                {
                    position = hit.point,
                    normal = hit.normal,
                    surface = GetMovementSurface(hit.normal, upDirection, minFloorUp),
                    collider = hit.collider,
                    hasNear = hasPreviousContact && sweepResult.safeDistance < maxNearContactDistanceOverHeight * capsuleHeight,
                    nearNormal = previousNormal,
                    nearSurface = previousSurface
                };

                hasContact = true;
            }
            else
            {
                hasContact = false;
            }

            sweepMovement = sweepResult.safeDistance * direction;
        }

        private void SweepTest(Vector3 position, Vector3 direction, float maxDistance, out SweepResult sweepResult)
        {
            SweepTestCapsule(position + toCapsuleLowerCenter, position + toCapsuleUpperCenter, capsuleRadius, direction, maxDistance, capsuleContactOffset, collisionMask, capsuleCollider, out sweepResult);
        }

        /// <summary>
        /// Checks if the contact point is on a floor surface or on the edge of a step with floor on top.
        /// </summary>
        /// <returns>True if is on floor, false otherwise.</returns>
        private bool CheckFloorOnContact(in ContactInfo contact)
        {
            if (contact.surface == MovementSurface.Floor)
                return true;

            if (Dot(contact.normal, upDirection) < 0f)
                return false;

            return CheckFloorAbovePoint(contact.position, capsuleRadius, minFloorUp, upDirection, collisionMask, capsuleCollider, out _);
        }

        /// <summary>
        /// Gets the resulting movement by resolving the given movement against the contact surface.
        /// </summary>
        /// <param name="progressDirection">Direction which the resulting movement can't be against.</param>
        /// <param name="canClampToFloor">If true, it projects the movement on a floor surface if <paramref name="contact"/> has a floor 
        /// surface or <paramref name="floorNormal"/> is present.</param>
        private Vector3 GetMovementOnContact(Vector3 movement, in ContactInfo contact, Vector3 progressDirection, Vector3? floorNormal, bool canClimbSteepSlope, bool canClampToFloor)
        {
            Vector3 result;

            MovementResolver movementResolver = new MovementResolver(upDirection, minFloorUp);

            if (canClampToFloor && floorNormal.HasValue && !HasFloorSurface(contact))
            {
                // Uses the floor normal as the second normal, ignoring the near normal if present.
                result = movementResolver.GetMovementTwoSurfaces(movement, contact.normal, floorNormal.Value, canClimbSteepSlope, true);
            }
            else if (contact.hasNear)
            {
                result = movementResolver.GetMovementTwoSurfaces(movement, contact.normal, contact.nearNormal, canClimbSteepSlope, canClampToFloor);
            }
            else
            {
                result = movementResolver.GetMovementOneSurface(movement, contact.normal, canClimbSteepSlope, canClampToFloor);
            }

            if (IsCircaZero(result))
                return new Vector3(0f, 0f, 0f);

            if (Dot(result, progressDirection) >= 0f)
                return result;

            // Uses the progress direction as a fake surface normal to enforce the constraint.
            result = movementResolver.GetMovementTwoSurfaces(movement, contact.normal, progressDirection, canClimbSteepSlope, canClampToFloor);

            return result;
        }

        /// <summary>
        /// Gets the resulting movement by resolving the given movement against the contact surface.
        /// </summary>
        /// <param name = "canClampToFloor" > If true, it projects the movement on a floor surface if <paramref name = "contact" /> has a 
        /// floor surface.</param>
        private Vector3 GetMovementOnContact(Vector3 movement, in ContactInfo contact, bool canClimbSteepSlope, bool canClampToFloor)
        {
            MovementResolver movementResolver = new MovementResolver(upDirection, minFloorUp);

            if (contact.hasNear)
            {
                return movementResolver.GetMovementTwoSurfaces(movement, contact.normal, contact.nearNormal, canClimbSteepSlope, canClampToFloor);
            }

            return movementResolver.GetMovementOneSurface(movement, contact.normal, canClimbSteepSlope, canClampToFloor);
        }

        private bool HasFloorSurface(in ContactInfo contact)
        {
            return contact.surface == MovementSurface.Floor || (contact.hasNear && contact.nearSurface == MovementSurface.Floor);
        }

        private float GetPointHeightFromCapsuleBottom(Vector3 point, Vector3 capsulePosition)
        {
            return Dot((point - capsulePosition), upDirection) - capsuleVerticalOffset;
        }

        private void AddMoveContact(in MoveContact moveContact, MoveContact[] moveContacts, ref int contactCount)
        {
            if (moveContacts != null && contactCount < moveContacts.Length)
                moveContacts[contactCount++] = moveContact;
        }

        public enum Mode : byte
        {
            SimpleSlide = 0,
            Walk = 1
        }

        private struct MoveLoopOptions
        {
            public bool canClimbSteepSlope;
            public bool canClampToFloor;
            public bool canClimbStep;
            public bool breakOnSweepOverlap;
        }

        private enum LoopBreakInfo : byte
        {
            None,
            ClimbedStep,
            SweepOverlap
        }

        private struct ContactInfo
        {
            public Vector3 position;
            public Vector3 normal;
            public MovementSurface surface;
            public Collider collider;
            public bool hasNear;
            public Vector3 nearNormal;
            public MovementSurface nearSurface;

        }
    }
}
