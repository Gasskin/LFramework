using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.Math;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public static class GeometricTests
    {
        /// <summary>
        /// Checks for the intersection between a line and a plane.
        /// </summary>
        public static bool CheckLineAndPlaneIntersection(Vector3 lineStart, Vector3 lineEnd, in Plane plane, out Vector3 intersectionPoint)
        {
            float planeD = plane.d;
            Vector3 planeNormal = plane.normal;

            float startDotNormal = Dot(lineStart, planeNormal);
            float endDotNormal = Dot(lineEnd, planeNormal);

            if (startDotNormal == planeD)
            {
                intersectionPoint = lineStart;
                return true;
            }
            else if (endDotNormal == planeD)
            {
                intersectionPoint = lineEnd;
                return true;
            }
            else if ((startDotNormal > planeD) ^ (endDotNormal > planeD))
            {
                float t = (planeD - startDotNormal) / (endDotNormal - startDotNormal);

                // Better be safe here...
                if (t <= 0f)
                    intersectionPoint = lineStart;
                else if (t >= 1f)
                    intersectionPoint = lineEnd;
                else
                    intersectionPoint = lineStart + t * (lineEnd - lineStart);

                return true;
            }

            intersectionPoint = default;
            return false;
        }

        /// <summary>
        /// Checks if the point is in front of the plane.
        /// </summary>
        public static bool IsPointInFrontOfPlane(Vector3 point, in Plane plane)
        {
            return Dot(point, plane.normal) > plane.d;
        }
    }
}
