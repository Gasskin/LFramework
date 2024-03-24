using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Asset;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game.Runtime
{
    public class InitPackageProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var assetComp = GameEntry.GetComponent<AssetComponent>();
            var operation = assetComp.InitPackage();
            await operation.ToUniTask();
            if (operation.Status == EOperationStatus.Succeed)
            {
                // 设置本地资源版本
                assetComp.SetPackageVersion(operation.PackageVersion);

                // 编辑器模式。
                if (assetComp.m_PlayMode == EPlayMode.EditorSimulateMode)
                {
                    ChangeState<LoadAssemblyProcedure>(procedureOwner);
                }
                // 单机模式。
                else if (assetComp.m_PlayMode == EPlayMode.OfflinePlayMode)
                {
                    ChangeState<LoadAssemblyProcedure>(procedureOwner);
                }
                // 可更新模式。
                else if (assetComp.m_PlayMode == EPlayMode.HostPlayMode)
                {
                    ChangeState<UpdateVersionProcedure>(procedureOwner);
                }
                else
                {
                    Log.Error("资源模式异常");
                }
            }
            else
            {
                Log.Fatal(operation.Error);
            }
        }
    }
}