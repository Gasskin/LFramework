using System;
using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;

namespace Game.Logic
{
    public class GroundCheckComponent : ECComponent
    {
        public override List<Type> EntityLimit => m_EntityLimit;

        private readonly List<Type> m_EntityLimit = new()
        {
            typeof(CharacterModelECEntity),
        };

        private Transform m_GroundCheck;
        private CharacterModelECEntity m_ModelEcEntity;
        private CharacterControllerEcEntity m_ControllerEcEntity;
        private AttrComponent m_ModelAttr;
        private AttrComponent m_ControllerAttr;

        private Collider[] m_GroundColliders;

        public override void Awake()
        {
            m_ModelEcEntity = Entity as CharacterModelECEntity;
            if (m_ModelEcEntity == null)
                return;
            m_GroundCheck = m_ModelEcEntity.Model.transform.Find("GroundCheck");

   
        }

        public override void Update()
        {
            m_ModelAttr ??= Entity.GetComponent<AttrComponent>();
            m_ControllerEcEntity ??= Entity.Parent.GetChild<CharacterControllerEcEntity>();
            m_ControllerAttr ??= m_ControllerEcEntity.GetComponent<AttrComponent>();
            
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