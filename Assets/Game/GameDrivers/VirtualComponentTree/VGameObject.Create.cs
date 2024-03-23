using System;
using System.Collections.Generic;

namespace Game.Module
{
    public abstract partial class VGameObject
    {
        private static MasterVGameObject MasterNode => MasterVGameObject.Instance;

        private static VGameObject NewEntity(Type entityType, long id = 0)
        {
            var entity = Activator.CreateInstance(entityType) as VGameObject;
            if (entity == null)
                return null;
            entity.InstanceId = IdFactory.NewInstanceId();
            entity.Id = id == 0 ? entity.InstanceId : id;
            if (!MasterNode.Entities.ContainsKey(entityType))
            {
                MasterNode.Entities.Add(entityType, new List<VGameObject>());
            }
            MasterNode.Entities[entityType].Add(entity);
            return entity;
        }

        public static T Create<T>() where T : VGameObject
        {
            var entity = NewEntity(typeof(T));
            SetupEntity(entity, MasterNode);
            return entity as T;
        }

        public static T Create<T>(object initData) where T : VGameObject
        {
            var entity = NewEntity(typeof(T));
            SetupEntity(entity, MasterNode, initData);
            return entity as T;
        }
        
        public static void Destroy(VGameObject vGameObject)
        {
            vGameObject.OnDestroy();
            vGameObject.Dispose();
        }

        private static void SetupEntity(VGameObject vGameObject, VGameObject parent)
        {
            parent.SetChild(vGameObject);
            vGameObject.Awake();
        }

        private static void SetupEntity(VGameObject vGameObject, VGameObject parent, object initData)
        {
            parent.SetChild(vGameObject);
            vGameObject.Awake(initData);
        }
        
        private void SetChild(VGameObject child)
        {
            Children.Add(child);
            Id2Children.Add(child.Id, child);
            if (!Type2Children.ContainsKey(child.GetType())) 
                Type2Children.Add(child.GetType(), new List<VGameObject>());
            Type2Children[child.GetType()].Add(child);
            child.SetParent(this);
        }
        
        private void SetParent(VGameObject parent)
        {
            var preParent = Parent;
            preParent?.RemoveChild(this);
            Parent = parent;
#if UNITY_EDITOR
            parent.GetComponent<GameObjectComponent>()?.OnAddChild(this);
#endif
        }
    }
}