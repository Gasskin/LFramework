using Game.Utility;
using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Procedure
{
    public class CreateModuleProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameComponent.GameUpdater.GetModule<ConfigModule.ConfigModule>();
            GameComponent.GameUpdater.GetModule<ConfigModule.LocalizationModule>();
            GameComponent.GameUpdater.GetModule<InputModule.InputModule>();

            await GameComponent.GameUpdater.InitAsync();

            ChangeState<LaunchProcedure>(procedureOwner);
        }
    }
}
