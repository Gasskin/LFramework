using Cysharp.Threading.Tasks;
using Game.Logic;
using Game.Logic.Utility;
using Game.Module.Entity;
using GameFramework.GameUpdater;
using Rewired;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Game.Module.Input
{
    public partial class InputModule : GameModuleBase
    {
    #region Override
        public override int Priority => (int)EModulePriority.None;
        
        public override async UniTask InitAsync()
        {
            var asset = await GameComponent.Resource.LoadAssetAsync<GameObject>(ResourcesPathConfig.Core.Rewired);
            var inst = Object.Instantiate(asset);

            await UniTask.Yield();
            GameComponent.Resource.UnloadAsset(asset);

            m_InputManager = inst.GetComponent<InputManager>();
            m_Player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);

            ReInput.InputSourceUpdateEvent += OnInputSourceUpdateEvent;

            await InitMapEnabler();
        }

        private Entity.Entity player;
        private int test = 0;
        public override void Update(float delta)
        {
            var dir = GetMoveDir();
            Move(dir);

            if (UnityEngine.Input.GetMouseButtonDown(0)) 
            {
                player = Entity.Entity.Create<PlayerEntity>();
                player.AddComponent<UpdateComponent>();
                var comp = player.AddComponent<AttrComponent>();
                comp.AddAttrWatcher<MoveDirAttrWatcher>();
                player.AddChild<ModelEntity>(ResourcesPathConfig.ModelPrefabs.RPG_Character);
            }

            if (UnityEngine.Input.GetMouseButtonDown(1)) 
            {
                var comp = player.GetComponent<AttrComponent>();
                comp.SetAttr((uint)EAttrType.MoveDir, test++);
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
    #endregion

        public EMoveDir MoveDir { get; private set; }
        
        private InputManager m_InputManager;
        private Player m_Player;
        
        private void OnInputSourceUpdateEvent()
        {
        }

        
        private EMoveDir GetMoveDir()
        {
            EMoveDir dir = EMoveDir.None;
            var left = m_Player.GetAxis(RewiredConsts.Action.Move_Left);
            var right = m_Player.GetAxis(RewiredConsts.Action.Move_Right);
            // 同时按键时保持之前的输入
            if (left > 0 && right > 0)
            {
                dir = MoveDir;
            }
            else if (left > 0)
            {
                dir = EMoveDir.Left;
            }
            else if (right > 0)
            {
                dir = EMoveDir.Right;
            }
            return dir;
        }
    }
}