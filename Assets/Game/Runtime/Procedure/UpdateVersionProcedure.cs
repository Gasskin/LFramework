using System;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game.Runtime
{
    public class UpdateVersionProcedure: ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Log.Error("设备无法访问网络");
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            var assetComponent = GameEntry.GetComponent<AssetComponent>();
            var operation = await assetComponent.UpdatePackageVersion();

            if (operation.Status == EOperationStatus.Succeed)
            {
                var assetComp = GameEntry.GetComponent<AssetComponent>();
                assetComp.SetPackageVersion("", operation.PackageVersion);         
                ChangeState<UpdateManifestProcedure>(procedureOwner);
            }
            else
            {
                Log.Fatal("更新资源版本异常：{0}",operation.Error);
            }
        }
    }
}