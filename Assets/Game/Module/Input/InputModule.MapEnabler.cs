using Cysharp.Threading.Tasks;
using Rewired;
using UnityGameFramework.Runtime;

namespace Game.Module
{
    public partial class InputModule
    {
        private ControllerMapEnabler.RuleSet m_DisableAll;
        private ControllerMapEnabler.RuleSet m_Battle;
        private ControllerMapEnabler.RuleSet m_UI;

        private async UniTask InitMapEnabler()
        {
            m_DisableAll = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.DisableAll);
            m_Battle = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.Battle);
            m_UI = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.UI);
            
            m_Player.controllers.maps.mapEnabler.ruleSets.Add(m_DisableAll);
            m_Player.controllers.maps.mapEnabler.ruleSets.Add(m_Battle);
            m_Player.controllers.maps.mapEnabler.ruleSets.Add(m_UI);

            ChangeInputMap(RewiredConsts.MapEnablerRuleSet.Battle);
            
            await UniTask.CompletedTask;
        }

        public void ChangeInputMap(int map)
        {
            Log.Info("change input map:{0}",map);
            
            m_DisableAll.enabled = false;
            m_Battle.enabled = false;
            m_UI.enabled = false;
            
            switch (map)
            {
                case RewiredConsts.MapEnablerRuleSet.UI:
                    m_UI.enabled = true;
                    break;
                case RewiredConsts.MapEnablerRuleSet.Battle:
                    m_Battle.enabled = true;
                    break;
                case RewiredConsts.MapEnablerRuleSet.DisableAll:
                    m_DisableAll.enabled = true;
                    break;
            }
            
            m_Player.controllers.maps.mapEnabler.Apply();
        }
    }
}