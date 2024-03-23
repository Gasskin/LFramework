using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Logic
{
    public class InitPackageProcedure: ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var t = GameComponent.Asset.m_PlayMode;
        }
    }
}