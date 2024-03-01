//#define MB_DEBUG

using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.GeometryCasts;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    /// <summary>
    /// Collection of sweep test methods which account for a given padding distance to help prevent from moving too close to the hit surface.
    /// The returned hit distance is shortened as if the sweep stopped at the padding distance from the hit surface. Padding distance is not 
    /// guaranteed to be kept if the sweep starts inside the padding distance or the angle between the direction and the hit normal is too
    /// narrow.
    /// </summary>
    public static class SweepTestsWithPadding
    {
        private const float extraDistanceOverPadding = 5f;
        
        public static void SweepTestCapsule(Vector3 lowerCenter, Vector3 upperCenter, float radius, Vector3 direction, float maxDistance, float padding, LayerMask collisionMask, Collider colliderToIgnore, out SweepResult result)
        {
            result = new SweepResult();

            float extraDistance = extraDistanceOverPadding * padding;

            if (CapsuleCast(lowerCenter, upperCenter, radius, direction, maxDistance + extraDistance, collisionMask, colliderToIgnore, out result.hit, out result.startedWithOverlap))
            {
                float dot = Math.Dot(direction, result.hit.normal);

                if (dot >= 0f)
                {
                    result.safeDistance = result.hit.distance - extraDistance;
                }
                else
                {
                    result.safeDistance = result.hit.distance - Mathf.Min(-padding / dot, extraDistance);
                }

                if (result.safeDistance > maxDistance)
                {
                    result.safeDistance = maxDistance;
                    result.hasHit = false;
                }
                else
                {

                    if (result.safeDistance < 0f)
                        result.safeDistance = 0f;

                    result.hasHit = true;
                }
            }
            else
            {
                result.safeDistance = maxDistance;
                result.hasHit = false;
            }
        }

        public static void SweepTestSphere(Vector3 center, float radius, Vector3 direction, float maxDistance, float padding, LayerMask collisionMask, Collider colliderToIgnore, out SweepResult result)
        {
            result = new SweepResult();

            float extraDistance = extraDistanceOverPadding * padding;

            if (SphereCast(center, radius, direction, maxDistance + extraDistance, collisionMask, colliderToIgnore, out result.hit, out result.startedWithOverlap))
            {
                float dot = Math.Dot(direction, result.hit.normal);

                if (dot >= 0f)
                {
                    result.safeDistance = result.hit.distance - extraDistance;
                }
                else
                {
                    result.safeDistance = result.hit.distance - Mathf.Min(-padding / dot, extraDistance);
                }

                if (result.safeDistance > maxDistance)
                {
                    result.safeDistance = maxDistance;
                    result.hasHit = false;
                }
                else
                {

                    if (result.safeDistance < 0f)
                        result.safeDistance = 0f;

                    result.hasHit = true;
                }
            }
            else
            {
                result.safeDistance = maxDistance;
                result.hasHit = false;
            }
        }

        public struct SweepResult
        {
            /// <summary>
            /// Distance that can be moved without getting too close to the hit point, if present, otherwise is equal to max distance.
            /// </summary>
            public float safeDistance;

            public bool hasHit;

            /// <summary>
            /// Retrieved hit, its distance can be grater than max distance.
            /// </summary>
            public RaycastHit hit;

            public bool startedWithOverlap;
        }
    }
}
