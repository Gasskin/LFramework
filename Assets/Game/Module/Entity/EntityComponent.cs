namespace Game.Module.Entity
{
    public abstract class EntityComponent
    {
        // 持有该组件的实体
        public Entity Entity { get; set; }
        
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
        
        public T GetEntity<T>() where T : Entity
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


        public static void Destroy(EntityComponent entityComponent)
        {
            entityComponent.Enable = false;
            entityComponent.OnDestroy();
            entityComponent.IsDisposed = true;
        }
    }
}
