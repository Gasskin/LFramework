using GameFramework.Fsm;
using GameFramework.Procedure;
using LFramework.LInput;
using UnityGameFramework.Runtime;

namespace LFramework.LProcedure
{
    public class TestProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            var comp = GameEntry.GetComponent<GameUpdaterComponent>();
            comp.GetModule<InputModule>();
            comp.GetModule<ConfigModule>();

            await comp.InitAsync();

            base.OnEnter(procedureOwner);
        }
    }
}
