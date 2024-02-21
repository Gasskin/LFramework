using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;

namespace Game.Module.Entity
{
    public class EntityModule: GameModuleBase
    {
    #region Override
        public override int Priority => (int)EModulePriority.None;
        public override async UniTask InitAsync()
        {
            MasterEntity.Instance.Create();
            await UniTask.CompletedTask;
        }

        public override void Update(float delta)
        {
            MasterEntity.Instance.Update();
        }

        public override void LateUpdate()
        {
            MasterEntity.Instance.LateUpdate();
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            MasterEntity.Instance.Destroy();
        }
#endregion
    }
}