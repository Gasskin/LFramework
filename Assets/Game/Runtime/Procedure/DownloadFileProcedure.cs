using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game.Runtime
{
    public class DownloadFileProcedure : ProcedureBase
    {
        private ResourceDownloaderOperation m_Downloader;

        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var data = GameEntry.GetComponent<DataNodeComponent>();
            m_Downloader = (ResourceDownloaderOperation)data.GetData<VarAsyncOperationBase>("downloader");

            // 注册下载回调
            m_Downloader.OnDownloadErrorCallback = OnDownloadErrorCallback;
            m_Downloader.OnDownloadProgressCallback = OnDownloadProgressCallback;
            m_Downloader.BeginDownload();
            await m_Downloader.ToUniTask();

            // 检测下载结果
            if (m_Downloader.Status == EOperationStatus.Succeed)
            {
                // ChangeState<ProcedureDownloadOver>(_procedureOwner);
                var asset = GameEntry.GetComponent<AssetComponent>();
                var operation = await asset.ClearUnusedCacheFilesAsync();
                if (operation.Status != EOperationStatus.Succeed)
                {
                    Log.Warning("清除缓存失败，异常：{0}", operation.Error);
                }
                ChangeState<LoadAssemblyProcedure>(procedureOwner);
            }
            else
            {
                Log.Fatal("下载更新失败，异常：{0}", m_Downloader.Error);
            }
        }

        private void OnDownloadErrorCallback(string fileName, string error)
        {
            Log.Error("下载文件失败：{0}，异常：{1}", fileName, error);
            // 是否重新下载？
            // ChangeState<CreateDownloaderProcedure>(procedureOwner);
        }

        private void OnDownloadProgressCallback(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
        {
            var currentSizeMb = (currentDownloadBytes / 1048576f).ToString("f1");
            var totalSizeMb = (totalDownloadBytes / 1048576f).ToString("f1");
            var progress = m_Downloader.Progress.ToString("f1");
            Log.Info("更新中：{0}/{1}个文件，{2}/{3}mb，{4}%", currentDownloadCount,
                totalDownloadCount, currentSizeMb, totalSizeMb, progress);
        }
    }
}