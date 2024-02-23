using Game.Module;
using Game.Module;
using Game.Module;

namespace Game.Logic
{
    public static class GameModule
    {
        private static ConfigModule s_Config;
        public static ConfigModule Config => s_Config ??= GameComponent.GameUpdater.GetModule<ConfigModule>();

        private static InputModule s_Input;
        public static InputModule Input => s_Input ??= GameComponent.GameUpdater.GetModule<InputModule>();
        
        private static LocalizationModule s_Localization;
        public static LocalizationModule Localization => s_Localization ??= GameComponent.GameUpdater.GetModule<LocalizationModule>();
        
        private static EntityModule s_Entity;
        public static EntityModule Entity => s_Entity ??= GameComponent.GameUpdater.GetModule<EntityModule>(); 
    }
}
