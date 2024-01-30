using Cysharp.Threading.Tasks;
using GameFramework;
using UnityGameFramework.Runtime;

namespace LFramework
{
    
    public class CoroutineLockComponent : GameFrameworkComponent
    {
        private ICoroutineLockManager m_Manager;

        protected override void Awake()
        {
            base.Awake();
            m_Manager = GameFrameworkEntry.GetModule<ICoroutineLockManager>();
        }

        public async UniTask<CoroutineLock> Wait(int lockType, long key, long lockTime = 60000)
        {
            return await m_Manager.Wait(lockType, key, lockTime);
        }
    }
}