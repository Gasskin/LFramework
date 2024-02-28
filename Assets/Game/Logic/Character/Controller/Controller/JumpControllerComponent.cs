using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;

namespace Game.Logic
{
    public class JumpControllerComponent : EntityComponent, ICharacterControllerComponent
    {
        public override bool DefaultEnable => false;
        private Vector2 m_EnterVelocity;
        private AttrComponent m_ControllerAttr;

        public override void Awake()
        {
            m_ControllerAttr = Entity.GetComponent<AttrComponent>();
        }

        public void Start(ECharacterState fromState)
        {
            Enable = true;
            var moveDir = GameModule.Input.MoveDir;
            var velocityY = 5f;
            var velocityX = 0f;

            if (moveDir > 0f)
                velocityX = 4f;
            else if (moveDir < 0f)
                velocityX = -4f;
            
            m_EnterVelocity = new Vector2(velocityX, velocityY);
            m_ControllerAttr.SetAttr(EAttrType.MoveDir.ToUint(), moveDir);
        }

        public void Stop(ECharacterState toState)
        {
            Enable = false;
            m_EnterVelocity = Vector3.zero;
        }

        public override void Update()
        {
            var jumpG = 10f;
            m_EnterVelocity.y -= jumpG * GameComponent.GameUpdater.DeltaTime;
            if (m_EnterVelocity.y <= 0f)
                m_EnterVelocity.y = 0f;
            m_ControllerAttr.SetAttr(EAttrType.MoveMode.ToUint(), EMoveMode.ParabolaMove);
            m_ControllerAttr.SetAttr(EAttrType.MoveHorizontalVelocity.ToUint(), m_EnterVelocity.x);
            m_ControllerAttr.SetAttr(EAttrType.MoveVerticalVelocity.ToUint(), m_EnterVelocity.y);
        }
    }
}