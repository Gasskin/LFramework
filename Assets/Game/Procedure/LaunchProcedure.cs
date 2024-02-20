using System;
using Cysharp.Threading.Tasks;
using Game.GlobalDefinition;
using Game.Utility;
using GameFramework.Fsm;
using GameFramework.Localization;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;

namespace Game.Procedure
{
    public class LaunchProcedure: ProcedureBase
    {

        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            InitLanguageSettings();
            InitCurrentVariant();
            
            await UniTask.Yield();
            
            if (GameComponent.Base.EditorResourceMode)
            {
                // 编辑器模式
                Log.Info("Editor resource mode detected.");
                procedureOwner.SetData<VarString>("NextScene", ResourcesPathConfig.Scene.SampleScene);
                ChangeState<ChangeSceneProcedure>(procedureOwner);
            }
            else if (GameComponent.Resource.ResourceMode == ResourceMode.Package)
            {
                // 单机模式
                Log.Error("todo");
                // ChangeState<ProcedureInitResources>(procedureOwner);
            }
            else
            {
                // 可更新模式
                Log.Error("todo");
                // ChangeState<ProcedureCheckVersion>(procedureOwner);
            }
        }

        
        private void InitLanguageSettings()
        {
            var language = GameModule.Localization.Language;
            if (GameComponent.Setting.HasSetting(Const.Setting.Language))
            {
                string languageString = GameComponent.Setting.GetString(Const.Setting.Language);
                if (Enum.TryParse<Language>(languageString,out var result))
                {
                    language = result;
                }
            }

            if (language != Language.English
                && language != Language.ChineseSimplified
                && language != Language.ChineseTraditional
                && language != Language.Korean)
            {
                // 若是暂不支持的语言，则使用英语
                language = Language.English;

                GameComponent.Setting.SetString(Const.Setting.Language, language.ToString());
                GameComponent.Setting.Save();
            }

            GameModule.Localization.ChangeLanguage(language);
        }
        
        private void InitCurrentVariant()
        {
            if (GameComponent.Base.EditorResourceMode)
            {
                // 编辑器资源模式不使用 AssetBundle，也就没有变体了
                return;
            }

            string currentVariant = null;
            switch (GameModule.Localization.Language)
            {
                case Language.English:
                    currentVariant = "en-us";
                    break;

                case Language.ChineseSimplified:
                    currentVariant = "zh-cn";
                    break;

                case Language.ChineseTraditional:
                    currentVariant = "zh-tw";
                    break;

                case Language.Korean:
                    currentVariant = "ko-kr";
                    break;

                default:
                    currentVariant = "zh-cn";
                    break;
            }

            GameComponent.Resource.SetCurrentVariant(currentVariant);
        }

    }
}