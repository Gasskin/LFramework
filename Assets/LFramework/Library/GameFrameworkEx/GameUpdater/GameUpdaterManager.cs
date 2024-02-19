using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.GameUpdater
{
    internal sealed class GameUpdaterManager : GameFrameworkModule, IGameUpdaterManager
    {
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        internal override void Shutdown()
        {
            for (var current = m_LFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }

            m_LFrameworkModules.Clear();
        }
        

        private readonly GameFrameworkLinkedList<GameModuleBase> m_LFrameworkModules = new();

        public async UniTask InitAsync()
        {
            foreach (var module in m_LFrameworkModules)
            {
                await module.InitAsync();
            }
        }

        public T GetModule<T>() where T : GameModuleBase
        {
            var type = typeof(T);
            foreach (var module in m_LFrameworkModules)
            {
                if (module.GetType() == type)
                {
                    return module as T;
                }
            }

            return CreateModule(type) as T;
        }

        private GameModuleBase CreateModule(Type moduleType)
        {
            var module = (GameModuleBase)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create module '{0}'.",
                    moduleType.FullName));
            }

            LinkedListNode<GameModuleBase> current = m_LFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_LFrameworkModules.AddBefore(current, module);
            }
            else
            {
                m_LFrameworkModules.AddLast(module);
            }

            return module;
        }

        public void Update(float delta)
        {
            foreach (var module in m_LFrameworkModules)
            {
                module.Update(delta);
            }
        }

        public void LateUpdate()
        {
            foreach (var module in m_LFrameworkModules)
            {
                module.LateUpdate();
            }
        }

        public void FixedUpdate()
        {

            foreach (var module in m_LFrameworkModules)
            {
                module.FixedUpdate();
            }
        }
    }
}