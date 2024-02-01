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

        public Tables AllTable { get; private set; }


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
            // 必须延一帧
            AllTable = new Tables();
            await AllTable.LoadAsync(AsyncLoader);
            Log.Info("====== LuBan Loaded! ======");
        }

        private async Task<ByteBuf> AsyncLoader(string s)
        {
            var comp = GameEntry.GetComponent<ResourceComponent>();
            var asset = await comp.LoadAssetAsync<TextAsset>($"Assets/AssetsPackage/LuBan/{s}.bytes");
            return new ByteBuf(asset.bytes);
        }
    }
}
