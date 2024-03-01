//#define MB_DEBUG

using UnityEngine;
using MenteBacata.ScivoloCharacterController.Internal;
using static MenteBacata.ScivoloCharacterController.Internal.Math;
using static MenteBacata.ScivoloCharacterController.Internal.MathUtils;
using static MenteBacata.ScivoloCharacterController.Internal.SweepTestsWithPadding;
using static MenteBacata.ScivoloCharacterController.Internal.StepDetectionUtils;
using static MenteBacata.ScivoloCharacterController.Internal.MovementSurfaceUtils;

namespace MenteBacata.ScivoloCharacterController
{
    [RequireComponent(typeof(CharacterCapsule), typeof(CharacterMover))]
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField]
        [Min(0f)]
        [Tooltip("Small tolerance distance so that ground is detected even if the capsule is not directly touching it but just close enough.")]
        private float tolerance = 0.05f;

        private LayerMask collisionMask;

        private CharacterCapsule capsule;

        private new Collider collider;

        private CharacterMover mover;

        private Vector3 upDirection;

        private float capsuleRadius;

        private float contactOffset;

        private float minFloorUp;


        private void Awake()
        {
            capsule = GetComponent<CharacterCapsule>();
            mover = GetComponent<CharacterMover>();
        }

        /// <summary>
        /// Detects ground below the capsule bottom and retrieves useful info.
        /// </summary>
        /// <returns>True, if ground has been found, false otherwise.</returns>
        public bool DetectGround(out GroundInfo groundInfo)
        {
            UpdateCachedValues();

            Vector3 capsuleLowerCenter = capsule.LowerHemisphereCenter;
            Vector3 sweepCenter = capsuleLowerCenter;
            Vector3 sweepDirection = -upDirection;
            float sweepMaxDistance = tolerance;

            SweepTestSphere(sweepCenter, capsuleRadius, sweepDirection, sweepMaxDistance, contactOffset, collisionMask, collider, out SweepResult downwardSweepResult);

            if (!downwardSweepResult.hasHit)
            {
                groundInfo = default;
                return false;
            }

            ref RaycastHit bottomHit = ref downwardSweepResult.hit;

            if (CheckFloorOnPoint(bottomHit.point, bottomHit.normal, out Vector3 floorNormal))
            {
                groundInfo = new GroundInfo(
                        bottomHit.point,
                        floorNormal,
                        bottomHit.normal,
                        bottomHit.collider,
                        true);

                return true;
            }

            float toleranceLeft = tolerance - downwardSweepResult.safeDistance;

            if (toleranceLeft < epsilon)
            {
                groundInfo = new GroundInfo(
                        bottomHit.point,
                        bottomHit.normal,
                        bottomHit.normal,
                        bottomHit.collider,
                        false);

                return true;
            }

            // It sweeps the bottom sphere along the surface of the first contact for a small distance to detect another contact.
            sweepCenter -= downwardSweepResult.safeDistance * upDirection;
            CalculateSteepestDescentVector(bottomHit.normal, toleranceLeft, upDirection, out sweepDirection, out sweepMaxDistance);

            SweepTestSphere(sweepCenter, capsuleRadius, sweepDirection, sweepMaxDistance, contactOffset, collisionMask, collider, out SweepResult slopedSweepResult);

            if (!slopedSweepResult.hasHit)
            {
                groundInfo = new GroundInfo(
                        bottomHit.point,
                        bottomHit.normal,
                        bottomHit.normal,
                        bottomHit.collider,
                        false);

                return true;
            }

            ref RaycastHit secondBottomHit = ref slopedSweepResult.hit;

            // If the hit point is not directly below the capsule...
            if (!IsPointWithinDistanceFromLine(secondBottomHit.point, capsuleLowerCenter, upDirection, capsuleRadius))
            {
                groundInfo = new GroundInfo(
                        bottomHit.point,
                        bottomHit.normal,
                        bottomHit.normal,
                        bottomHit.collider,
                        false);

                return true;
            }

            if (CheckFloorOnPoint(secondBottomHit.point, secondBottomHit.normal, out floorNormal))
            {
                groundInfo = new GroundInfo(
                        secondBottomHit.point,
                        floorNormal,
                        secondBottomHit.normal,
                        secondBottomHit.collider,
                        true);

                return true;
            }

            groundInfo = new GroundInfo(
                        bottomHit.point,
                        bottomHit.normal,
                        bottomHit.normal,
                        bottomHit.collider,
                        false);

            return true;
        }

        private void UpdateCachedValues()
        {
            collider = capsule.Collider;
            collisionMask = capsule.CollisionMask;
            upDirection = capsule.UpDirection;
            capsuleRadius = capsule.Radius;
            contactOffset = capsule.contactOffset;
            minFloorUp = Mathf.Cos(Mathf.Deg2Rad * mover.maxFloorAngle);
        }

        private bool CheckFloorOnPoint(Vector3 position, Vector3 normal, out Vector3 floorNormal)
        {
            if (Dot(normal, upDirection) < 0f)
            {
                floorNormal = default;
                return false;
            }

            bool hasUpperFloor = CheckFloorAbovePoint(position, capsuleRadius, minFloorUp, upDirection, collisionMask, collider, out RaycastHit upperFloorHit);

            if (GetMovementSurface(normal, upDirection, minFloorUp) == MovementSurface.Floor)
            {
                if (hasUpperFloor)
                {
                    // Uses the normal of the flattest floor.
                    floorNormal = Dot(upperFloorHit.normal - normal, upDirection) > 0f ? upperFloorHit.normal : normal;
                }
                else
                {
                    floorNormal = normal;
                }

                return true;
            }

            if (hasUpperFloor)
            {
                floorNormal = upperFloorHit.normal;
                return true;
            }

            floorNormal = default;
            return false;
        }
    }
}
