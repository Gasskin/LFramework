using System;
using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;
using UnityGameFramework.Runtime;
using EntityComponent = Game.Module.EntityComponent;

namespace Game.Logic
{
    public class GroundCheckComponent : EntityComponent
    {
        public override List<Type> EntityLimit => m_EntityLimit;

        private readonly List<Type> m_EntityLimit = new()
        {
            typeof(CharacterModelEntity),
        };

        private BoxCollider m_GroundCheck;
        private CharacterModelEntity m_ModelEntity;
        private CharacterControllerEntity m_ControllerEntity;
        private AttrComponent m_ModelAttr;
        private AttrComponent m_ControllerAttr;

        private Collider[] m_GroundColliders;
        
        public override void Awake()
        {
            m_ModelEntity = Entity as CharacterModelEntity;
            if (m_ModelEntity == null)
                return;
            m_GroundCheck = m_ModelEntity.Model.transform.Find("GroundCheck").GetComponent<BoxCollider>();

            m_ModelAttr = Entity.GetComponent<AttrComponent>();
            m_ControllerEntity = Entity.Parent.GetChild<CharacterControllerEntity>();
            m_ControllerAttr = m_ControllerEntity.GetComponent<AttrComponent>();
        }

        public override void Update()
        {
            var state = m_ControllerAttr.GetAttr<ECharacterState>((uint)EAttrType.CharacterState);
            switch (state)
            {
                case ECharacterState.Fall:
                case ECharacterState.Jump:
                    break;
                default:
                    return;
            }

            var isOnGround = Physics.CheckBox(m_GroundCheck.center, m_GroundCheck.size, Quaternion.identity, LayerMask.GetMask("Ground"));
            m_ModelAttr.SetAttr((uint)EModelAttr.IsOnGround, isOnGround);
        }
    }
}