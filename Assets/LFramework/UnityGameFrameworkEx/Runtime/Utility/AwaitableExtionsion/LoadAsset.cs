using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

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
        
        /// <summary>
        /// 加载多个资源（可等待）
        /// </summary>
        public static async UniTask<T[]> LoadAssetsAsync<T>(this ResourceComponent resourceComponent, string[] assetName) where T : UnityEngine.Object
        {
            if (assetName == null)
            {
                return null;
            }
            var assets = new T[assetName.Length];
            var tasks = new UniTask<T>[assets.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = resourceComponent.LoadAssetAsync<T>(assetName[i]);
            }
            return await UniTask.WhenAll(tasks);
        }
    }
}
