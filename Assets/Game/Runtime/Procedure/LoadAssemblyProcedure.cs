using System.IO;
using System.Linq;
using System.Reflection;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game.Runtime
{
    public class LoadAssemblyProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var asset = GameEntry.GetComponent<AssetComponent>();
            var playMode = asset.m_PlayMode;

            Assembly hotFix;
#if UNITY_EDITOR
            if (playMode == EPlayMode.EditorSimulateMode)
            {
                hotFix = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotFix");
            }
            else
#endif
            {
                var dll = await asset.LoadAssetAsync<TextAsset>("Assets/Bundles/DLL/HotFix.dll.bytes");
                hotFix = Assembly.Load(dll.bytes);
            }
            var cls = hotFix.GetType("Game.HotFix.HotFixEntry");
            if (cls != null)
            {
                var method = cls.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
                if (method != null)
                {
                    method.Invoke(null, null);
                }
                else
                {
                    Log.Fatal("加载HotFix.dll失败，不存在入口方法");
                }
            }
            else
            {
                Log.Fatal("加载HotFix.dll失败，不存在入口类");
            }
        }
    }
}