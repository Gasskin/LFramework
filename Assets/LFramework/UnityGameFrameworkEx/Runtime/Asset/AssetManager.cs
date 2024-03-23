using YooAsset;

namespace GameFramework.Asset
{
    public class AssetManager : GameFrameworkModule, IAssetManager
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
            string packageName = PackageName;
            var defaultPackage = YooAssets.TryGetPackage(packageName);
            if (defaultPackage == null)
            {
                defaultPackage = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(defaultPackage);
            }
        }

        public InitializationOperation InitPackage()
        {
            throw new System.NotImplementedException();
        }
    }
}