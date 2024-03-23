﻿using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;

namespace Game.Module
{
    public class EntityComponentDriver: GameDriverBase
    {
    #region Override
        public override int Priority => (int)EModulePriority.None;
        public override async UniTask InitAsync()
        {
            MasterECEntity.Instance.Create();
            await UniTask.CompletedTask;
        }

        public override void Update(float delta)
        {
            MasterECEntity.Instance.Update();
        }

        public override void LateUpdate()
        {
            MasterECEntity.Instance.LateUpdate();
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            MasterECEntity.Instance.Destroy();
        }
#endregion
    }
}