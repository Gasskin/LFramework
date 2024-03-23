using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class MoveComponent : VComponent
    {
        private readonly Dictionary<EMoveMode, MoveModuleBase> m_MoveModuleDic = new();
        private EMoveMode m_CurrentMoveMode;
        private AttrComponent m_ModelAttr;
        private AttrComponent m_ControllerAttr;

        public override void Awake()
        {
            m_MoveModuleDic.Add(EMoveMode.SpeedMove, new SpeedMove());
            m_MoveModuleDic.Add(EMoveMode.ParabolaMove, new ParabolaMove());
            foreach (var module in m_MoveModuleDic)
                module.Value.SetHost(VGameObject);
        }

        public override void Update()
        {
            PrepareComponent();
            
            var moveMode = m_ControllerAttr.GetAttr<EMoveMode>(EControllerAttr.MoveMode.ToUint());
            if (!m_MoveModuleDic.TryGetValue(moveMode, out var mode))
                return;
            mode.Move();
        }

        private void PrepareComponent()
        {
            m_ModelAttr ??= VGameObject.GetComponent<AttrComponent>();
            m_ControllerAttr ??= VGameObject.Parent.GetChild<CharacterControllerVGameObject>().GetComponent<AttrComponent>();
        }
    }
}