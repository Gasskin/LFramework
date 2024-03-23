using UnityGameFramework.Runtime;

namespace Game.Logic
{
    public static class GameComponent
    {
        private static SceneComponent s_Scene;
        public static SceneComponent Scene => s_Scene ??= GameEntry.GetComponent<SceneComponent>();

        private static ResourceComponent s_Resource;
        public static ResourceComponent Resource => s_Resource ??= GameEntry.GetComponent<ResourceComponent>();
        
        private static GameDriverComponent s_GameDriver;
        public static GameDriverComponent GameDriver => s_GameDriver ??= GameEntry.GetComponent<GameDriverComponent>();
        
        private static CoroutineLockComponent s_CoroutineLock;
        public static CoroutineLockComponent CoroutineLock => s_CoroutineLock ??= GameEntry.GetComponent<CoroutineLockComponent>();
        
        private static EventComponent s_Event;
        public static EventComponent Event => s_Event ??= GameEntry.GetComponent<EventComponent>();
        
        private static BaseComponent s_Base;
        public static BaseComponent Base => s_Base ??= GameEntry.GetComponent<BaseComponent>();
        
        private static SettingComponent s_Setting;
        public static SettingComponent Setting => s_Setting ??= GameEntry.GetComponent<SettingComponent>();
        
        private static SoundComponent s_Sound;
        public static SoundComponent Sound => s_Sound ??= GameEntry.GetComponent<SoundComponent>();
        
        private static EntityComponent s_Entity;
        public static EntityComponent Entity => s_Entity ??= GameEntry.GetComponent<EntityComponent>();
        
        
        private static UIComponent s_UI;
        public static UIComponent UI => s_UI ??= GameEntry.GetComponent<UIComponent>();
        
        private static ObjectPoolComponent s_ObjectPool;
        public static ObjectPoolComponent ObjectPool => s_ObjectPool ??= GameEntry.GetComponent<ObjectPoolComponent>();

        private static AssetComponent s_Asset;
        public static AssetComponent Asset => s_Asset ??= GameEntry.GetComponent<AssetComponent>();
    }
}
