using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class DefaultControllerComponent: EntityComponent,ICharacterControllerComponent
    {
        public override bool DefaultEnable => false;
        private AttrComponent m_ControllerAttr;

        public void Enter(ECharacterState fromState)
        {
            Enable = true;
        }

        public void Exit(ECharacterState toState)
        {
            Enable = false;
        }
        
        public override void Awake()
        {
            m_ControllerAttr = Entity.GetComponent<AttrComponent>();
        }

        public override void Update()
        {
            var moveDir = GameModule.Input.MoveDir;
            if (moveDir == 0f)
            {
                m_ControllerAttr.SetAttr(EAttrType.MoveMode.ToUint(), EMoveMode.None);
                return;
            }
            m_ControllerAttr.SetAttr(EAttrType.MoveMode.ToUint(), EMoveMode.SpeedMove);
            m_ControllerAttr.SetAttr(EAttrType.MoveDir.ToUint(), moveDir);
            m_ControllerAttr.SetAttr(EAttrType.MoveHorizontalVelocity.ToUint(), 4f);
        }
    }
}