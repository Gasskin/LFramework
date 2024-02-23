using Game.Logic;
using Game.Module;
using Game.Module;
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

            GameComponent.GameUpdater.GetModule<ConfigModule>();
            GameComponent.GameUpdater.GetModule<LocalizationModule>();
            GameComponent.GameUpdater.GetModule<InputModule>();
            GameComponent.GameUpdater.GetModule<EntityModule>();

            await GameComponent.GameUpdater.InitAsync();

            ChangeState<LaunchProcedure>(procedureOwner);
        }
    }
}
