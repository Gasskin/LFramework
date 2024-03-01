using UnityEngine;

namespace MenteBacata.ScivoloCharacterControllerDemo
{
    public class MovingPlatform : MonoBehaviour
    {
        public float speed = 2f;

        public float angularSpeed = 1f;

        private Transform start;

        private Transform end;

        private Vector3 deltaPosition;

        private Quaternion deltaRotation;

        private bool isMovingForward = true;


        private Vector3 CurrentDestination => isMovingForward ? end.position : start.position;

        private Vector3 UpDirection => transform.parent != null ? transform.parent.up : transform.up;


        private void Start()
        {
            start = transform.GetChild(0);
            end = transform.GetChild(1);

            start.SetParent(transform.parent, true);
            end.SetParent(transform.parent, true);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            deltaPosition = Vector3.MoveTowards(Vector3.zero, CurrentDestination - transform.position, speed * deltaTime);
            deltaRotation = Quaternion.AngleAxis(angularSpeed * deltaTime, UpDirection);

            transform.SetPositionAndRotation(transform.position + deltaPosition, deltaRotation * transform.rotation);

            // Invert moving direction when it reaches the destination.
            if ((CurrentDestination - transform.position).sqrMagnitude < 1E-04f)
                isMovingForward = !isMovingForward;
        }

        public void GetDeltaPositionAndRotation(out Vector3 deltaPosition, out Quaternion deltaRotation)
        {
            deltaPosition = this.deltaPosition;
            deltaRotation = this.deltaRotation;
        }
    }
}
