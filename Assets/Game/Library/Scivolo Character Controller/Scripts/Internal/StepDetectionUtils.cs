//#define MB_DEBUG

using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.GeometryCasts;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public static class StepDetectionUtils
    {
        /// <summary>
        /// Checks if there is floor above a certain point by casting down a sphere from above the specified point.
        /// </summary>
        public static bool CheckFloorAbovePoint(Vector3 point, float capsuleRadius, float minFloorUp, Vector3 upDirection,
            LayerMask collisionMask, Collider colliderToIgnore, out RaycastHit floorHit)
        {
            const float probeRadiusOverCapsuleRadius = 0.25f;
            const float verticalOffsetOverCapsuleRadius = 0.6f;

            float verticalOffset = verticalOffsetOverCapsuleRadius * capsuleRadius;
            float probeRadius = probeRadiusOverCapsuleRadius * capsuleRadius;

            float centerToPointDistance = verticalOffset + probeRadius;
            Vector3 center = point + centerToPointDistance * upDirection;

            if (SphereCast(center, probeRadius, -upDirection, centerToPointDistance, collisionMask, colliderToIgnore, out floorHit, out bool hasInitialOverlap))
            {
                return !hasInitialOverlap && Math.Dot(floorHit.normal, upDirection) > minFloorUp;
            }

            return false;
        }

        /// <summary>
        /// Checks if there is enough space above a point for the capsule to partially fit.
        /// </summary>
        public static bool CheckSpaceAbovePoint(Vector3 point, float capsuleRadius, float capsuleHeight, Vector3 upDirection,
            LayerMask collisionMask, Collider colliderToIgnore)
        {
            const float probeRadiusOverCapsuleRadius = 0.8f;
            const float verticalOffsetOverCapsuleRadius = 0.4f;

            float verticalOffset = verticalOffsetOverCapsuleRadius * capsuleRadius;
            float probeRadius = probeRadiusOverCapsuleRadius * capsuleRadius;
            float probeHeight = capsuleHeight - verticalOffset;

            // Not needed if 2 * probe radius + vertical offset <= 2 * capsule radius.
            /*if (probeHeight < 2f * probeRadius)
                probeHeight = 2f * probeRadius;*/

            float centerToPointDistance = verticalOffset + probeRadius;
            Vector3 lowerCenter = point + centerToPointDistance * upDirection;
            Vector3 upperCenter = lowerCenter + (probeHeight - 2f * probeRadius) * upDirection;

            return !OverlapUtils.CheckCapsuleOverlap(lowerCenter, upperCenter, probeRadius, collisionMask, colliderToIgnore);
        }
    }
}
