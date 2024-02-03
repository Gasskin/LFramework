using System;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace LFramework
{
    public class TestProcedure : ProcedureBase
    {
        protected override async void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            LFrameworkEntry.GetModule<PlayerInputModule>();

            await LFrameworkEntry.InitAsync();

            base.OnEnter(procedureOwner);
        }
    }
}
