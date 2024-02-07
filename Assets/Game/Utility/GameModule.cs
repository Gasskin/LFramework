
namespace Game.Utility
{
    public static class GameModule
    {
        private static ConfigModule.ConfigModule s_Config;
        public static ConfigModule.ConfigModule Config => s_Config ??= GameComponent.GameUpdater.GetModule<ConfigModule.ConfigModule>();

        private static InputModule.InputModule s_Input;
        public static InputModule.InputModule Input => s_Input ??= GameComponent.GameUpdater.GetModule<InputModule.InputModule>();
        
        private static ConfigModule.LocalizationModule s_Localization;
        public static ConfigModule.LocalizationModule Localization => s_Localization ??= GameComponent.GameUpdater.GetModule<ConfigModule.LocalizationModule>();
    }
}
