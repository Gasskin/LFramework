using Cysharp.Threading.Tasks;
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
            TestWait("111",3000);
            TestWait("222");
            TestWait("333");
            TestWait("444");
            TestWait("555", 0, 1000);
        }

        public async void TestWait(string tag, int time = 0, long outTime = 60000)
        {
            var comp = GameEntry.GetComponent<CoroutineLockComponent>();
            using (await comp.Wait(1, 1,outTime))
            {
                Log.Error($"before {tag}");
                await UniTask.Delay(time);
                Log.Error($"after {tag}");
            }
        }
    }
}
