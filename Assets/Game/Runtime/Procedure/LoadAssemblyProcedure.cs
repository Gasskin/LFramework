using System.IO;
using System.Linq;
using System.Reflection;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace Game.Runtime
{
    public class LoadAssemblyProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
#if UNITY_EDITOR
            var hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotFix");
            var cls = hotUpdateAss.GetType("Game.HotFix.Logic.HotFixEntry");
            if (cls != null)
            {
                var method = cls.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
#else
            var hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotFix.dll.bytes"));
#endif
        }
    }
}