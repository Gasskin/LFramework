using System.IO;
using System.Linq;
using System.Reflection;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Runtime
{
    public class LoadAssemblyProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
#if UNITY_EDITOR
            var hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotFix");
#else
            var hotUpdateAss = Assembly.Load(await File.ReadAllBytesAsync($"{Application.streamingAssetsPath}/HotFix.dll.bytes"));
#endif
            var cls = hotUpdateAss.GetType("Game.HotFix.HotFixEntry");
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