using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class DefaultControllerComponent: ECComponent,ICharacterControllerComponent
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
                m_ControllerAttr.SetAttr(EControllerAttr.MoveMode.ToUint(), EMoveMode.None);
                return;
            }
            m_ControllerAttr.SetAttr(EControllerAttr.MoveMode.ToUint(), EMoveMode.SpeedMove);
            m_ControllerAttr.SetAttr(EControllerAttr.MoveDir.ToUint(), moveDir);
            m_ControllerAttr.SetAttr(EControllerAttr.MoveSpeed.ToUint(), 8f);
        }
    }
}