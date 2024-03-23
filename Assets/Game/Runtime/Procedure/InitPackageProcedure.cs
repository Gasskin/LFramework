using Cysharp.Threading.Tasks;
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
            // base.OnEnter(procedureOwner);
            // var operation = GameComponent.Asset.InitPackage();
            // await operation.ToUniTask();
            // if (operation.Status == EOperationStatus.Succeed)
            // {
            //     // 编辑器模式。
            //     if (GameComponent.Asset.m_PlayMode == EPlayMode.EditorSimulateMode)
            //     {
            //         Log.Info("Editor resource mode detected.");
            //         // ChangeState<ProcedurePreload>(procedureOwner);
            //     }
            //     // 单机模式。
            //     else if (GameComponent.Asset.m_PlayMode == EPlayMode.OfflinePlayMode)
            //     {
            //         Log.Info("Package resource mode detected.");
            //         // ChangeState<ProcedureInitResources>(procedureOwner);
            //     }
            //     // 可更新模式。
            //     else if (GameComponent.Asset.m_PlayMode == EPlayMode.HostPlayMode)
            //     {
            //         // 打开启动UI。
            //
            //         Log.Info("Updatable resource mode detected.");
            //         // ChangeState<ProcedureUpdateVersion>(procedureOwner);
            //     }
            //     else
            //     {
            //         Log.Error("UnKnow resource mode detected Please check???");
            //     }
            // }
        }
    }
}