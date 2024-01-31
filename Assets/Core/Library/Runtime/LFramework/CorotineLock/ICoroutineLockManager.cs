using Cysharp.Threading.Tasks;

namespace GameFramework
{
    public interface ICoroutineLockManager
    {
        /// <summary>
        /// 协程锁
        /// </summary>
        /// <param name="lockType">锁类型</param>
        /// <param name="key">关键字</param>
        /// <param name="lockTime">锁的时间，超时自动失效</param>
        /// <returns></returns>
        UniTask<CoroutineLockInfo> Wait(int lockType, long key, long lockTime = 60000);

        /// <summary>
        /// 执行下一个任务
        /// </summary>
        /// <param name="lockType"></param>
        /// <param name="key"></param>
        /// <param name="level"></param>
        void RunNextCoroutine(int lockType, long key, int level);
    }
}
