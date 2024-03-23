using Cysharp.Threading.Tasks;
using Game.Module;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class GameEntry
    {
        public static async UniTaskVoid Start()
        {
            GameComponent.GameDriver.GetModule<ConfigDriver>();
            // GameComponent.GameDriver.GetModule<LocalizationDriver>();
            // GameComponent.GameDriver.GetModule<InputDriver>();
            // GameComponent.GameDriver.GetModule<EntityComponentDriver>();
            await GameComponent.GameDriver.InitAsync();

            var asset = await GameComponent.Asset.LoadAssetAsync<GameObject>("Assets/Bundles/UI/Canvas");
            var go = Object.Instantiate(asset);
            var text = go.GetComponentsInChildren<Text>();
            text[0].text = GameModule.Config.AllTable.LocalizationTable.Get("key1").Cn;
        }
    }
}