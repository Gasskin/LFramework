using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LFramework
{
    public class LuBanComponent : GameFrameworkComponent
    {
        public Tables AllTable => m_AllTable;
        private Tables m_AllTable;
        private bool m_IsInit;
        private void Start()
        {
            // var comp = GameEntry.GetComponent<ResourceComponent>();
            // var call = new LoadAssetCallbacks(((assetName, asset, duration, data) =>
            // {
            //     var v = asset as TextAsset;
            //     Log.Error(v.bytes);
            // }),((assetName, status, message, data) =>
            // {
            //     Log.Error(message);
            // }));
            // comp.LoadAsset($"Assets/AssetsPackage/LuBan/aitable.bytes", call);
            // InitAsync();
        }

        public async UniTask InitAsync()
        {
            if (m_IsInit)
                return;
            // 必须延一帧
            m_AllTable = new Tables();
            await m_AllTable.LoadAsync(AsyncLoader);
            m_IsInit = true;
            Log.Info("====== LuBan Loaded! ======");
        }

        public GlobalConfig GetGlobalConfig(string key)
        {
            if (m_AllTable.GlobalTable.DataMap.TryGetValue(key, out var config))
            {
                return config;
            }
            Log.Error("GlobalTable no exit key: {0}", key);
            return null;
        }

        private async Task<ByteBuf> AsyncLoader(string s)
        {
            var comp = GameEntry.GetComponent<ResourceComponent>();
            var asset = await comp.LoadAssetAsync<TextAsset>($"Assets/AssetsPackage/LuBan/{s}.bytes");
            return new ByteBuf(asset.bytes);
        }
    }
}
