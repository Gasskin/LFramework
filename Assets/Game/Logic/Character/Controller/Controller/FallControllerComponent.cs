using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class FallControllerComponent : EntityComponent, ICharacterControllerComponent
    {
        public override bool DefaultEnable => false;
        private AttrComponent m_ControllerAttr;

        public void Start(ECharacterState fromState)
        {
            Enable = true;
        }

        public void Stop(ECharacterState toState)
        {
            Enable = false;
            m_ControllerAttr.SetAttr(EAttrType.MoveHorizontalVelocity.ToUint(), 0f);
            m_ControllerAttr.SetAttr(EAttrType.MoveVerticalVelocity.ToUint(), 0f);
        }

        public override void Awake()
        {
            m_ControllerAttr = Entity.GetComponent<AttrComponent>();
        }

        public override void Update()
        {
            var fallGravity = 10f;
            var vertical = m_ControllerAttr.GetAttr(EAttrType.MoveVerticalVelocity.ToUint(), 0f);
            vertical -= fallGravity * GameComponent.GameUpdater.DeltaTime;
            
            m_ControllerAttr.SetAttr(EAttrType.MoveMode.ToUint(), EMoveMode.ParabolaMove);
            m_ControllerAttr.SetAttr(EAttrType.MoveVerticalVelocity.ToUint(), vertical);
        }
    }
}