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
        }

        public override void Awake()
        {
            m_ControllerAttr = Entity.GetComponent<AttrComponent>();
        }

        public override void Update()
        {
        }
    }
}