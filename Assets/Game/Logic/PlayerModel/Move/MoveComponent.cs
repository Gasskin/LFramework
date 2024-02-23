using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class MoveComponent: EntityComponent,IPlayerControllerComponent
    {
        public override bool DefaultEnable => false;

        private readonly Dictionary<EMoveMode, MoveModuleBase> m_MoveModuleDic = new();
        private EMoveMode m_CurrentMoveMode;
        
        public override void Awake()
        {
            m_MoveModuleDic.Add(EMoveMode.SpeedMove, new SpeedMove());

            foreach (var module in m_MoveModuleDic)
                module.Value.SetHost(Entity);
        }
        
        public override void Update()
        {
            var attrComponent = Entity.GetComponent<AttrComponent>();
            if (attrComponent == null) 
                return;
            var moveMode = (EMoveMode)attrComponent.GetAttr<int>((uint)EAttrType.MoveMode);
            if (!m_MoveModuleDic.TryGetValue(moveMode,out var mode))
                return;
            mode.Move();
        }

        public void Enter(EPlayerState fromState)
        {
            Enable = true;
        }

        public void Exit(EPlayerState toState)
        {
            Enable = false;
        }
    }
}