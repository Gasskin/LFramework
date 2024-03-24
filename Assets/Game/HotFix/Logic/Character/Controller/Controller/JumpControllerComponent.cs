using Game.HotFix.GlobalDefinition;
using Game.HotFix.GameDrivers;
using Game.HotFix.Utility;
using UnityEngine;

namespace Game.HotFix.Logic
{
    public class JumpControllerComponent : ECComponent, ICharacterControllerComponent
    {
        public override bool DefaultEnable => false;
        private Vector2 m_EnterVelocity;
        private AttrComponent m_ControllerAttr;
        private AttrComponent m_ModelAttr;

        public override void Awake()
        {
            
        }

        public void Start(ECharacterState fromState)
        {
            m_ControllerAttr ??= Entity.GetComponent<AttrComponent>();
            m_ModelAttr ??= Entity.Parent.GetChild<CharacterModelECEntity>().GetComponent<AttrComponent>();
            
            Enable = true;

            var input = HotFixEntry.GetDriver<InputDriver>();
            var moveDir = input.MoveDir;
            var velocityY = 8f;
            var velocityX = 0f;

            if (moveDir > 0f)
                velocityX = 5f;
            else if (moveDir < 0f)
                velocityX = -5f;
            
            m_EnterVelocity = new Vector2(velocityX, velocityY);
            m_ControllerAttr.SetAttr(EControllerAttr.MoveDir.ToUint(), moveDir);
            m_ModelAttr.SetAttr(EModelAttr.IsOnGround.ToUint(), false);
        }

        public void Stop(ECharacterState toState)
        {
            Enable = false;
            m_EnterVelocity = Vector3.zero;
        }

        public override void Update()
        {
            var jumpG = 10f;
            m_EnterVelocity.y -= jumpG * TimeUtility.DeltaTime;
            if (m_EnterVelocity.y <= 0f)
                m_EnterVelocity.y = 0f;
            m_ControllerAttr.SetAttr(EControllerAttr.MoveMode.ToUint(), EMoveMode.ParabolaMove);
        }
    }
}