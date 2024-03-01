//#define MB_DEBUG

using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.CapsuleUtils;
using static MenteBacata.ScivoloCharacterController.Internal.OverlapUtils;
using static MenteBacata.ScivoloCharacterController.Internal.SeparationOfBestFitCalculator;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public static class OverlapResolver
    {
        internal const int maxOverlaps = 5;

        private const int maxIterations = 3;

        private static readonly Collider[] overlaps = new Collider[maxOverlaps];

        // Buffer for separation directions from single colliders.
        private static readonly Vector3[] directions = new Vector3[maxOverlaps];

        // Buffer for separation distances from single colliders.
        private static readonly float[] distances = new float[maxOverlaps];

        /// <summary>
        /// Tries to resolve capsule's overlaps, if it succeeds it return true, false otherwise.
        /// </summary>
        public static bool TryResolveCapsuleOverlap(Vector3 position, Quaternion rotation, CapsuleCollider capsuleCollider, float overlapMargin, float contactOffset, LayerMask collisionMask, out Vector3 suggestedPosition)
        {
            Vector3 toLowerCenter = rotation * GetLocalLowerCenter_YAxis(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.center);
            Vector3 toUpperCenter = rotation * GetLocalUpperCenter_YAxis(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.center);
            float checkOverlapRadius = capsuleCollider.radius + overlapMargin;

            // It uses inflated capsule to collect and resolve overlaps but the original it's still used to check overlaps.
            CapsuleInflator capsuleInflator = new CapsuleInflator(capsuleCollider);

            Vector3 currentPosition = position;
            bool success = false;

            for (int i = 0; i < maxIterations; i++)
            {
                Vector3 lowerCenter = currentPosition + toLowerCenter;
                Vector3 upperCenter = currentPosition + toUpperCenter;

                if (!CheckCapsuleOverlap(lowerCenter, upperCenter, checkOverlapRadius, collisionMask, capsuleCollider))
                {
                    success = true;
                    break;
                }

                // It inflates the capsule only if it needs to resolve overlaps, just in case changing collider's fields triggers something on
                // the unity side.
                if (!capsuleInflator.IsInflated)
                {
                    capsuleInflator.InflateCapsule(contactOffset);
                }

                int overlapsCount = Physics.OverlapCapsuleNonAlloc(lowerCenter, upperCenter, capsuleCollider.radius, overlaps, collisionMask, QueryTriggerInteraction.Ignore);

                if (TryGetSeparation(currentPosition, rotation, capsuleCollider, overlapsCount, out Vector3 separation))
                {
                    currentPosition += separation;

                    if (Math.IsCircaZero(separation))
                        break;
                }
                else
                {
                    success = false;
                    break;
                }
            }

            if (capsuleInflator.IsInflated)
            {
                capsuleInflator.DeflateCapsule();
            }

            suggestedPosition = currentPosition;

            return success || !CheckCapsuleOverlap(currentPosition + toLowerCenter, currentPosition + toUpperCenter, checkOverlapRadius, collisionMask, capsuleCollider);
        }

        private static bool TryGetSeparation(Vector3 position, in Quaternion rotation, CapsuleCollider capsuleCollider, int overlapsCount, out Vector3 separation)
        {
            int separationsCount = PopulateSeparations(position, rotation, capsuleCollider, overlapsCount);

            if (separationsCount < 1)
            {
                separation = default;
                return false;
            }

            if (separationsCount > 1)
            {
                separation = GetSeparationOfBestFit(directions, distances, separationsCount);

                // Sometime separation of best fit can have very high magnitude so it's better to clamp it.
                Math.ClampMagnitude(ref separation, capsuleCollider.radius);
            }
            else
                separation = distances[0] * directions[0];

            return true;
        }

        private static int PopulateSeparations(Vector3 position, in Quaternion rotation, CapsuleCollider capsuleCollider, int overlapsCount)
        {
            int k = 0;

            for (int i = 0; i < overlapsCount; i++)
            {
                Collider otherCollider = overlaps[i];

                if (capsuleCollider == otherCollider)
                    continue;

                if (Physics.ComputePenetration(capsuleCollider, position, rotation, otherCollider, otherCollider.transform.position, otherCollider.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    directions[k] = direction;
                    distances[k++] = distance;
                }
            }

            return k;
        }
    }
}
