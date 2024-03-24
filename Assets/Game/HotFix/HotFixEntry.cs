using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.HotFix.GameDrivers;
using Game.HotFix.Utility;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Game.HotFix
{
    public class HotFixEntry
    {
        public static HotFixEntry s_Instance;

        private List<GameDriverBase> m_GameDrivers = new();
        private Dictionary<Type, GameDriverBase> m_GameDriverDic = new();
        
        public static async UniTaskVoid Start()
        {
            if (s_Instance != null) 
            {
                Log.Error("重复初始化HotFixEntry？？？");
                return;
            }
            s_Instance = new HotFixEntry();
            
            await s_Instance.InitAsync();
            
            GameComponent.GameDriver.SetUpdateAction(s_Instance.Update);
            GameComponent.GameDriver.SetLateUpdateAction(s_Instance.LateUpdate);
            GameComponent.GameDriver.SetFixedUpdateAction(s_Instance.FixedUpdate);
            GameComponent.GameDriver.SetShutDownAction(s_Instance.ShutDown);
        }

        public static T GetDriver<T>() where T : GameDriverBase
        {
            var type = typeof(T);
            if (s_Instance.m_GameDriverDic.TryGetValue(type,out var driver))
            {
                return driver as T;
            }
            return null;
        }

        private async UniTask InitAsync()
        {
            foreach (var gameDriver in m_GameDrivers)
            {
                await gameDriver.InitAsync();
            }
            
            foreach (var gameDriver in m_GameDrivers)
            {
                m_GameDriverDic.Add(gameDriver.GetType(), gameDriver);
            }
            
            // StartGame
            var asset = await GameComponent.Asset.LoadAssetAsync<GameObject>("Assets/Bundles/UI/Canvas");
            Object.Instantiate(asset);
        }

        
        private void Update()
        {
            Log.Info("Update");
            TimeUtility.DeltaTime = Time.deltaTime;
            foreach (var gameDriver in m_GameDrivers)
            {
                gameDriver.Update();
            }
        }

        private void LateUpdate()
        {
            foreach (var gameDriver in m_GameDrivers)
            {
                gameDriver.LateUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (var gameDriver in m_GameDrivers)
            {
                gameDriver.FixedUpdate();
            }
        }

        private void ShutDown()
        {
            foreach (var gameDriver in m_GameDrivers)
            {
                gameDriver.ShutDown();
            }
            s_Instance = null;
        }
    }
}