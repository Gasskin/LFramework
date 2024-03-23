using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.CoroutineLock;

namespace UnityGameFramework.Runtime
{
    public static partial class AwaitableExtension
    {
        private static readonly Dictionary<string, AutoResetUniTaskCompletionSource<bool>> LoadSceneTcs = new();

        private static void RegisterSceneEvent()
        {
            var comp = GameEntry.GetComponent<EventComponent>();
            comp.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            comp.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailed);
        }
        

        public static async UniTask<bool> LoadSceneAsync(this SceneComponent sceneComponent, string sceneAssetName, int priority)
        {
            var comp = GameEntry.GetComponent<CoroutineLockComponent>();
            var tcs = AutoResetUniTaskCompletionSource<bool>.Create();

            using var coroutineLock = await comp.Wait(ECoroutineLockType.LoadScene, sceneAssetName.GetHashCode());
            sceneComponent.LoadScene(sceneAssetName, priority);
            LoadSceneTcs.Add(sceneAssetName, tcs);
            return await tcs.Task;
        }

        private static void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneSuccessEventArgs)e;
            if (LoadSceneTcs.TryGetValue(ne.SceneAssetName,out var tcs))
            {
                tcs.TrySetResult(true);
                LoadSceneTcs.Remove(ne.SceneAssetName);
            }
        }

        private static void OnLoadSceneFailed(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneFailureEventArgs)e;
            if (LoadSceneTcs.TryGetValue(ne.SceneAssetName,out var tcs))
            {
                Log.Error(ne.ErrorMessage);
                tcs.TrySetException(new GameFrameworkException(ne.ErrorMessage));
                LoadSceneTcs.Remove(ne.SceneAssetName);
            }
        }
    }
}