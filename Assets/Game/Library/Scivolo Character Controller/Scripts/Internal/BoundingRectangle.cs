using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.Math;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    /*
     * Defines a bounding volume with rectangular section and which extends indefinitely both upward and downward.
     */
    public struct BoundingRectangle
    {
        public Vector3 center;
        public Vector3 forwardDirection;
        public Vector3 rightDirection;
        public float forwardExtent;
        public float rightExtent;

        public bool IsPointInside(Vector3 point)
        {
            Vector3 centerToPoint = point - center;

            return Mathf.Abs(Dot(centerToPoint, forwardDirection)) - forwardExtent <= 0f &&
                Mathf.Abs(Dot(centerToPoint, rightDirection)) - rightExtent <= 0f;
        }

        /// <summary>
        /// Checks if a line, assuming it starts inside the bounding rectangle, intersects the bounding rectangle surface.
        /// </summary>
        /// <returns>True if it intersect, false otherwise.</returns>
        public bool CheckLineIntersectionFromInsideToOutside(Vector3 lineStart, Vector3 lineEnd, out Vector3 intersectionPoint)
        {
            Vector3 centerToStart = lineStart - center;
            Vector3 startToEnd = lineEnd - lineStart;
            float startToEndDotForward = Dot(startToEnd, forwardDirection);
            float startToEndDotRight = Dot(startToEnd, rightDirection);

            float tMin = float.MaxValue;

            if (startToEndDotForward > 0f)
            {
                float t = -(Dot(centerToStart, forwardDirection) - forwardExtent) / startToEndDotForward;

                if (0f <= t && t <= 1f)
                    tMin = t;
            }
            else if (startToEndDotForward < 0f)
            {
                float t = (-Dot(centerToStart, forwardDirection) - forwardExtent) / startToEndDotForward;

                if (0f <= t && t <= 1f)
                    tMin = t;
            }

            if (startToEndDotRight > 0f)
            {
                float t = -(Dot(centerToStart, rightDirection) - rightExtent) / startToEndDotRight;

                if (0f <= t && t <= 1f && t < tMin)
                    tMin = t;
            }
            else if (startToEndDotRight < 0f)
            {
                float t = (-Dot(centerToStart, rightDirection) - rightExtent) / startToEndDotRight;

                if (0f <= t && t <= 1f && t < tMin)
                    tMin = t;
            }

            if (tMin <= 1f)
            {
                intersectionPoint = lineStart + tMin * startToEnd;
                return true;
            }

            intersectionPoint = default;
            return false;
        }
    }
}
