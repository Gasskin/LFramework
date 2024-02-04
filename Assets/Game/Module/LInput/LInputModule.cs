using Cysharp.Threading.Tasks;
using Rewired;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace LFramework.LInput
{
    public class LInputModule : LFrameworkModule
    {
        public override int Priority => (int)EModulePriority.None;

        private const string RewiredPath = "Assets/AssetsPackage/Core/Rewired.prefab";
        private InputManager m_InputManager;
        private Player m_Player;
        private Player m_System;

        public override async UniTask InitAsync()
        {
            var comp = GameEntry.GetComponent<ResourceComponent>();
            var asset = await comp.LoadAssetAsync<GameObject>(RewiredPath);
            var inst = Object.Instantiate(asset);
            Object.DontDestroyOnLoad(inst);
            
            await UniTask.Yield();
            
            m_InputManager = inst.GetComponent<InputManager>();
            m_System = ReInput.players.GetPlayer(RewiredConsts.Player.UI);
            
            comp.UnloadAsset(asset);
        }

        private void OnInputSourceUpdateEvent()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            Object.Destroy(m_InputManager.gameObject);
        }
    }
}