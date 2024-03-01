//#define MB_DEBUG

using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.GeometryCasts;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    /*
     * Collection of sweep test methods that add a small offset to the maximum distance so that the sweep stops at the offset distance from 
     * the hit point, and if no hit occurs, also ensures that there are no hits within the offset distance further along the sweep direction.
     */
    public static class SweepTestsWithOffset
    {
        public static void SweepTestCapsule(Vector3 lowerCenter, Vector3 upperCenter, float radius, Vector3 direction, float maxDistance, float offset, LayerMask collisionMask, Collider colliderToIgnore, out SweepResult result)
        {
            result = new SweepResult();

            if (CapsuleCast(lowerCenter, upperCenter, radius, direction, maxDistance + offset, collisionMask, colliderToIgnore, out result.hit, out result.startedWithOverlap))
            {
                result.hasHit = true;
                result.safeDistance = Mathf.Clamp(result.hit.distance - offset, 0f, maxDistance);
            }
            else
            {
                result.hasHit = false;
                result.safeDistance = maxDistance;
            }
        }

        public static void SweepTestSphere(Vector3 center, float radius, Vector3 direction, float maxDistance, float offset, LayerMask collisionMask, Collider colliderToIgnore, out SweepResult result)
        {
            result = new SweepResult();

            if (SphereCast(center, radius, direction, maxDistance + offset, collisionMask, colliderToIgnore, out result.hit, out result.startedWithOverlap))
            {
                result.hasHit = true;
                result.safeDistance = Mathf.Clamp(result.hit.distance - offset, 0f, maxDistance);
            }
            else
            {
                result.hasHit = false;
                result.safeDistance = maxDistance;
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
