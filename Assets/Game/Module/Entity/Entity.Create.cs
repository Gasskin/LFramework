using System;
using System.Collections.Generic;

namespace Game.Module
{
    public abstract partial class NodeEntity
    {
        private static MasterNodeEntity MasterNode => MasterNodeEntity.Instance;

        private static NodeEntity NewEntity(Type entityType, long id = 0)
        {
            var entity = Activator.CreateInstance(entityType) as NodeEntity;
            if (entity == null)
                return null;
            entity.InstanceId = IdFactory.NewInstanceId();
            entity.Id = id == 0 ? entity.InstanceId : id;
            if (!MasterNode.Entities.ContainsKey(entityType))
            {
                MasterNode.Entities.Add(entityType, new List<NodeEntity>());
            }
            MasterNode.Entities[entityType].Add(entity);
            return entity;
        }

        public static T Create<T>() where T : NodeEntity
        {
            var entity = NewEntity(typeof(T));
            SetupEntity(entity, MasterNode);
            return entity as T;
        }

        public static T Create<T>(object initData) where T : NodeEntity
        {
            var entity = NewEntity(typeof(T));
            SetupEntity(entity, MasterNode, initData);
            return entity as T;
        }
        
        public static void Destroy(NodeEntity nodeEntity)
        {
            nodeEntity.OnDestroy();
            nodeEntity.Dispose();
        }

        private static void SetupEntity(NodeEntity nodeEntity, NodeEntity parent)
        {
            parent.SetChild(nodeEntity);
            nodeEntity.Awake();
        }

        private static void SetupEntity(NodeEntity nodeEntity, NodeEntity parent, object initData)
        {
            parent.SetChild(nodeEntity);
            nodeEntity.Awake(initData);
        }
        
        private void SetChild(NodeEntity child)
        {
            Children.Add(child);
            Id2Children.Add(child.Id, child);
            if (!Type2Children.ContainsKey(child.GetType())) 
                Type2Children.Add(child.GetType(), new List<NodeEntity>());
            Type2Children[child.GetType()].Add(child);
            child.SetParent(this);
        }
        
        private void SetParent(NodeEntity parent)
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