using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.GameUpdater;

namespace UnityGameFramework.Runtime
{
    public class GameUpdaterComponent: GameFrameworkComponent
    {
        private IGameUpdater m_GameUpdaterManager;
        private bool m_Initialized;

        public async UniTask InitAsync()
        {
            if (m_Initialized)
            {
                return;
            }
            m_GameUpdaterManager = GameFrameworkEntry.GetModule<IGameUpdater>();
            await m_GameUpdaterManager.InitAsync();
            m_Initialized = true;
        }

        public T GetModule<T>() where T : GameModuleBase
        {
            return m_GameUpdaterManager.GetModule<T>();
        }
        
        private void Update()
        {
            if (m_Initialized)
            {
                m_GameUpdaterManager.Update();
            }
        }

        private void LateUpdate()
        {
            if (m_Initialized)
            {
                m_GameUpdaterManager.LateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (m_Initialized)
            {
                m_GameUpdaterManager.FixedUpdate();
            }
        }
    }
}
