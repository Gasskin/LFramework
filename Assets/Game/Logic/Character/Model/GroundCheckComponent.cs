using System;
using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;
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

        private Transform m_GroundCheck;
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
            m_GroundCheck = m_ModelEntity.Model.transform.Find("GroundCheck");

   
        }

        public override void Update()
        {
            m_ModelAttr ??= Entity.GetComponent<AttrComponent>();
            m_ControllerEntity ??= Entity.Parent.GetChild<CharacterControllerEntity>();
            m_ControllerAttr ??= m_ControllerEntity.GetComponent<AttrComponent>();
            
            var state = m_ControllerAttr.GetAttr<ECharacterState>((uint)EControllerAttr.CharacterState);
            switch (state)
            {
                case ECharacterState.Fall:
                    break;
                default:
                    return;
            }

            var ray = new Ray(m_GroundCheck.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, 1.2f, LayerMask.GetMask("Ground")))
            {
                m_ModelAttr.SetAttr((uint)EModelAttr.IsOnGround, true);
            }
        }
    }
}