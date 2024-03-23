﻿using System.Collections.Generic;
using System.Linq;
using GameFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityGameFramework.Asset;
using YooAsset;
using YooAsset.Editor;

namespace UnityGameFramework.Runtime
{
    public class AssetComponent: GameFrameworkComponent
    {
        [LabelText("资源模式")]
        public EPlayMode m_PlayMode;


        [ValueDropdown("GetPackages")]
        [LabelText("默认包")]
        public string m_DefaultPackageName;
        private IAssetManager m_AssetManager;
        
        protected override void Awake()
        {
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