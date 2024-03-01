using UnityEngine;

namespace MenteBacata.ScivoloCharacterController.Internal
{
    public struct CapsuleInflator
    {
        private CapsuleCollider capsuleCollider;
        
        private float originalRadius;
        
        private float originalHeight;

        public bool IsInflated { get; private set; }

        public CapsuleInflator(CapsuleCollider capsuleCollider)
        {
            this.capsuleCollider = capsuleCollider;
            originalRadius = capsuleCollider.radius;
            originalHeight = capsuleCollider.height;
            IsInflated = false;
        }

        public void InflateCapsule(float offset)
        {
            capsuleCollider.radius += offset;
            capsuleCollider.height += 2f * offset;
            
            IsInflated = true;
        }

        public void DeflateCapsule()
        {
            capsuleCollider.radius = originalRadius;
            capsuleCollider.height = originalHeight;
            
            IsInflated = false;
        }
    }
}
