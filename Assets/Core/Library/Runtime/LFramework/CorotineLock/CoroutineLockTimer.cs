using Cysharp.Threading.Tasks;

namespace GameFramework
{
    public class CoroutineLockTimer: IReference
    {
        public long m_LockTime;
        public AutoResetUniTaskCompletionSource<CoroutineLockInfo> m_Tcs;
        
        public void Clear()
        {
            m_LockTime = 0;
            m_Tcs = null;
        }
    }
}