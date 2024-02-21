using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Game.Module.Entity
{
    public class MasterEntity : Entity
    {
        // 记录所有的实体，和所有的组件
        public Dictionary<Type, List<Entity>> Entities { get; private set; } = new();
        public List<EntityComponent> AllComponents { get; private set; } = new();

        // 单例
        private static MasterEntity s_Instance;

        public static MasterEntity Instance
        {
            get
            {
                if (s_Instance==null)
                {
                    s_Instance = new MasterEntity();
                    var go = s_Instance.AddComponent<GameObjectComponent>().GameObject;
                    Object.DontDestroyOnLoad(go);
                }

                return s_Instance;
            }
            private set => s_Instance = value;
        }

        // 禁止实例化
        private MasterEntity()
        {
            
        }

        public void Create()
        {
            
        }
        
        public void Destroy()
        {
            Destroy(Instance);
            Instance = null;
        }

        public override void Update()
        {
            if (AllComponents.Count == 0)
            {
                return;
            }
            for (int i = AllComponents.Count - 1; i >= 0; i--)
            {
                var component = AllComponents[i];
                if (component.IsDisposed)
                {
                    AllComponents.RemoveAt(i);
                    continue;
                }
                if (!component.Enable)
                {
                    continue;
                }
                component.Update();
            }
        }
        
        public override void LateUpdate()
        {
            if (AllComponents.Count == 0)
            {
                return;
            }
            for (int i = AllComponents.Count - 1; i >= 0; i--)
            {
                var component = AllComponents[i];
                if (component.IsDisposed)
                {
                    AllComponents.RemoveAt(i);
                    continue;
                }
                if (!component.Enable)
                {
                    continue;
                }
                component.LateUpdate();
            }
        }
    }
}