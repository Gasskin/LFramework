using UnityEngine;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    /*
     * Represents a plane defined by the equation: x dot normal = d.
     */
    public struct Plane
    {
        public readonly Vector3 normal;
        public readonly float d;

        public Plane(Vector3 normal, Vector3 point, bool skipNormalization = false)
        {
            this.normal = skipNormalization ? normal : Math.Normalized(normal);
            d = Math.Dot(point, this.normal);
        }

        public Plane(Vector3 normal, float d, bool skipNormalization = false)
        {
            this.normal = skipNormalization ? normal : Math.Normalized(normal);
            this.d = d;
        }
    }
}
