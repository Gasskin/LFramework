﻿using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Game.Runtime
{
    public class MainProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ChangeState<InitPackageProcedure>(procedureOwner);
        }
    }
}