using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Resource;

namespace UnityGameFramework.Runtime
{
    public static partial class AwaitableExtension
    {
        /// <summary>
        /// 加载资源（可等待）
        /// </summary>
        public static UniTask<T> LoadAssetAsync<T>(this ResourceComponent resourceComponent, string assetName) where T : UnityEngine.Object
        {
            var tcs = AutoResetUniTaskCompletionSource<T>.Create();
            resourceComponent.LoadAsset(assetName, typeof(T), new LoadAssetCallbacks(
                (_, asset, _, _) =>
                {
                    var source = tcs;
                    tcs = null;
                    T tAsset = asset as T;
                    if (tAsset != null)
                    {
                        source.TrySetResult(tAsset);
                    }
                    else
                    {
                        var err = $"Load asset failure,can not convert type {asset.GetType()} to target {typeof(T)}";
                        Log.Error(err);
                        source.TrySetException(new GameFrameworkException(err));
                    }
                },
                (_, _, errorMessage, _) =>
                {
                    Log.Error(errorMessage);
                    tcs.TrySetException(new GameFrameworkException(errorMessage));
                }
            ));

            return tcs.Task;
        }
    }
}
