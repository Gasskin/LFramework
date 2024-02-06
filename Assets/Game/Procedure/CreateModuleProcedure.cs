using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Game.Procedure
{
    public class CreateModuleProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            var comp = GameEntry.GetComponent<GameUpdaterComponent>();
            comp.GetModule<ConfigModule.ConfigModule>();
            comp.GetModule<InputModule.InputModule>();

            await comp.InitAsync();

            base.OnEnter(procedureOwner);
        }
    }
}
