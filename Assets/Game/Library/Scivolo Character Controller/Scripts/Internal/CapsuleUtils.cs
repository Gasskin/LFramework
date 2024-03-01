using UnityEngine;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public static class CapsuleUtils
    {
        /// <summary>
        /// Gets the position of the lower hemisphere center of a capsule with Y-Axis direction in the object's local space.
        /// </summary>
        /// <param name="localCenter">Capsule center in the object's local space.</param>
        public static Vector3 GetLocalLowerCenter_YAxis(float radius, float height, Vector3 localCenter)
        {
            return new Vector3(localCenter.x, localCenter.y - 0.5f * height + radius, localCenter.z);
        }

        /// <summary>
        /// Gets the position of the upper hemisphere center of a capsule with Y-Axis direction in the object's local space.
        /// </summary>
        /// <param name="localCenter">Capsule center in the object's local space.</param>
        public static Vector3 GetLocalUpperCenter_YAxis(float radius, float height, Vector3 localCenter)
        {
            return new Vector3(localCenter.x, localCenter.y + 0.5f * height - radius, localCenter.z);
        }
    }
}
