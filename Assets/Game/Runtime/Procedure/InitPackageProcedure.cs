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
                    // 打开启动UI。
            
                    Log.Info("Updatable resource mode detected.");
                    // ChangeState<ProcedureUpdateVersion>(procedureOwner);
                }
                else
                {
                    Log.Error("UnKnow resource mode detected Please check???");
                }
            }
        }
    }
}