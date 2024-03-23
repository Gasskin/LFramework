using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.GameUpdater
{
    internal sealed class GameDriverManager : GameFrameworkModule, IGameDriverManager
    {
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        internal override void Shutdown()
        {
            for (var current = m_LFrameworkDrivers.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }

            m_LFrameworkDrivers.Clear();
        }
        

        private readonly GameFrameworkLinkedList<GameDriverBase> m_LFrameworkDrivers = new();

        public async UniTask InitAsync()
        {
            foreach (var module in m_LFrameworkDrivers)
            {
                await module.InitAsync();
            }
        }

        public T GetModule<T>() where T : GameDriverBase
        {
            var type = typeof(T);
            foreach (var module in m_LFrameworkDrivers)
            {
                if (module.GetType() == type)
                {
                    return module as T;
                }
            }

            return CreateModule(type) as T;
        }

        private GameDriverBase CreateModule(Type moduleType)
        {
            var module = (GameDriverBase)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create module '{0}'.",
                    moduleType.FullName));
            }

            LinkedListNode<GameDriverBase> current = m_LFrameworkDrivers.First;
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
                m_LFrameworkDrivers.AddBefore(current, module);
            }
            else
            {
                m_LFrameworkDrivers.AddLast(module);
            }

            return module;
        }

        public void Update(float delta)
        {
            foreach (var module in m_LFrameworkDrivers)
            {
                module.Update(delta);
            }
        }

        public void LateUpdate()
        {
            foreach (var module in m_LFrameworkDrivers)
            {
                module.LateUpdate();
            }
        }

        public void FixedUpdate()
        {

            foreach (var module in m_LFrameworkDrivers)
            {
                module.FixedUpdate();
            }
        }
    }
}