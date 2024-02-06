using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;
using LFramework;
using Rewired;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Game.InputModule
{
    public partial class InputModule : GameModuleBase
    {
        private const string DefaultRewiredPath = "Assets/AssetsPackage/Core/Rewired.prefab";
        public override int Priority => (int)EModulePriority.None;
        private InputManager m_InputManager;
        private Player m_Player;

        private ControllerMapEnabler.RuleSet rule1;
        
        public override async UniTask InitAsync()
        {
            var comp = GameEntry.GetComponent<ResourceComponent>();
            var asset = await comp.LoadAssetAsync<GameObject>(DefaultRewiredPath);
            var inst = Object.Instantiate(asset);

            await UniTask.Yield();
            comp.UnloadAsset(asset);

            m_InputManager = inst.GetComponent<InputManager>();
            m_Player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);

            ReInput.InputSourceUpdateEvent += OnInputSourceUpdateEvent;


            await InitMapEnabler();
        }

        private void OnInputSourceUpdateEvent()
        {
        }

        public override void Update()
        {
            if (m_Player.GetButton("Jump"))
            {
                Log.Error("Jump");
            }

            if (m_Player.GetButton("Down"))
            {
                Log.Error("Down");
            }

            if (m_Player.GetButton("Map"))
            {
                Log.Error("Map");
            }

            if (Input.GetMouseButtonDown(0))
            {
                ChangeInputMap(7);
            }
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            ReInput.InputSourceUpdateEvent -= OnInputSourceUpdateEvent;
            m_InputManager = null;
            m_Player = null;
        }
    }
}