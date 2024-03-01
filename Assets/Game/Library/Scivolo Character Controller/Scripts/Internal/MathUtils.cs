using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.Math;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    /// <summary>
    /// Collection of mathematical methods which are less canonical than those in the main Math class.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Gets the normal of the plane passing through three points.
        /// </summary>
        public static Vector3 NormalFromThreePoints(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            return Normalized(Cross(point1 - point2, point1 - point3));
        }

        /// <summary>
        /// Computes the vertices of an equilateral triangle given its center, axes and size.
        /// </summary>
        public static void ComputeTriangleVertices(Vector3 center, Vector3 forward, Vector3 right, float size,
            out Vector3 a, out Vector3 b, out Vector3 c)
        {
            const float cos60 = 0.5f;
            const float sen60 = 0.866f;

            a = size * (sen60 * right + -cos60 * forward) + center;
            b = size * (-sen60 * right + -cos60 * forward) + center;
            c = size * forward + center;
        }

        /// <summary>
        /// Calculates a vector of steepest descent laying on a plane given its downward component.
        /// </summary>
        public static void CalculateSteepestDescentVector(Vector3 planeNormal, float downwardDistance, Vector3 upDirection, out Vector3 descentDirection, out float descentLenght)
        {
            descentDirection = Normalized(ProjectOnPlane(-upDirection, planeNormal));
            descentLenght = downwardDistance / -Dot(descentDirection, upDirection);
        }

        /// <summary>
        /// Checks if a point is within a given distance from a line.
        /// </summary>
        public static bool IsPointWithinDistanceFromLine(Vector3 point, Vector3 linePoint, Vector3 lineDirection, float distance)
        {
            return ProjectOnPlane(point - linePoint, lineDirection).sqrMagnitude <= distance * distance;
        }

        /// <summary>
        /// Resizes the given vector so that it has the same vertical component as the target vector.
        /// </summary>
        /// <returns>False if failed to resize, that is the given vector vertical component is zero.</returns>
        public static bool TryResizeToTargetVerticalComponent(Vector3 v, Vector3 target, Vector3 upDirection, out Vector3 resized)
        {
            float verticalComponent = Dot(v, upDirection);

            if (IsCircaZero(verticalComponent))
            {
                resized = default;
                return false;
            }

            float scaling = Dot(target, upDirection) / verticalComponent;

            resized = scaling * v;
            return true;
        }
    }
}
