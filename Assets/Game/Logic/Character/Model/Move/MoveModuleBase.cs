using Game.Module;
using MenteBacata.ScivoloCharacterController;
using UnityEngine;
using UnityGameFramework.Runtime;
using Entity = Game.Module.Entity;

namespace Game.Logic
{
    public abstract class MoveModuleBase
    {
        protected Entity Host { get; private set; }
        protected AttrComponent ModelAttr { get; private set; }
        protected AttrComponent ControllerAttr { get; private set; }

        public Collider[] Overlaps { get; private set; } = new Collider[5];
        public int OverlapCount { get; private set; }
        public MoveContact[] MoveContacts { get; private set; } = CharacterMover.NewMoveContactArray;
        public int ContactCount { get; private set; }
        public bool HasGround { get; private set; }
        public GroundInfo GroundInfo { get; private set; }
        public Vector3 Forward => m_CharacterCapsule.transform.forward;
        
        private CharacterCapsule m_CharacterCapsule;
        private CharacterMover m_CharacterMover;
        private GroundDetector m_GroundDetector;

        public abstract void Move();

        public void SetHost(Entity host)
        {
            Host = host;

            if (Host is CharacterModelEntity modelEntity)
            {
                m_CharacterCapsule = modelEntity.Model.GetComponent<CharacterCapsule>();
                m_CharacterMover = modelEntity.Model.GetComponent<CharacterMover>();
                m_GroundDetector = modelEntity.Model.GetComponent<GroundDetector>();

                if (m_CharacterCapsule == null || m_CharacterMover == null || m_GroundDetector == null)
                    Log.Error("character prepare failed!");

                ModelAttr = modelEntity.GetComponent<AttrComponent>();
                ControllerAttr = modelEntity.Parent.GetChild<CharacterControllerEntity>().GetComponent<AttrComponent>();
            }
            else
            {
                Log.Error("move module can only add to model entity");
            }
        }

        public void SetMoverMode(CharacterMover.Mode mode)
        {
            m_CharacterMover.mode = mode;
        }

        public bool DetectGround()
        {
            HasGround = m_GroundDetector.DetectGround(out var info);
            if (HasGround)
                GroundInfo = info;
            return HasGround;
        }

        public bool TryResolveOverlap()
        {
            var flag = m_CharacterCapsule.TryResolveOverlap();
            if (flag)
                OverlapCount = m_CharacterCapsule.CollectOverlaps(Overlaps);
            return flag;
        }

        public void DoMove(Vector3 movement)
        {
            TryResolveOverlap();
            DetectGround();
            m_CharacterMover.Move(movement, HasGround, GroundInfo, OverlapCount, Overlaps, MoveContacts, out var count);
            ContactCount = count;
        }
    }
}