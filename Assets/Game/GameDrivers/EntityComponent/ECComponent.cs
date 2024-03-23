using System;
using System.Collections.Generic;

namespace Game.Module
{
    public abstract class ECComponent
    {
        // 持有该组件的实体
        public ECEntity Entity { get; set; }

        // 如果不为空，那么这个组件只能添加到这些Entity上
        public virtual List<Type> EntityLimit { get; set; }

        // 是否释放
        public bool IsDisposed { get; set; }
        
        // 默认是否启用
        public virtual bool DefaultEnable { get; set; } = true;
        
        // 是否启用
        private bool m_Enable = false;
        public bool Enable
        {
            get => m_Enable;
            set
            {
                if (m_Enable == value) return;
                m_Enable = value;
                if (m_Enable) OnEnable();
                else OnDisable();
            }
        }
        
        public T GetEntity<T>() where T : ECEntity
        {
            return Entity as T;
        }

        public virtual void Awake()
        {

        }

        public virtual void Awake(object initData)
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void LateUpdate()
        {
            
        }
        
        public virtual void OnDestroy()
        {
            
        }


        public static void Destroy(ECComponent ecComponent)
        {
            ecComponent.Enable = false;
            ecComponent.OnDestroy();
            ecComponent.IsDisposed = true;
        }
    }
}
