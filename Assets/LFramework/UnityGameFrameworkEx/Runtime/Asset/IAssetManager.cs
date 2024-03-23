using UnityEngine;
using YooAsset;

namespace GameFramework.Asset
{
    public interface IAssetManager
    {
        /// <summary>
        /// 获取或设置资源包名称。
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        public string ReadOnlyPath { get; set; }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        public string ReadWritePath { get; set; }

        /// <summary>
        /// 获取或设置运行模式。
        /// </summary>
        public EPlayMode PlayMode { get; set; }

        /// <summary>
        /// 热更链接URL。
        /// </summary>
        public string HostServerURL { get; set; }

        /// <summary>
        /// 初始化接口。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 初始化操作。
        /// </summary>
        /// <returns></returns>
        InitializationOperation InitPackage();
    }
}