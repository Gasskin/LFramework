using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    public static partial class AwaitableExtension
    {
        private static void RegisterOpenUIEvent()
        {
            var comp = GameEntry.GetComponent<EventComponent>();
            comp.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            comp.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
        }

        private static readonly Dictionary<int, AutoResetUniTaskCompletionSource<UIForm>> UITcs = new();

        public static async UniTask<UIForm> OpenUIFormAsync(this UIComponent uiComponent, string uiFormAssetName, string uiGroupName, int priority, bool pauseCoveredUIForm = false, object userData = null)
        {
            var comp = GameEntry.GetComponent<CoroutineLockComponent>();
            var tcs = AutoResetUniTaskCompletionSource<UIForm>.Create();

            using var coroutineLock = await comp.Wait(ECoroutineLockType.OpenUI, uiFormAssetName.GetHashCode());
            int serialId = uiComponent.OpenUIForm(uiFormAssetName, uiGroupName, priority, pauseCoveredUIForm, userData);
            UITcs.Add(serialId, tcs);

            return await tcs.Task;
        }

        private static void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            UITcs.TryGetValue(ne.UIForm.SerialId, out AutoResetUniTaskCompletionSource<UIForm> tcs);
            if (tcs != null)
            {
                tcs.TrySetResult(ne.UIForm);
                UITcs.Remove(ne.UIForm.SerialId);
            }
        }

        private static void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            OpenUIFormFailureEventArgs ne = (OpenUIFormFailureEventArgs)e;
            UITcs.TryGetValue(ne.SerialId, out AutoResetUniTaskCompletionSource<UIForm> tcs);
            if (tcs != null)
            {
                Log.Error(ne.ErrorMessage);
                tcs.TrySetException(new GameFrameworkException(ne.ErrorMessage));
                UITcs.Remove(ne.SerialId);
            }
        }
    }
}