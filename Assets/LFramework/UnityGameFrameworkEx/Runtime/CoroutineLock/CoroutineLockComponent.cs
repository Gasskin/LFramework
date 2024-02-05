using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.CoroutineLock;

namespace UnityGameFramework.Runtime
{
    
    public class CoroutineLockComponent : GameFrameworkComponent
    {
        private ICoroutineLockManager m_Manager;

        protected override void Awake()
        {
            base.Awake();
            m_Manager = GameFrameworkEntry.GetModule<ICoroutineLockManager>();
        }

        /// <summary>
        /// 协程锁
        /// </summary>
        /// <param name="lockType">锁的类型，必须>0</param>
        /// <param name="key">某把锁的KEY</param>
        /// <param name="lockTime">加锁时间，毫秒</param>
        /// <returns></returns>
        public async UniTask<CoroutineLockInfo> Wait(ECoroutineLockType lockType , long key, long lockTime = 60000)
        {
            return await m_Manager.Wait((int)lockType, key, lockTime);
        }
    }
}