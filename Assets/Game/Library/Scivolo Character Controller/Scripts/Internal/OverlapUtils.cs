using UnityEngine;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public static class OverlapUtils
    {
        private readonly static Collider[] colliders = new Collider[2];

        /// <summary>
        /// Checks if capsule overlaps other colliders, returns true if it does, false otherwise.
        /// </summary>
        public static bool CheckCapsuleOverlap(Vector3 lowerCenter, Vector3 upperCenter, float radius, LayerMask collisionMask, Collider colliderToIgnore)
        {
            int colliderCount = Physics.OverlapCapsuleNonAlloc(lowerCenter, upperCenter, radius, colliders, collisionMask, QueryTriggerInteraction.Ignore);

            return colliderCount > 1 || (colliderCount == 1 && colliders[0] != colliderToIgnore);
        }

        /// <summary>
        /// Populates an array with the overlapping colliders.
        /// </summary>
        public static int OverlapCapsule(Vector3 lowerCenter, Vector3 upperCenter, float radius, Collider[] overlaps, LayerMask collisionMask, Collider colliderToIgnore)
        {
            int count = Physics.OverlapCapsuleNonAlloc(lowerCenter, upperCenter, radius, overlaps, collisionMask, QueryTriggerInteraction.Ignore);
            return RearrangeColliders(overlaps, colliderToIgnore, count);
        }

        /// <summary>
        /// Gets the average of the separation directions from each overlapping collider.
        /// </summary>
        /// <returns>The average of the separation directions, not normalized.</returns>
        public static Vector3 GetSeparationDirectionAverage(Vector3 position, Quaternion rotation, CapsuleCollider capsuleCollider, float overlapMargin, int overlapCount, Collider[] overlaps)
        {
            overlapCount = Mathf.Min(overlapCount, overlaps.Length);
            Vector3 directionAverage = new Vector3(0f, 0f, 0f);
            int directionCount = 0;

            var capsuleInflator = new CapsuleInflator(capsuleCollider);
            capsuleInflator.InflateCapsule(overlapMargin);

            for (int i = 0; i < overlapCount; i++)
            {
                Collider otherCollider = overlaps[i];

                if (Physics.ComputePenetration(capsuleCollider, position, rotation, otherCollider, otherCollider.transform.position, otherCollider.transform.rotation, out Vector3 direction, out _))
                {
                    directionAverage += direction;
                    directionCount++;
                }
            }

            capsuleInflator.DeflateCapsule();

            if (directionCount == 0)
            {
                return new Vector3(0f, 0f, 0f);
            }

            directionAverage /= directionCount;

            return directionAverage;
        }

        // It makes all the valid colliders to be in the 0 to count - 1 range.
        private static int RearrangeColliders(Collider[] colliders, Collider colliderToIgnore, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (colliders[i] == colliderToIgnore)
                {
                    count--;

                    if (i != count)
                        colliders[i] = colliders[count];

                    break;
                }
            }

            return count;
        }
    }
}
