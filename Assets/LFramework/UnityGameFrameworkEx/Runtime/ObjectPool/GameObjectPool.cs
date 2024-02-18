using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class GameObjectInstance : ObjectBase
    {
        public static GameObjectInstance Create(GameObject target)
        {
            var instance = ReferencePool.Acquire<GameObjectInstance>();
            instance.Initialize(target);
            return instance;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (Target is GameObject go)
            {
                go.SetActive(true);
            }
        }

        protected override void OnUnspawn()
        {
            base.OnUnspawn();
            if (Target is GameObject go)
            {
                go.SetActive(false);
            }
        }

        protected override void Release(bool isShutdown)
        {
            var target = Target as GameObject;
            if (target == null) 
            {
                return;
            }
            
            Object.Destroy(target);
        }
    }
}