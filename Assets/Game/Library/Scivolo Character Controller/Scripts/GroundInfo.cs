using UnityEngine;

namespace MenteBacata.ScivoloCharacterController
{
    public struct GroundInfo
    {
        /// <summary>
        /// Ground point considered in contact with the capsule bottom hemisphere. 
        /// </summary>
        public readonly Vector3 point;

        /// <summary>
        /// Ground normal, it accounts for steps by returning the normal of the upper floor. 
        /// </summary>
        public readonly Vector3 normal;

        /// <summary>
        /// Normal of the plane tangent to the ground and to the capsule bottom hemisphere at the contact point.
        /// </summary>
        public readonly Vector3 tangentNormal;

        /// <summary>
        /// Ground collider.
        /// </summary>
        public readonly Collider collider;

        /// <summary>
        /// True if the character should be considered on floor on this ground, false otherwise.
        /// </summary>
        public readonly bool isOnFloor;

        public GroundInfo(Vector3 point, Vector3 normal, Vector3 tangentNormal, Collider collider, bool isOnFloor)
        {
            this.point = point;
            this.normal = normal;
            this.tangentNormal = tangentNormal;
            this.collider = collider;
            this.isOnFloor = isOnFloor;
        }
    }
}
