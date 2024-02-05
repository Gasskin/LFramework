using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Game.Procedure
{
    public class TestProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            var comp = GameEntry.GetComponent<GameUpdaterComponent>();
            comp.GetModule<InputModule.InputModule>();
            comp.GetModule<ConfigModule.ConfigModule>();

            await comp.InitAsync();

            base.OnEnter(procedureOwner);
        }
    }
}
