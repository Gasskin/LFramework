using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Procedure
{
    public class CreateModuleProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            var comp = GameEntry.GetComponent<GameUpdaterComponent>();
            comp.GetModule<ConfigModule.ConfigModule>();
            comp.GetModule<InputModule.InputModule>();

            await comp.InitAsync();

            var scene = GameEntry.GetComponent<SceneComponent>();
            var result = await scene.LoadSceneAsync("Assets/AssetsPackage/Scene/SampleScene.unity");
            Log.Error("load scene? {0}",result);
        }
    }
}
