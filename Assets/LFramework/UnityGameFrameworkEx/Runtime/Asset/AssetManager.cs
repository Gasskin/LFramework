using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameFramework.Asset
{
    public partial class AssetManager : GameFrameworkModule, IAssetManager
    {
        public string PackageName { get; set; } = "DefaultPackage";
        public string LocalPackageVersion { get; set; }
        public string RemotePackageVersion { get; set; }
        public string ReadOnlyPath { get; set; }

        public string ReadWritePath { get; set; }
        public EPlayMode PlayMode { get; set; }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        internal override void Shutdown()
        {
        }

    #region 初始化
        public void Initialize()
        {
            // 初始化资源系统
            YooAssets.Initialize(new YooAssetsLogger());

            // 创建默认的资源包
            var defaultPackage = YooAssets.TryGetPackage(PackageName);
            if (defaultPackage == null)
            {
                defaultPackage = YooAssets.CreatePackage(PackageName);
                YooAssets.SetDefaultPackage(defaultPackage);
            }
        }

        public InitializationOperation InitPackage()
        {
            var package = YooAssets.TryGetPackage(PackageName);

            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (PlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, PackageName);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (PlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (PlayMode == EPlayMode.HostPlayMode)
            {
                var createParameters = new HostPlayModeParameters();
                createParameters.BuildinQueryServices = new QueryServices();
                var defaultHostServer = GetHostServerURL();
                var fallbackHostServer = GetHostServerURL();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            return initializationOperation;
        }
    #endregion

    #region 资源加载/卸载
        public AssetHandle LoadAssetAsync<T>(string path) where T : Object
        {
            return YooAssets.LoadAssetAsync<T>(path);
        }

        public void UnLoadAsset(string path)
        {
            var package = YooAssets.TryGetPackage(PackageName);
            package.TryUnloadUnusedAsset(path);
        }

        public void UnLoadAllUnusedAsset()
        {
            var package = YooAssets.TryGetPackage(PackageName);
            package.UnloadUnusedAssets();
        }

        public void ForceUnLoadAllAsset()
        {
            var package = YooAssets.TryGetPackage(PackageName);
            package.ForceUnloadAllAssets();
        }
    #endregion

    #region 资源更新
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        public async UniTask<UpdatePackageVersionOperation> UpdatePackageVersion()
        {
            var package = YooAssets.TryGetPackage(PackageName);
            var operate = package.UpdatePackageVersionAsync();
            await operate.ToUniTask();
            return operate;
        }

        public async UniTask<UpdatePackageManifestOperation> UpdatePackageManifest()
        {
            var package = YooAssets.TryGetPackage(PackageName);
            var operate = package.UpdatePackageManifestAsync(RemotePackageVersion);
            await operate.ToUniTask();
            return operate;
        }
        
        public ResourceDownloaderOperation CreateResourceDownloader(int downloadingMaxNumber, int failedTryAgain)
        {
            var package = YooAssets.GetPackage(PackageName);
            return package.CreateResourceDownloader(downloadingMaxNumber,failedTryAgain);
        }

        public async UniTask<ClearUnusedCacheFilesOperation> ClearUnusedCacheFilesAsync()
        {
            var package = YooAssets.TryGetPackage(PackageName);
            var operate = package.ClearUnusedCacheFilesAsync();
            await operate.ToUniTask();
            return operate;
        }
    #endregion
    }
}