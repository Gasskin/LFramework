using System;
using System.Collections.Generic;
using Entitas;
using Game.Utility;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Game.Entitas
{
    public class ViewSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private GameContext m_Context;
        private IGroup<GameEntity> m_Group;
        private IObjectPool<GameObjectInstance> m_GameObjectPool;
        
        public ViewSystem(IContext<GameEntity> context) : base(context)
        {
            m_Context = context as GameContext;
        }

        public ViewSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }
        
        public void Initialize()
        {
            var pool = GameComponent.ObjectPool.GetObjectPool<GameObjectInstance>("GameObjectPool");
            if (pool == null)
            {
                pool = GameComponent.ObjectPool.CreateSingleSpawnObjectPool<GameObjectInstance>("GameObjectPool");
            }
            m_GameObjectPool = pool;
            
            m_Group = m_Context.GetGroup(GameMatcher.View);
            m_Group.OnEntityRemoved += (group, entity, index, component) =>
            {
                if (component is ViewComponent { m_View: not null } comp)
                {
                    m_GameObjectPool.Unspawn(comp.m_View);
                }
            };
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.View);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasView;
        }

        protected override async void Execute(List<GameEntity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var e = entities[i];
                var path = e.view.m_AssetPath;
                try
                {
                    if (e.view.m_View != null)
                    {
                        m_GameObjectPool.Unspawn(e.view.m_View);
                        e.view.m_View = null;
                    }
                    
                    var spawn = m_GameObjectPool.Spawn(path);
                    if (spawn == null) 
                    {
                        var asset = await GameComponent.Resource.LoadAssetAsync<GameObject>(path);
                        var instance = Object.Instantiate(asset);
                        spawn = GameObjectInstance.Create(path,instance);
                        m_GameObjectPool.Register(spawn, true);
                        
                        GameComponent.Resource.UnloadAsset(asset);
                    }
                    e.view.m_View = spawn;
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                    throw;
                }
            }
        }
    }
}