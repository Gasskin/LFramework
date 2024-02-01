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
            base.OnEnter(procedureOwner);
            TestWait("111", 1000);
            TestWait("222", 2000);
            TestWait("333", 0);
            TestWait("444", 2000);
            TestWait("555", 1000);
        }

        public async void TestWait(string tag, int delayTime)
        {
            var comp = GameEntry.GetComponent<CoroutineLockComponent>();
            using (var clock = await comp.Wait(ECoroutineLockType.Test, 1))
            {
                Log.Error($"before {tag} {DateTime.UtcNow.Ticks/10000}");
                if (delayTime > 0) 
                {
                    await UniTask.Delay(delayTime);
                }
                Log.Error($"after {tag} {DateTime.UtcNow.Ticks/10000}");
            }
        }
    }
}
