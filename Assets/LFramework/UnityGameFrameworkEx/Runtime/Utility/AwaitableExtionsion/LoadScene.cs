using Cysharp.Threading.Tasks;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    public static partial class AwaitableExtension
    {
        private static void RegisterSceneEvent()
        {
            var comp = GameEntry.GetComponent<EventComponent>();
            comp.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            comp.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailed);
        }
        
        public static async UniTask<bool> LoadSceneAsync(this SceneComponent sceneComponent, string sceneAssetName)
        {
            var comp = GameEntry.GetComponent<CoroutineLockComponent>();
            var tcs = AutoResetUniTaskCompletionSource<bool>.Create();
            
            using var coroutineLock = await comp.Wait(ECoroutineLockType.LoadScene, sceneAssetName.GetHashCode());
            sceneComponent.LoadScene(sceneAssetName, tcs);
            return await tcs.Task;
        }
        
        private static void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            var param = (LoadSceneSuccessEventArgs)e;
            var tcs = param.UserData as AutoResetUniTaskCompletionSource<bool>;
            tcs?.TrySetResult(true);
        }
        
        private static void OnLoadSceneFailed(object sender, GameEventArgs e)
        {
            var param = (LoadSceneFailureEventArgs)e;
            Log.Error(param.ErrorMessage);
            var tcs = param.UserData as AutoResetUniTaskCompletionSource<bool>;
            tcs?.TrySetResult(false);
        }
    }
}