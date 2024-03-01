using UnityEngine;
using static MenteBacata.ScivoloCharacterController.Internal.Math;
using static MenteBacata.ScivoloCharacterController.Internal.MovementSurfaceUtils;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public struct MovementResolver
    {
        private Vector3 upDirection;

        private float minFloorUp;

        public MovementResolver(Vector3 upDirection, float minFloorUp)
        {
            this.upDirection = upDirection;
            this.minFloorUp = minFloorUp;
        }

        /// <summary>
        /// Gets the resulting movement, constrained above a surface, by handling the projection of the given movement on the surface.
        /// </summary>
        /// <param name="normal">Surface normal.</param>
        /// <param name="forceFloorProjection">Always projects on the surface if it is floor.</param>
        public Vector3 GetMovementOneSurface(Vector3 movement, Vector3 normal, bool canClimbSteepSlope, bool forceFloorProjection)
        {
            MovementProjector projector = new MovementProjector(upDirection, minFloorUp, !canClimbSteepSlope);

            if ((forceFloorProjection && GetMovementSurface(normal, upDirection, minFloorUp) == MovementSurface.Floor) || Dot(movement, normal) < 0f)
            {
                return projector.ProjectOnSurface(movement, normal);
            }

            return movement;
        }

        /// <summary>
        /// Gets the resulting movement, constrained within two surfaces, by handling the projection of the given movement on one of the two 
        /// surfaces or on their intersection. The order in which the normals are provided affects the result, the projection on the first 
        /// normal has the precedence.
        /// </summary>
        /// <param name="normal1">First surface normal.</param>
        /// <param name="normal2">Second surface normal.</param>
        /// <param name="forceFloorProjection">Always projects on at least one floor surface if there are any.</param>
        public Vector3 GetMovementTwoSurfaces(Vector3 movement, Vector3 normal1, Vector3 normal2, bool canClimbSteepSlope, bool forceFloorProjection)
        {
            Vector3 result;

            MovementProjector projector = new MovementProjector(upDirection, minFloorUp, !canClimbSteepSlope);

            if (forceFloorProjection && IsThereAnyFloor(normal1, normal2, out bool isFloor1, out bool isFloor2))
            {
                // If is a floor, it always projects on the first surface.
                if (isFloor1)
                {
                    result = projector.ProjectOnSurface(movement, normal1);

                    if (Dot(result, normal2) >= 0f)
                        return result;
                }

                // Only projects on the second surface if the first one isn't a floor or projection on it has not succeeded.
                if (isFloor2)
                {
                    result = projector.ProjectOnSurface(movement, normal2);

                    if (Dot(result, normal1) >= 0f)
                        return result;
                }
            }
            else
            {
                float movementDotNormal1 = Dot(movement, normal1);
                float movementDotNormal2 = Dot(movement, normal2);

                // Gets the movement as it is.
                if (movementDotNormal1 >= 0f && movementDotNormal2 >= 0f)
                    return movement;

                // Gets the movement projection on the first surface.
                if (movementDotNormal1 < 0f)
                {
                    result = projector.ProjectOnSurface(movement, normal1);

                    if (Dot(result, normal2) >= 0f)
                        return result;
                }

                // Gets the movement projection on the second surface.
                if (movementDotNormal2 < 0f)
                {
                    result = projector.ProjectOnSurface(movement, normal2);

                    if (Dot(result, normal1) >= 0f)
                        return result;
                }
            }

            if (projector.TryProjectOnSurfacesIntersection(movement, normal1, normal2, out result))
                return result;

            return new Vector3(0f, 0f, 0f);
        }

        // Checks if at least one of the two surfaces is a floor.
        private bool IsThereAnyFloor(Vector3 normal1, Vector3 normal2, out bool isFloor1, out bool isFloor2)
        {
            isFloor1 = GetMovementSurface(normal1, upDirection, minFloorUp) == MovementSurface.Floor;
            isFloor2 = GetMovementSurface(normal2, upDirection, minFloorUp) == MovementSurface.Floor;

            return isFloor1 || isFloor2;
        }
    }
}
