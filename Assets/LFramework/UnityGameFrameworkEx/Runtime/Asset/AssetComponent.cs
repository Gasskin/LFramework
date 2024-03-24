using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Asset;
using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;

#if UNITY_EDITOR
using YooAsset.Editor;
#endif

namespace UnityGameFramework.Runtime
{
    public class AssetComponent : GameFrameworkComponent
    {
        [LabelText("资源模式")]
        public EPlayMode m_PlayMode;


        [ValueDropdown("GetPackages")]
        [LabelText("默认包")]
        public string m_DefaultPackageName;

        private IAssetManager m_AssetManager;

        protected override void Awake()
        {
            base.Awake();
            m_AssetManager = GameFrameworkEntry.GetModule<IAssetManager>();
#if !UNITY_EDITOR
            if (m_PlayMode == EPlayMode.EditorSimulateMode)
            {
                m_PlayMode = EPlayMode.HostPlayMode;
            }
#endif
            m_AssetManager.ReadOnlyPath = Application.streamingAssetsPath;
            m_AssetManager.ReadWritePath = Application.persistentDataPath;
            m_AssetManager.PackageName = m_DefaultPackageName;
            m_AssetManager.PlayMode = m_PlayMode;
            m_AssetManager.Initialize();
            Log.Info($"AssetsComponent Run Mode：{m_PlayMode}");
        }

        public InitializationOperation InitPackage()
        {
            return m_AssetManager.InitPackage();
        }

        public void SetPackageVersion(string local = "", string remote = "")
        {
            if (!string.IsNullOrEmpty(local))
            {
                m_AssetManager.LocalPackageVersion = local;
            }
            if (!string.IsNullOrEmpty(remote))
            {
                m_AssetManager.RemotePackageVersion = remote;
            }
        }

    #region 资源加载/卸载
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR || DEBUG
            var watcher = ReferencePool.Acquire<TimeWatcher>();
            watcher.Start();
#endif
            var handle = m_AssetManager.LoadAssetAsync<T>(path);
            await handle.ToUniTask(this);
#if UNITY_EDITOR || DEBUG
            watcher.Stop();
            Log.Info("<color=#00FF35>加载资源耗时：</color>{0}ms  {1}", watcher.ElapsedMilliseconds, path);
            ReferencePool.Release(watcher);
#endif
            if (handle.Status == EOperationStatus.Succeed)
            {
                return handle.AssetObject as T;
            }

            return null;
        }

        public void UnLoadAsset(string path)
        {
            m_AssetManager.UnLoadAsset(path);
        }

        public void UnLoadAllUnusedAsset()
        {
            m_AssetManager.UnLoadAllUnusedAsset();
        }

        public void ForceUnLoadAllAsset()
        {
            m_AssetManager.ForceUnLoadAllAsset();
        }
    #endregion

    #region 资源更新
        public async UniTask<UpdatePackageVersionOperation> UpdatePackageVersion()
        {
            return await m_AssetManager.UpdatePackageVersion();
        }

        public async UniTask<UpdatePackageManifestOperation> UpdatePackageManifest()
        {
            return await m_AssetManager.UpdatePackageManifest();
        }

        public ResourceDownloaderOperation CreateResourceDownloader(int max, int tryAgainCount)
        {
            return m_AssetManager.CreateResourceDownloader(max, tryAgainCount);
        }
    #endregion

    #region Odin
#if UNITY_EDITOR
        private static List<string> GetPackages()
        {
            List<string> result = new List<string>();
            result.Add("None");
            foreach (var package in AssetBundleCollectorSettingData.Setting.Packages)
            {
                result.Add(package.PackageName);
            }
            return result;
        }
#endif
    #endregion
    }
}