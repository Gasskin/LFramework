using Game.GlobalDefinition;
using Game.Logic.Utility;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    public class MenuProcedure: ProcedureBase
    {
        private bool m_Start = false;
        private UIForm m_Menu;
        
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_Start = false;
            m_Menu = await GameComponent.UI.OpenUIFormAsync("Assets/AssetsPackage/UI/MenuForm.prefab", "Default", Const.AssetPriority.UIFormAsset, true, this);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_Start)
            {
            }
        }
    }
}