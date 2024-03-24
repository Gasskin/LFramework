using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

namespace GameFramework.Asset
{
    public partial class AssetManager
    {
        private class QueryServices : IBuildinQueryServices
        {
            private Dictionary<string, bool> m_Cache = new();

            public bool Query(string packageName, string fileName, string fileCRC)
            {
                var filePath = Path.Combine(Application.streamingAssetsPath, "Bundles", packageName, fileName);
                if (!m_Cache.TryGetValue(filePath, out var value))
                {
                    value = File.Exists(filePath);
                    m_Cache.Add(filePath, value);
                }
                return value;
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
            string hostServerIP = "http://127.0.0.1:1080";
            string appVersion = "v1.0";

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }
    }
}