﻿using System.IO;
using UnityEngine;
using YooAsset;

namespace GameFramework.Asset
{
    public partial class AssetManager
    {
        private class QueryServices: IBuildinQueryServices
        {
            private readonly bool m_CompareFileCRC = false;
            
            public bool Query(string packageName, string fileName, string fileCRC)
            {
                var filePath = Path.Combine(Application.streamingAssetsPath, "bundles", packageName, fileName);
                if (File.Exists(filePath))
                {
                    if (m_CompareFileCRC)
                    {
                        string crc32 = YooAsset.Editor.EditorTools.GetFileCRC32(filePath);
                        return crc32 == fileCRC;
                    }
                    return true;
                }
                return false;
            }
        }
        
        private class RemoteServices : IRemoteServices
        {
            private readonly string m_DefaultHostServer;
            private readonly string m_FallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                m_DefaultHostServer = defaultHostServer;
                m_FallbackHostServer = fallbackHostServer;
            }
            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{m_DefaultHostServer}/{fileName}";
            }
            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{m_FallbackHostServer}/{fileName}";
            }
        }
        
        private string GetHostServerURL()
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            string hostServerIP = "http://127.0.0.1";
            string appVersion = "v1.0";

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            else
                return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }
    }
}