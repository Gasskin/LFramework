//#define MB_DEBUG

using UnityEngine;
using MenteBacata.ScivoloCharacterController.Internal;
using static MenteBacata.ScivoloCharacterController.Internal.OverlapResolver;

namespace MenteBacata.ScivoloCharacterController
{
    public class CharacterCapsule : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Height of the capsule.")]
        private float height = 2f;

        [SerializeField]
        [Tooltip("Raidus of the capsule.")]
        [Min(0f)]
        private float radius = 0.5f;

        [SerializeField]
        [Tooltip("Vertical offset from the game object position to the bottom of the capsule.")]
        private float verticalOffset = 0f;

        [Min(0f)]
        [Tooltip("Small distance from the surface of the capsule used as a safety margin to avoid that the capsule is directly in contact with other colliders.")]
        public float contactOffset = 0.01f;

        [Tooltip("Overlaps with colliders in layers excluded from this mask will be ignored if the attempt to resolve all overlaps fails.")]
        public LayerMask prioritizedOverlap = Physics.AllLayers;

        /// <summary>
        /// If true, it uses the transform position and rotation, otherwise it uses its own position and rotation.
        /// </summary>
        [System.NonSerialized]
        public bool useTransform = true;

        /// <summary>
        /// Small margin added to the radius when retrieving overlaps.
        /// </summary>
        internal const float overlapMargin = 0.001f;

        private Vector3 _position;

        private Quaternion _rotation;

        private CapsuleCollider capsuleCollider;

        private LayerMask collisionMask;

        /// <summary>
        /// Height of the capsule.
        /// </summary>
        public float Height
        {
            get => height;
            set 
            {
                height = value;
                ValidateHeight();
                SetColliderProperties(); 
            }
        }

        /// <summary>
        /// Radius of the capsule.
        /// </summary>
        public float Radius
        {
            get => radius;
            set 
            {
                radius = value;
                ValidateHeight();
                SetColliderProperties(); 
            }
        }

        /// <summary>
        /// Vertical offset from the game object position to the bottom of the capsule.
        /// </summary>
        public float VerticalOffset
        {
            get => verticalOffset;
            set
            {
                verticalOffset = value;
                SetColliderProperties();
            }
        }

        /// <summary>
        /// Position of the capsule.
        /// </summary>
        public Vector3 Position
        {
            get => useTransform ? transform.position : _position;
            set
            {
                if (useTransform)
                    transform.position = value;
                else
                    _position = value;
            }
        }

        /// <summary>
        /// Rotation of the capsule.
        /// </summary>
        public Quaternion Rotation
        {
            get => useTransform ? transform.rotation : _rotation;
            set
            {
                if (useTransform)
                    transform.rotation = value;
                else
                    _rotation = value;
            }
        }

        /// <summary>
        /// Capsule up direction.
        /// </summary>
        public Vector3 UpDirection
        {
            get => Rotation * new Vector3(0f, 1f, 0f);
            set => Rotation = Quaternion.FromToRotation(UpDirection, value) * Rotation;
        }

        /// <summary>
        /// World space center of the capsule.
        /// </summary>
        public Vector3 Center => Position + Rotation * LocalCenter;

        /// <summary>
        /// World space center of the capsule lower hemisphere.
        /// </summary>
        public Vector3 LowerHemisphereCenter => Position + Rotation * LocalLowerHemisphereCenter;

        /// <summary>
        /// World space center of the capsule upper hemisphere.
        /// </summary>
        public Vector3 UpperHemisphereCenter => Position + Rotation * LocalUpperHemisphereCenter;

        /// <summary>
        /// Center of the capsule in the object's local space.
        /// </summary>
        public Vector3 LocalCenter => new Vector3(0f, verticalOffset + 0.5f * height, 0f);

        /// <summary>
        /// Center of the capsule lower hemisphere in the object's local space.
        /// </summary>
        public Vector3 LocalLowerHemisphereCenter => CapsuleUtils.GetLocalLowerCenter_YAxis(radius, height, LocalCenter);

        /// <summary>
        /// Center of the capsule upper hemisphere in the object's local space.
        /// </summary>
        public Vector3 LocalUpperHemisphereCenter => CapsuleUtils.GetLocalUpperCenter_YAxis(radius, height, LocalCenter);

        /// <summary>
        /// The collision layer mask.
        /// </summary>
        public LayerMask CollisionMask => collisionMask;

        /// <summary>
        /// The collider of the character capsule.
        /// </summary>
        public Collider Collider => capsuleCollider;

        /// <summary>
        /// The rigidbody of the character capsule.
        /// </summary>
        public Rigidbody Rigidbody { get; private set; }

