using System;
using System.Linq;
using System.Reflection;
using Game.Module;

using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Logic
{
    public class CreateModuleProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#else
            // Editor下无需加载，直接查找获得HotUpdate程序集
            Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Game");
#endif
    
            var cls = hotUpdateAss.GetType("Game.Logic.GameEntry");
            if (cls != null)
            {
                var method = cls.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
            
            // GameComponent.GameDriver.GetModule<ConfigDriver>();
            // GameComponent.GameDriver.GetModule<LocalizationDriver>();
            // GameComponent.GameDriver.GetModule<InputDriver>();
            // GameComponent.GameDriver.GetModule<EntityComponentDriver>();
            //
            // await GameComponent.GameDriver.InitAsync();
            //
            // ChangeState<LaunchProcedure>(procedureOwner);
        }
    }
}
