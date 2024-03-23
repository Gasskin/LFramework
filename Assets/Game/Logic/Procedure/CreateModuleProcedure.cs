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

            GameComponent.GameDriver.GetModule<ConfigDriver>();
            GameComponent.GameDriver.GetModule<LocalizationDriver>();
            GameComponent.GameDriver.GetModule<InputDriver>();
            GameComponent.GameDriver.GetModule<EntityComponentDriver>();

            await GameComponent.GameDriver.InitAsync();

            ChangeState<LaunchProcedure>(procedureOwner);
        }
    }
}
