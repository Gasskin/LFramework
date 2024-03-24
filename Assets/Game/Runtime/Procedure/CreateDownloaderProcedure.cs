using System;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game.Runtime
{
    public class CreateDownloaderProcedure: ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            var assetComponent = GameEntry.GetComponent<AssetComponent>();
            
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = assetComponent.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

            if (downloader.TotalDownloadCount == 0)
            {
                Log.Info("没有需要更新文件");
                await assetComponent.ClearUnusedCacheFilesAsync();
                ChangeState<LoadAssemblyProcedure>(procedureOwner);
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                var count = downloader.TotalDownloadCount;
                var sizeByte = downloader.TotalDownloadBytes;

                var sizeMb = sizeByte / 1048576f;
                sizeMb = UnityEngine.Mathf.Clamp(sizeMb, 0.1f, float.MaxValue);
                var totalSizeMb = sizeMb.ToString("f1");

                Log.Info("需要下载的文件数量：{0}，总计大小：{1}mb", count, totalSizeMb);

                var data = GameEntry.GetComponent<DataNodeComponent>();
                data.SetData("downloader", (VarAsyncOperationBase)downloader);
                ChangeState<DownloadFileProcedure>(procedureOwner);
            }
        }
    }
}