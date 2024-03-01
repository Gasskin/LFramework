//#define MB_DEBUG

using UnityEngine;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public static class Math
    {
        public const float epsilon = 1E-04f;

        public static bool IsCircaZero(float x)
        {
            return -epsilon < x && x < epsilon;
        }

        public static bool IsCircaZero(Vector3 v)
        {
            return -epsilon < v.x && v.x < epsilon &&
                -epsilon < v.y && v.y < epsilon &&
                -epsilon < v.z && v.z < epsilon;
        }

        public static float Dot(Vector3 v, Vector3 w)
        {
            return v.x * w.x + v.y * w.y + v.z * w.z;
        }

        public static Vector3 Cross(Vector3 v, Vector3 w)
        {
            return new Vector3(v.y * w.z - v.z * w.y, v.z * w.x - v.x * w.z, v.x * w.y - v.y * w.x);
        }

        /// <summary>
        /// Gets the vector magnitude.
        /// </summary>
        public static float Magnitude(Vector3 v)
        {
            return (float)System.Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        /// <summary>
        /// Gets the normalized vector.
        /// </summary>
        public static Vector3 Normalized(Vector3 v)
        {
            float m = Magnitude(v);

            if (m < epsilon)
                return Vector3.zero;

            return new Vector3(v.x / m, v.y / m, v.z / m);
        }

        /// <summary>
        /// Gets magnitude and direction of the given vector.
        /// </summary>
        public static void MagnitudeAndDirection(Vector3 v, out float magnitude, out Vector3 direction)
        {
            magnitude = Magnitude(v);

            if (magnitude < epsilon)
            {
                direction = new Vector3(0f, 0f, 0f);
                return;
            }

            direction = new Vector3(v.x / magnitude, v.y / magnitude, v.z / magnitude);
        }

        public static void ClampMagnitude(ref Vector3 v, float maxLength)
        {
            float sqrMag = v.x * v.x + v.y * v.y + v.z * v.z;

            if (sqrMag > maxLength * maxLength)
            {
                float mag = (float)System.Math.Sqrt(sqrMag);
                v = new Vector3(maxLength * v.x / mag, maxLength * v.y / mag, maxLength * v.z / mag);
            }
        }

        /// <summary>
        /// Projects the vector onto the given direction. Direction must be a unit vector.
        /// </summary>
        public static Vector3 ProjectOnDirection(Vector3 v, Vector3 direction)
        {
            return Dot(v, direction) * direction;
        }
        
        /// <summary>
        /// Projects the first vector onto the direction of the second.
        /// </summary>
        public static Vector3 ProjectOnVector(Vector3 v, Vector3 w)
        {
            return (Dot(v, w) / Dot(w, w)) * w;
        }

        /// <summary>
        /// Orthogonally projects the vector onto the plane defined by its normal. Normal must be a unit vector.
        /// </summary>
        public static Vector3 ProjectOnPlane(Vector3 v, Vector3 normal)
        {
            return v - Dot(v, normal) * normal;
        }

        /// <summary>
        /// Orthogonally projects the vector onto the plane.
        /// </summary>
        public static Vector3 ProjectOnPlane(Vector3 v, in Plane plane)
        {
            return v + (plane.d - Dot(v, plane.normal)) * plane.normal;
        }

        /// <summary>
        /// Projects the vector onto the plane defined by its normal and along the given direction. Both normal and direction 
        /// must be unit vectors.
        /// </summary>
        public static Vector3 ProjectOnPlane(Vector3 v, Vector3 normal, Vector3 direction)
        {
            return v - (Dot(v, normal) / Dot(normal, direction)) * direction;
        }
    } 
}
