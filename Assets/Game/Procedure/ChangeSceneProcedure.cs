using Cysharp.Threading.Tasks;
using Game.GlobalDefinition;
using Game.Utility;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Game.Procedure
{
    public class ChangeSceneProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 隐藏所有实体
            GameComponent.Entity.HideAllLoadingEntities();
            GameComponent.Entity.HideAllLoadedEntities();

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameComponent.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameComponent.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameComponent.Base.ResetNormalGameSpeed();

            var scene = procedureOwner.GetData<VarString>("NextScene");
            var flag = await GameComponent.Scene.LoadSceneAsync(scene, Const.AssetPriority.SceneAsset);
            Log.Info("change scene:{1} success?{0}", flag, scene);
            if (!flag)
            {
                return;
            }
            // await UniTask.Yield();
            //
            // if (scene == ResourcesPathConfig.Scene.Main)
            // {
            //     Log.Error("Main");
            // }
            // else if (scene == ResourcesPathConfig.Scene.Menu)
            // {
            //     ChangeState<MenuProcedure>(procedureOwner);
            // }
        }
    }
}