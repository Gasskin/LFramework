using System.Collections.Generic;
using Entitas;
using Game.Utility;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Entitas
{
    public class GameObjectSystem : ReactiveSystem<GameEntity>, IInitializeSystem, ICleanupSystem
    {
        private GameContext m_GameContext;
        private IGroup<GameEntity> m_Group;
        private IObjectPool<GameObjectInstance> m_GameObjectPool;

        public GameObjectSystem(IContext<GameEntity> context) : base(context)
        {
            m_GameContext = context as GameContext;
        }

        public GameObjectSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GameObject);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject && entity.gameObject.m_GameObject == null;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                var instance = m_GameObjectPool.Spawn();
                if (instance == null)
                {
                    instance = GameObjectInstance.Create(new GameObject());
                    m_GameObjectPool.Register(instance, true);
                }
                e.gameObject.m_GameObject = instance;
            }
        }

        public void Initialize()
        {
            m_GameObjectPool = GameComponent.ObjectPool.CreateSingleSpawnObjectPool<GameObjectInstance>();

            m_Group = m_GameContext.GetGroup(GameMatcher.GameObject);
            m_Group.OnEntityRemoved += (group, entity, index, component) =>
            {
                if (component is GameObjectComponent comp)
                {
                    m_GameObjectPool.Unspawn(comp.m_GameObject);
                }
            };
        }

        public void Cleanup()
        {
        }
    }
}