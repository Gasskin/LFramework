using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFramework;

namespace LFramework
{
    public static class LFrameworkEntry
    {
        private static readonly GameFrameworkLinkedList<LFrameworkModule> s_LFrameworkModules = new();
        private static bool s_Initialized = false;

        public static async UniTask InitAsync()
        {
            foreach (var module in s_LFrameworkModules)
            {
                await module.InitAsync();
            }

            s_Initialized = true;
        }

        public static T GetModule<T>() where T : LFrameworkModule
        {
            var type = typeof(T);
            foreach (var module in s_LFrameworkModules)
            {
                if (module.GetType() == type)
                {
                    return module as T;
                }
            }

            return CreateModule(type) as T;
        }

        private static LFrameworkModule CreateModule(Type moduleType)
        {
            var module = (LFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create module '{0}'.",
                    moduleType.FullName));
            }

            LinkedListNode<LFrameworkModule> current = s_LFrameworkModules.First;
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
                s_LFrameworkModules.AddBefore(current, module);
            }
            else
            {
                s_LFrameworkModules.AddLast(module);
            }

            return module;
        }

        public static void Update()
        {
            if (!s_Initialized)
            {
                return;
            }
            
            foreach (var module in s_LFrameworkModules)
            {
                module.Update();
            }
        }

        public static void LateUpdate()
        {
            if (!s_Initialized)
            {
                return;
            }
            
            foreach (var module in s_LFrameworkModules)
            {
                module.LateUpdate();
            }
        }

        public static void FixedUpdate()
        {
            if (!s_Initialized)
            {
                return;
            }
            
            foreach (var module in s_LFrameworkModules)
            {
                module.FixedUpdate();
            }
        }
        
        public static void Shutdown()
        {
            if (!s_Initialized)
            {
                return;
            }
            
            for (var current = s_LFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }

            s_LFrameworkModules.Clear();
        }
    }
}