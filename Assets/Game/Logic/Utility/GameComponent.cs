using UnityGameFramework.Runtime;

namespace Game.Logic
{
    public static class GameComponent
    {
        private static SceneComponent s_Scene;
        public static SceneComponent Scene => s_Scene ??= UnityGameFramework.Runtime.GameEntry.GetComponent<SceneComponent>();

        private static ResourceComponent s_Resource;
        public static ResourceComponent Resource => s_Resource ??= UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
        
        private static GameDriverComponent s_GameDriver;
        public static GameDriverComponent GameDriver => s_GameDriver ??= UnityGameFramework.Runtime.GameEntry.GetComponent<GameDriverComponent>();
        
        private static CoroutineLockComponent s_CoroutineLock;
        public static CoroutineLockComponent CoroutineLock => s_CoroutineLock ??= UnityGameFramework.Runtime.GameEntry.GetComponent<CoroutineLockComponent>();
        
        private static EventComponent s_Event;
        public static EventComponent Event => s_Event ??= UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>();
        
        private static BaseComponent s_Base;
        public static BaseComponent Base => s_Base ??= UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
        
        private static SettingComponent s_Setting;
        public static SettingComponent Setting => s_Setting ??= UnityGameFramework.Runtime.GameEntry.GetComponent<SettingComponent>();
        
        private static SoundComponent s_Sound;
        public static SoundComponent Sound => s_Sound ??= UnityGameFramework.Runtime.GameEntry.GetComponent<SoundComponent>();
        
        private static EntityComponent s_Entity;
        public static EntityComponent Entity => s_Entity ??= UnityGameFramework.Runtime.GameEntry.GetComponent<EntityComponent>();
        
        
        private static UIComponent s_UI;
        public static UIComponent UI => s_UI ??= UnityGameFramework.Runtime.GameEntry.GetComponent<UIComponent>();
        
        private static ObjectPoolComponent s_ObjectPool;
        public static ObjectPoolComponent ObjectPool => s_ObjectPool ??= UnityGameFramework.Runtime.GameEntry.GetComponent<ObjectPoolComponent>();

        private static AssetComponent s_Asset;
        public static AssetComponent Asset => s_Asset ??= UnityGameFramework.Runtime.GameEntry.GetComponent<AssetComponent>();
    }
}
