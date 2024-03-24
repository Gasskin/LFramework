using System;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game.Runtime
{
    public class UpdateManifestProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            var assetComponent = GameEntry.GetComponent<AssetComponent>();
            var operation = await assetComponent.UpdatePackageManifest();
            if (operation.Status == EOperationStatus.Succeed)
            {
                ChangeState<CreateDownloaderProcedure>(procedureOwner);
            }
            else
            {
                Log.Error("更新资源清单异常：{0}", operation.Error);
            }
        }
    }
}