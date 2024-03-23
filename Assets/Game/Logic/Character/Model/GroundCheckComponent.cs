using System;
using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;

namespace Game.Logic
{
    public class GroundCheckComponent : VComponent
    {
        public override List<Type> VGameObjectLimit => m_EntityLimit;

        private readonly List<Type> m_EntityLimit = new()
        {
            typeof(CharacterModelVGameObject),
        };

        private Transform m_GroundCheck;
        private CharacterModelVGameObject m_ModelVGameObject;
        private CharacterControllerVGameObject m_ControllerVGameObject;
        private AttrComponent m_ModelAttr;
        private AttrComponent m_ControllerAttr;

        private Collider[] m_GroundColliders;

        public override void Awake()
        {
            m_ModelVGameObject = VGameObject as CharacterModelVGameObject;
            if (m_ModelVGameObject == null)
                return;
            m_GroundCheck = m_ModelVGameObject.Model.transform.Find("GroundCheck");

   
        }

        public override void Update()
        {
            m_ModelAttr ??= VGameObject.GetComponent<AttrComponent>();
            m_ControllerVGameObject ??= VGameObject.Parent.GetChild<CharacterControllerVGameObject>();
            m_ControllerAttr ??= m_ControllerVGameObject.GetComponent<AttrComponent>();
            
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