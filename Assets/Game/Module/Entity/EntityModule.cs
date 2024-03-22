using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;

namespace Game.Module
{
    public class EntityModule: GameModuleBase
    {
    #region Override
        public override int Priority => (int)EModulePriority.None;
        public override async UniTask InitAsync()
        {
            MasterNodeEntity.Instance.Create();
            await UniTask.CompletedTask;
        }

        public override void Update(float delta)
        {
            MasterNodeEntity.Instance.Update();
        }

        public override void LateUpdate()
        {
            MasterNodeEntity.Instance.LateUpdate();
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            MasterNodeEntity.Instance.Destroy();
        }
#endregion
    }
}