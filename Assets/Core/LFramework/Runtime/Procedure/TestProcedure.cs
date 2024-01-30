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
            TestWait("111");
            TestWait("222");
            TestWait("333");
        }

        public async void TestWait(string tag)
        {
            var comp = GameEntry.GetComponent<CoroutineLockComponent>();
            using (await comp.Wait(1, 1))
            {
                Log.Error($"before {tag}");
                await UniTask.Delay(3000);
                Log.Error($"after {tag}");
            }
        }
    }
}
