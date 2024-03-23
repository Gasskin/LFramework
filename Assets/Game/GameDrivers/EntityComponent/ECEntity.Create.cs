using System;
using System.Collections.Generic;

namespace Game.Module
{
    public abstract partial class ECEntity
    {
        private static MasterECEntity MasterNode => MasterECEntity.Instance;

        private static ECEntity NewEntity(Type entityType, long id = 0)
        {
            var entity = Activator.CreateInstance(entityType) as ECEntity;
            if (entity == null)
                return null;
            entity.InstanceId = IdFactory.NewInstanceId();
            entity.Id = id == 0 ? entity.InstanceId : id;
            if (!MasterNode.Entities.ContainsKey(entityType))
            {
                MasterNode.Entities.Add(entityType, new List<ECEntity>());
            }
            MasterNode.Entities[entityType].Add(entity);
            return entity;
        }

        public static T Create<T>() where T : ECEntity
        {
            var entity = NewEntity(typeof(T));
            SetupEntity(entity, MasterNode);
            return entity as T;
        }

        public static T Create<T>(object initData) where T : ECEntity
        {
            var entity = NewEntity(typeof(T));
            SetupEntity(entity, MasterNode, initData);
            return entity as T;
        }
        
        public static void Destroy(ECEntity ecEntity)
        {
            ecEntity.OnDestroy();
            ecEntity.Dispose();
        }

        private static void SetupEntity(ECEntity ecEntity, ECEntity parent)
        {
            parent.SetChild(ecEntity);
            ecEntity.Awake();
        }

        private static void SetupEntity(ECEntity ecEntity, ECEntity parent, object initData)
        {
            parent.SetChild(ecEntity);
            ecEntity.Awake(initData);
        }
        
        private void SetChild(ECEntity child)
        {
            Children.Add(child);
            Id2Children.Add(child.Id, child);
            if (!Type2Children.ContainsKey(child.GetType())) 
                Type2Children.Add(child.GetType(), new List<ECEntity>());
            Type2Children[child.GetType()].Add(child);
            child.SetParent(this);
        }
        
        private void SetParent(ECEntity parent)
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