using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.CoroutineLock
{
    public class CoroutineLockManager : GameFrameworkModule, ICoroutineLockManager
    {
        // 所有的锁，第一层的键是LockType，第二层的键是Key
        private readonly Dictionary<int, Dictionary<long, Queue<CoroutineLockTimer>>> m_AllLockDic = new();

        // 记录锁的过期时间，KEY就是过期时间，不同锁的过期时间是可以重复的
        private readonly GameFrameworkMultiDictionary<long, CoroutineLockInfo> m_LockTimerDic = new();
        // 最接近的过期时间
        private long m_ClosetOutTime = long.MaxValue;

        // 下一帧释放的锁的信息，LockType,Key,Level
        private readonly Queue<(int,long,int)> m_RunNextFrameQueue = new();

        // 记录已经过期的锁
        private readonly Queue<long> m_OutTimerIdQueue = new();
        private readonly Queue<CoroutineLockInfo> m_OutTimeLockQueue = new();

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            // 没有超时锁
            if (m_LockTimerDic.Count <= 0)
            {
                return;
            }

            var now = DateTime.UtcNow.Ticks / 10000;
            if (now > m_ClosetOutTime)
            {
                return;
            }

            // 遍历所有锁，检查时间
            m_OutTimerIdQueue.Clear();
            using (var t = m_LockTimerDic.GetEnumerator())
            {
                var min = long.MaxValue;
                while (t.MoveNext())
                {
                    var outTime = t.Current.Key;
                    if (outTime <= now)
                    {
                        m_OutTimerIdQueue.Enqueue(outTime);
                    }
                    // 没过期的锁里头，最早的过期时间
                    else if (outTime < min)
                    {
                        min = outTime;
                    }
                }
                m_ClosetOutTime = min;
            }

            // 找到所有过期的锁
            m_OutTimeLockQueue.Clear();
            while (m_OutTimerIdQueue.Count > 0)
            {
                var id = m_OutTimerIdQueue.Dequeue();
                if (m_LockTimerDic.TryGetValue(id, out var lockTimers)) 
                {
                    using (var t = lockTimers.GetEnumerator())
                    {
                        while (t.MoveNext())
                        {
                            m_OutTimeLockQueue.Enqueue(t.Current);
                        }
                    }

                    m_LockTimerDic.RemoveAll(id);
                }
            }
            
            // 执行
            while (m_OutTimeLockQueue.Count > 0)
            {
                var coroutineLock = m_OutTimeLockQueue.Dequeue();
                RunNextCoroutine(coroutineLock.m_LockType, coroutineLock.m_Key, coroutineLock.m_Level + 1);
                // 这里这个Lock最终来自于m_LockTimerDic，只会在CreateCoroutineLock里生成，并且生成结果会直接返回给using
                // 所以这里必须MarkTimeOut，因为过期了，我们已经主动抛出了，不能在using结束后的Dispose方法里不需要再次抛出
                coroutineLock.MarkTimeOut();
                // 这里不用回收，using结束后，Dispose方法中会回收的
                // ReferencePool.Release(coroutineLock);
            }

            while (m_RunNextFrameQueue.Count > 0)
            {
                var (lockType,key,level) = m_RunNextFrameQueue.Dequeue();
                SetResult(lockType, key, level);
            }
        }

        internal override void Shutdown()
        {
        }

        public async UniTask<CoroutineLockInfo> Wait(int lockType, long key, long lockTime = 60000)
        {
            if (!m_AllLockDic.TryGetValue(lockType, out var lockKeyDic))
            {
                lockKeyDic = new Dictionary<long, Queue<CoroutineLockTimer>>();
                m_AllLockDic.Add(lockType, lockKeyDic);
            }

            // 如果没有这个key的锁队列，说明没有需要等待的，直接返回
            if (!lockKeyDic.TryGetValue(key, out var lockTimerQueue))
            {
                lockTimerQueue = new Queue<CoroutineLockTimer>();
                lockKeyDic.Add(key, lockTimerQueue);

                return CreateCoroutineLock(lockType, key, lockTime, 1);
            }

            // 否则，说明已经有进行中的任务了，此时不会直接创建锁，而是创建等待的Task
            var lockTimer = ReferencePool.Acquire<CoroutineLockTimer>();
            lockTimer.m_LockTime = lockTime;
            lockTimer.m_Tcs = AutoResetUniTaskCompletionSource<CoroutineLockInfo>.Create();

            lockTimerQueue.Enqueue(lockTimer);

            return await lockTimer.m_Tcs.Task;
        }

        public void RunNextCoroutine(int lockType, long key, int level)
        {
            if (lockType <= 0) 
            {
                return;
            }
            
            if (level >= 10)
            {
                GameFrameworkLog.Info($"maybe too much lock [{lockType}][{key}][{level}]");
            }

            if (level >= 50)
            {
                GameFrameworkLog.Warning($"maybe too much lock [{lockType}][{key}][{level}]");
            }

            if (level >= 100)
            {
                GameFrameworkLog.Error($"maybe too much lock [{lockType}][{key}][{level}]");
            }

            m_RunNextFrameQueue.Enqueue((lockType,key,level));
        }

        private CoroutineLockInfo CreateCoroutineLock(int lockType, long key, long lockTime, int level)
        {
            var coroutineLock = ReferencePool.Acquire<CoroutineLockInfo>();
            coroutineLock.m_LockType = lockType;
            coroutineLock.m_Key = key;
            coroutineLock.m_Level = level;

            if (lockTime > 0)
            {
                var time = DateTime.UtcNow.Ticks / 10000 + lockTime;
                m_LockTimerDic.Add(time, coroutineLock);
                if (time < m_ClosetOutTime)
                {
                    m_ClosetOutTime = time;
                }
            }

            return coroutineLock;
        }

        private void SetResult(int lockType, long key, int level)
        {
            if (m_AllLockDic.TryGetValue(lockType,out var lockKeyDic))
            {
                if (lockKeyDic.TryGetValue(key, out var lockTimerQueue))
                {
                    if (lockTimerQueue.Count <= 0)
                    {
                        lockKeyDic.Remove(key);
                        return;
                    }

                    var lockTimer = lockTimerQueue.Dequeue();
                    lockTimer.m_Tcs?.TrySetResult(CreateCoroutineLock(lockType, key, lockTimer.m_LockTime, level));
                    ReferencePool.Release(lockTimer);
                }
            }
        }
    }
}