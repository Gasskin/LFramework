using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LFramework;

namespace GameFramework
{
    public class CoroutineLockManager : GameFrameworkModule, ICoroutineLockManager
    {
        // 存储所有的锁，第一层KEY是锁的类型，第二层KEY是锁的关键字
        private readonly Dictionary<int, Dictionary<long, Queue<CoroutineLock>>> m_AllLock = new();
        // 下一帧释放的锁的类型信息，LockType,Key,Level
        private readonly Queue<(int, long, int)> m_RunNextFrame = new();
        // 时间锁检查，ID是过期时间，毫秒
        private readonly MultiMap<long, CoroutineLock> m_LockTimer = new();
        // 当前帧过期的锁
        private Queue<long> m_TimeOutIds = new();
        private Queue<CoroutineLock> m_TimeOutLocks = new();

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            var now = DateTime.UtcNow.Millisecond;
            m_TimeOutIds.Clear();
            m_TimeOutLocks.Clear();

            // 找到所有过期的ID
            foreach (var timer in m_LockTimer)
            {
                if (timer.Key > now)
                {
                    break;
                }
                m_TimeOutIds.Enqueue(timer.Key);
            }

            // 找到所有过期的锁
            while (m_TimeOutIds.Count > 0)
            {
                var id = m_TimeOutIds.Dequeue();
                if (m_LockTimer.TryGetValue(id, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        m_TimeOutLocks.Enqueue(list[i]);
                    }
                }
                m_LockTimer.Remove(id);
            }

            while (m_TimeOutLocks.Count > 0)
            {
                var coroutineLock = m_TimeOutLocks.Dequeue();
                coroutineLock.MarkTimeOut();
                RunNextCoroutine(coroutineLock.LockType,coroutineLock.Key,coroutineLock.Level);
            }
            
            // 循环过程中会有对象继续加入队列
            while (m_RunNextFrame.Count > 0)
            {
                (int lockType, long key, int level) = m_RunNextFrame.Dequeue();
                SetResult(lockType, key, level);
            }
        }

        internal override void Shutdown()
        {
        }

        public async UniTask<CoroutineLock> Wait(int lockType, long key, long lockTime = 60000)
        {
            if (!m_AllLock.TryGetValue(lockType, out var lockTypeDic))
            {
                lockTypeDic = new Dictionary<long, Queue<CoroutineLock>>();
                m_AllLock.Add(lockType, lockTypeDic);
            }

            // 如果还没有这个锁的队列，那说明没有需要等待的任务，直接返回就行了
            if (!lockTypeDic.TryGetValue(key, out var lockQueue))
            {
                lockQueue = new Queue<CoroutineLock>();
                lockTypeDic.Add(key, lockQueue);

                return CreateCoroutineLock(lockType, key, lockTime, 1);
            }

            var tcs = AutoResetUniTaskCompletionSource<CoroutineLock>.Create();
            var coroutineLock = ReferencePool.Acquire<CoroutineLock>();
            coroutineLock.Init(lockType, key, 1, 0, tcs);
            return await tcs.Task;
        }

        public void RunNextCoroutine(int lockType, long key, int level)
        {
            if (level >= 10)
            {
                GameFrameworkLog.Info($"Maybe too much coroutine lock [{lockType}][{key}][{level}]");
            }

            if (level >= 50)
            {
                GameFrameworkLog.Warning($"Maybe too much coroutine lock [{lockType}][{key}][{level}]");
            }

            if (level >= 100)
            {
                GameFrameworkLog.Error($"Maybe too much coroutine lock [{lockType}][{key}][{level}]");
            }

            m_RunNextFrame.Enqueue((lockType, key, level));
        }

        private void SetResult(int lockType, long key, int level)
        {
            if (m_AllLock.TryGetValue(lockType, out var lockTypeDic))
            {
                if (lockTypeDic.TryGetValue(key, out var lockQueue))
                {
                    if (lockQueue.Count == 0)
                    {
                        lockTypeDic.Remove(key);
                        return;
                    }

                    var coroutineLock = lockQueue.Dequeue();
                    coroutineLock.Tcs?.TrySetResult(CreateCoroutineLock(lockType, key, coroutineLock.UnLockTime, level));
                }
            }
        }
        
        private CoroutineLock CreateCoroutineLock(int lockType, long key, long lockTime, int level)
        {
            var coroutineLock = ReferencePool.Acquire<CoroutineLock>();
            coroutineLock.Init(lockType, key, 0, level, null);
            if (lockTime > 0)
            {
                var untilTime = DateTime.UtcNow.Millisecond + lockTime;
                m_LockTimer.Add(untilTime, coroutineLock);
            }
            return coroutineLock;
        }
    }
}