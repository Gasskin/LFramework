using Game.Module;
using Game.Module;
using Game.Module;

namespace Game.Logic
{
    public static class GameModule
    {
        private static ConfigDriver s_Config;
        public static ConfigDriver Config => s_Config ??= GameComponent.GameDriver.GetModule<ConfigDriver>();

        private static InputDriver s_Input;
        public static InputDriver Input => s_Input ??= GameComponent.GameDriver.GetModule<InputDriver>();
        
        private static LocalizationDriver s_Localization;
        public static LocalizationDriver Localization => s_Localization ??= GameComponent.GameDriver.GetModule<LocalizationDriver>();
        
        private static EntityComponentDriver s_EntityComponent;
        public static EntityComponentDriver EntityComponent => s_EntityComponent ??= GameComponent.GameDriver.GetModule<EntityComponentDriver>(); 
    }
}
