using YooAsset;

namespace GameFramework.Asset
{
    public partial class AssetManager : GameFrameworkModule, IAssetManager
    {
        public string PackageName { get; set; } = "DefaultPackage";

        public string ReadOnlyPath { get; set; }

        public string ReadWritePath { get; set; }
        public EPlayMode PlayMode { get; set; }
        public string HostServerURL { get; set; }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        internal override void Shutdown()
        {
        }

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
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline,PackageName);
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
    }
}