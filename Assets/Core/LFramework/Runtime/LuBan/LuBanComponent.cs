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

        public async UniTask InitAsync()
        {
            if (m_IsInit)
            {
                Log.Warning("luban is already initialized");
                return;
            }
            m_AllTable = new Tables();
            await m_AllTable.LoadAsync(AsyncLoader);
            m_IsInit = true;
            Log.Info("====== luban initialize success ======");
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
