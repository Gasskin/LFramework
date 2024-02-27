﻿using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class MoveComponent : EntityComponent
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
                module.Value.SetHost(Entity);
            
            m_ModelAttr = Entity.GetComponent<AttrComponent>();
            m_ControllerAttr = Entity.Parent.GetChild<CharacterControllerEntity>().GetComponent<AttrComponent>();
        }

        public override void Update()
        {
            var moveMode = m_ControllerAttr.GetAttr<EMoveMode>(EAttrType.MoveMode.ToUint());
            if (!m_MoveModuleDic.TryGetValue(moveMode, out var mode))
                return;
            mode.Move();
        }
    }
}