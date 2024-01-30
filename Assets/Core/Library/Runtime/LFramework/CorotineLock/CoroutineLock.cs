using System;
using Cysharp.Threading.Tasks;

namespace GameFramework
{
    public class CoroutineLock : IDisposable, IReference
    {
        // 锁的类型，比如所有的资源加载操作，共享一个锁类型
        public int LockType { get; private set; }

        // 锁的唯一KEY，虽然都是资源加载，但是加载不同的资源，KEY应该不同
        public long Key { get; private set; }

        // 层级，代表同类型的一个锁，有多少把，太多了可能就有异常了
        public int Level { get; private set; }

        // 解锁的时间，毫秒
        public long UnLockTime { get; private set; }

        // TCS
        public AutoResetUniTaskCompletionSource<CoroutineLock> Tcs { get; private set; }

        public void Init(int lockType, long key, int level, long unLockTime, AutoResetUniTaskCompletionSource<CoroutineLock> tcs)
        {
            LockType = lockType;
            Key = key;
            Level = level;
            UnLockTime = unLockTime;
            Tcs = tcs;
        }

        /// <summary>
        /// 单纯只是超时标记，会被我们主动释放
        /// </summary>
        public void MarkTimeOut()
        {
            LockType = 0;
        }

        public void Dispose()
        {
            // 如果是被动Dispose，那么正常走if，但也有可能超时了被主动释放了
            if (LockType > 0) 
            {
                GameFrameworkEntry.GetModule<ICoroutineLockManager>().RunNextCoroutine(LockType, Key, Level + 1);
            }
            Clear();
            ReferencePool.Release(this);
        }

        public void Clear()
        {
            LockType = 0;
            Key = 0;
            Level = 0;
            UnLockTime = 0;
        }
    }
}