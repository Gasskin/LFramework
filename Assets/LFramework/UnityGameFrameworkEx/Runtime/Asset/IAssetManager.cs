using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace GameFramework.Asset
{
    public interface IAssetManager
    {
        /// <summary>
        /// 获取或设置资源包名称。
        /// </summary>
        string PackageName { get; set; }
        
        /// <summary>
        /// 本地资源包的版本
        /// </summary>
        string LocalPackageVersion { get; set; }

        /// <summary>
        /// 远端资源包的版本
        /// </summary>
        string RemotePackageVersion { get; set; }
        
        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        string ReadOnlyPath { get; set; }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        string ReadWritePath { get; set; }

        /// <summary>
        /// 获取或设置运行模式。
        /// </summary>
        EPlayMode PlayMode { get; set; }

        /// <summary>
        /// 初始化接口。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 初始化操作。
        /// </summary>
        /// <returns></returns>
        InitializationOperation InitPackage();

        /// <summary>
        /// 异步加载资源并获取句柄。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>同步加载资源句柄。</returns>
        AssetHandle LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object;

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="path"></param>
        void UnLoadAsset(string path);

        void UnLoadAllUnusedAsset();

        void ForceUnLoadAllAsset();

        UniTask<UpdatePackageVersionOperation> UpdatePackageVersion();
        UniTask<UpdatePackageManifestOperation> UpdatePackageManifest();
        ResourceDownloaderOperation CreateResourceDownloader(int downloadingMaxNumber, int failedTryAgain);
        UniTask<ClearUnusedCacheFilesOperation> ClearUnusedCacheFilesAsync();
    }
}