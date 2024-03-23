using Cysharp.Threading.Tasks;
using Game.Module;

namespace Game.Logic
{
    public class GameEntry
    {
        public static async UniTaskVoid Start()
        {
            GameComponent.GameDriver.GetModule<ConfigDriver>();
            GameComponent.GameDriver.GetModule<LocalizationDriver>();
            GameComponent.GameDriver.GetModule<InputDriver>();
            GameComponent.GameDriver.GetModule<EntityComponentDriver>();

            await GameComponent.GameDriver.InitAsync();
        }
    }
}