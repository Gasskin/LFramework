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
            m_AssetManager.HostServerURL = GetHostServerURL();
            m_AssetManager.Initialize();
            Log.Info($"AssetsComponent Run Mode：{m_PlayMode}");
        }

        private string GetHostServerURL()
        {
            return "";
        }

        public InitializationOperation InitPackage()
        {
            return m_AssetManager.InitPackage();
        }

        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR || DEBUG
            var watcher = ReferencePool.Acquire<LoadWatcher>();
            watcher.Start();
#endif
            var handle = m_AssetManager.LoadAssetAsync<T>(path);
            await handle.ToUniTask(this);
#if UNITY_EDITOR || DEBUG
            watcher.Stop();
            Log.Info("<color=#00FF35>加载资源耗时：</color>{0}ms  {1}", watcher.ElapsedMilliseconds , path);
            ReferencePool.Release(watcher);
#endif
            if (handle.Status == EOperationStatus.Succeed)
            {
                return handle.AssetObject as T;
            }
            return null;
        }

    #region Ondin
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