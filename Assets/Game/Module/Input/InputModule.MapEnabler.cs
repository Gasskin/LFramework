using Cysharp.Threading.Tasks;
using LFramework.LInput;
using Rewired;

namespace Game.InputModule
{
    public partial class InputModule
    {
        private ControllerMapEnabler.RuleSet m_DisableAll;
        private ControllerMapEnabler.RuleSet m_Normal;
        
        public async UniTask InitMapEnabler()
        {
            m_DisableAll =
                ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.DisableAll);
            m_Normal = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.Normal);
            
            m_Player.controllers.maps.mapEnabler.ruleSets.Add(m_DisableAll);
            m_Player.controllers.maps.mapEnabler.ruleSets.Add(m_Normal);

            await UniTask.CompletedTask;
        }

        private int test = 0;
        public void ChangeMapEnabler()
        {
            test++;
            m_DisableAll.enabled = test % 2 == 0;
            m_Normal.enabled = test % 2 != 0;
            
            m_Player.controllers.maps.mapEnabler.Apply();
        }
    }
}