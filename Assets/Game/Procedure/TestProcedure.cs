using GameFramework.Fsm;
using GameFramework.Procedure;
using LFramework.LInput;

namespace LFramework.LProcedure
{
    public class TestProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            LFrameworkEntry.GetModule<LInputModule>();
            LFrameworkEntry.GetModule<LConfigModule>();

            await LFrameworkEntry.InitAsync();

            base.OnEnter(procedureOwner);
        }
    }
}
