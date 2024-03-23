using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Game.HotFix.GameDrivers
{
    public class MasterECEntity : ECEntity
    {
        // 记录所有的实体，和所有的组件
        public Dictionary<Type, List<ECEntity>> Entities { get; private set; } = new();
        public List<ECComponent> AllComponents { get; private set; } = new();

        // 单例
        private static MasterECEntity s_Instance;

        public static MasterECEntity Instance
        {
            get
            {
                if (s_Instance==null)
                {
                    s_Instance = new MasterECEntity();
#if UNITY_EDITOR
                    var go = s_Instance.GetComponent<GameObjectComponent>().GameObject;
                    Object.DontDestroyOnLoad(go);
#endif
                }

                return s_Instance;
            }
            private set => s_Instance = value;
        }

        // 禁止实例化
        private MasterECEntity()
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