        internal CapsuleCollider CapsuleCollider => capsuleCollider;


        private void Awake()
        {
            DoPreliminaryCheck();
            InstantiateComponents();

            collisionMask = gameObject.GetCollisionMask();
        }

        /// <summary>
        /// Checks if the character capsule overlaps any other collider in the same layer mask of the character game object.
        /// </summary>
        /// <returns>True if it overlaps, false otherwise.</returns>
        public bool CheckOverlap()
        {
            return OverlapUtils.CheckCapsuleOverlap(LowerHemisphereCenter, UpperHemisphereCenter, radius + overlapMargin, collisionMask, capsuleCollider);
        }

        /// <summary>
        /// Checks if the character capsule overlaps any other colliders in the given layer mask.
        /// </summary>
        /// <returns>True if it overlaps, false otherwise.</returns>
        public bool CheckOverlap(LayerMask layerMask)
        {
            return OverlapUtils.CheckCapsuleOverlap(LowerHemisphereCenter, UpperHemisphereCenter, radius + overlapMargin, layerMask, capsuleCollider);
        }

        /// <summary>
        /// Collects the colliders that overlap the character capsule in the same layer mask.
        /// </summary>
        /// <returns>Overlapping colliders count.</returns>
        public int CollectOverlaps(Collider[] overlaps)
        {
            return OverlapUtils.OverlapCapsule(LowerHemisphereCenter, UpperHemisphereCenter, radius + overlapMargin, overlaps, collisionMask, capsuleCollider);
        }

        /// <summary>
        /// Collects the colliders that overlap the character capsule.
        /// </summary>
        /// <returns>Overlapping colliders count.</returns>
        public int CollectOverlaps(Collider[] overlaps, LayerMask layerMask)
        {
            return OverlapUtils.OverlapCapsule(LowerHemisphereCenter, UpperHemisphereCenter, radius + overlapMargin, overlaps, layerMask, capsuleCollider);
        }

        /// <summary>
        /// Tries to resolve overlaps with every colliders it is supposed to collide with. If the first attempt fails, it tries again 
        /// considering only the high priority colliders.
        /// </summary>
        /// <returns>True if it managed to resolve all overlaps, false otherwise.</returns>
        public bool TryResolveOverlap()
        {
            Vector3 position = Position;
            Quaternion rotation = Rotation;

            if (TryResolveCapsuleOverlap(position, rotation, capsuleCollider, overlapMargin, contactOffset, collisionMask, out Vector3 newPosition))
            {
                Position = newPosition;
                return true;
            }

            LayerMask prioritizedCollisionMask = collisionMask & prioritizedOverlap;

            if (prioritizedCollisionMask == collisionMask)
            {
                Position = newPosition;
                return false;
            }

            TryResolveCapsuleOverlap(position, rotation, capsuleCollider, overlapMargin, contactOffset, prioritizedCollisionMask, out newPosition);

            Position = newPosition;
            return false;
        }

        private void DoPreliminaryCheck()
        {
            if (!Mathf.Approximately(transform.lossyScale.x, 1f) ||
                !Mathf.Approximately(transform.lossyScale.y, 1f) ||
                !Mathf.Approximately(transform.lossyScale.z, 1f))
            {
                Debug.LogWarning($"{nameof(CharacterCapsule)}: Object scale is not (1, 1, 1).");
            }
            
            foreach (var col in gameObject.GetComponentsInChildren<Collider>(true))
            {
                if (col != capsuleCollider && !col.isTrigger && !Physics.GetIgnoreLayerCollision(gameObject.layer, col.gameObject.layer))
                {
                    Debug.LogWarning($"{nameof(CharacterCapsule)}: Found other colliders on this gameobject or in its childrens.");
                    break;
                }
            }
        }

        private void InstantiateComponents()
        {
            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            SetColliderProperties();

            Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.isKinematic = true;
        }

        private void ValidateHeight()
        {
            if (height >= 2f * radius)
                return;

            height = 2f * radius;
        }

        private void SetColliderProperties()
        {
            if (capsuleCollider is null)
                return;

            capsuleCollider.center = LocalCenter;
            capsuleCollider.height = height;
            capsuleCollider.radius = radius;
            capsuleCollider.direction = 1; // Y-Axis
        }

        private void OnValidate()
        {
            ValidateHeight();

            if (capsuleCollider != null)
                SetColliderProperties();
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying == false)
            {
                Gizmos.color = GizmosUtility.defaultColliderColor;
                GizmosUtility.DrawWireCapsule(LowerHemisphereCenter, UpperHemisphereCenter, radius);
            }
        }
    }
}
