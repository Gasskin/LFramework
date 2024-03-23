using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;

namespace Game.Module
{
    public class VirtualGameObjectDriver: GameDriverBase
    {
    #region Override
        public override int Priority => (int)EModulePriority.None;
        public override async UniTask InitAsync()
        {
            MasterVGameObject.Instance.Create();
            await UniTask.CompletedTask;
        }

        public override void Update(float delta)
        {
            MasterVGameObject.Instance.Update();
        }

        public override void LateUpdate()
        {
            MasterVGameObject.Instance.LateUpdate();
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            MasterVGameObject.Instance.Destroy();
        }
#endregion
    }
}