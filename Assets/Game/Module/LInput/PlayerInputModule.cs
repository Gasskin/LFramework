using Cysharp.Threading.Tasks;
using Rewired;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LFramework.LInput
{
    public class PlayerInputModule : LFrameworkModule
    {
        internal override int Priority => (int)EModulePriority.None;

        private const string RewiredPath = "Assets/AssetsPackage/Core/Rewired.prefab";
        private InputManager m_InputManager;
        private Player m_Player;
        private Player m_System;
        
        internal override async UniTask InitAsync()
        {
            var comp = GameEntry.GetComponent<ResourceComponent>();
            var asset = await comp.LoadAssetAsync<GameObject>(RewiredPath);
            var inst = Object.Instantiate(asset);
            Object.DontDestroyOnLoad(inst);

            await UniTask.Yield();

            m_InputManager = inst.GetComponent<InputManager>();
            m_Player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);
            m_System = ReInput.players.GetPlayer(RewiredConsts.Player.System);

            ReInput.InputSourceUpdateEvent += OnInputSourceUpdateEvent;
            
            comp.UnloadAsset(asset);
        }

        private void OnInputSourceUpdateEvent()
        {
            
        }

        internal override void Update()
        {
            
        }

        internal override void LateUpdate()
        {
        }

        internal override void FixedUpdate()
        {
        }

        internal override void ShutDown()
        {
            ReInput.InputSourceUpdateEvent -= OnInputSourceUpdateEvent;
            Object.Destroy(m_InputManager.gameObject);
        }
    }
}