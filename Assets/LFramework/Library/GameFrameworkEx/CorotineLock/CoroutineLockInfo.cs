using System;
using Cysharp.Threading.Tasks;

namespace GameFramework.CoroutineLock
{
    public class CoroutineLockInfo : IDisposable, IReference
    {
        public int m_LockType;
        public long m_Key;
        public int m_Level;
        
        /// <summary>
        /// 如果是在using结束由系统释放的，那么LockType必然>0，此时会在Dispose方法中主动推进锁系统
        /// 但也有可能锁超时了，我们会主动释放，此时会标记TimeOut，也就是LockType归零，然后等系统释放资源时，就不会再去推进锁系统了
        /// </summary>
        public void MarkTimeOut()
        {
            m_LockType = 0;
        }
        
        public void Dispose()
        {
            if (m_LockType > 0)
            {
                GameFrameworkEntry.GetModule<ICoroutineLockManager>().RunNextCoroutine(m_LockType, m_Key, m_Level + 1);
            }
            m_LockType = 0;
            m_Key = 0;
            m_Level = 0;
            // 归还引用
            ReferencePool.Release(this);
        }

        public void Clear()
        {
            m_LockType = 0;
            m_Key = 0;
            m_Level = 0;
        }
    }
}