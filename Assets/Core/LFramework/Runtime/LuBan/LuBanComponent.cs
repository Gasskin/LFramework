using System;
using System.Threading.Tasks;
using cfg;
using Luban;
using SimpleJSON;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LFramework
{
    public class LuBanComponent : GameFrameworkComponent
    {
        public Tables AllTable => m_AllTable;
        private Tables m_AllTable;
        private bool m_IsInit;
        private ELuBanLoadType m_ELuBanLoadType = ELuBanLoadType.Json;

        public async Task InitAsync()
        {
            if (m_IsInit)
            {
                Log.Warning("luban is already initialized");
                return;
            }
            m_AllTable = new Tables();

            var tableType = m_AllTable.GetType();
            var loadMethodInfo = tableType.GetMethod("LoadAsync");
            if (loadMethodInfo == null)
            {
                Log.Error("tables has no method LoadAsync,may generated error");
                return;
            }
            
            var loaderReturnType = loadMethodInfo.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            m_ELuBanLoadType = loaderReturnType == typeof(Task<ByteBuf>) ? ELuBanLoadType.Byte : ELuBanLoadType.Json;
            
            var comp = GameEntry.GetComponent<ResourceComponent>();
            switch (m_ELuBanLoadType)
            {
                case ELuBanLoadType.Byte:

                    async Task<ByteBuf> ByteLoader(string s)
                    {
                        TextAsset textAsset = await comp.LoadAssetAsync<TextAsset>($"Assets/AssetsPackage/LuBan/{s}.bytes");
                        return new ByteBuf(textAsset.bytes);
                    }

                    Func<string, Task<ByteBuf>> func1 = ByteLoader;
                    await (Task)loadMethodInfo.Invoke(m_AllTable, new object[] { func1 });
                    break;
                case ELuBanLoadType.Json:

                    async Task<JSONNode> JsonLoader(string s)
                    {
                        TextAsset textAsset = await comp.LoadAssetAsync<TextAsset>($"Assets/AssetsPackage/LuBan/{s}.json");
                        return JSON.Parse(textAsset.text);
                    }

                    Func<string, Task<JSONNode>> func2 = JsonLoader;
                    await (Task)loadMethodInfo.Invoke(m_AllTable, new object[] { func2 });
                    break;
            }
            m_IsInit = true;
            Log.Info("====== luban initialize success ======");
        }

        public GlobalConfig GetGlobalConfig(string key)
        {
            if (!m_IsInit)
            {
                return null;
            }
            if (m_AllTable.GlobalTable.DataMap.TryGetValue(key, out var config))
            {
                return config;
            }
            Log.Error("GlobalTable no exit key: {0}", key);
            return null;
        }
    }
}
