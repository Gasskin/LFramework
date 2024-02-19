using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.GameUpdater;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class GameUpdaterComponent: GameFrameworkComponent
    {
        public float DeltaTime { get; private set; }
        
        private IGameUpdaterManager m_GameUpdaterManagerManager;
        private bool m_Initialized;

        protected override void Awake()
        {
            base.Awake();
            m_GameUpdaterManagerManager = GameFrameworkEntry.GetModule<IGameUpdaterManager>();
        }

        public async UniTask InitAsync()
        {
            if (m_Initialized)
            {
                return;
            }
            await m_GameUpdaterManagerManager.InitAsync();
            m_Initialized = true;
        }

        public T GetModule<T>() where T : GameModuleBase
        {
            return m_GameUpdaterManagerManager.GetModule<T>();
        }
        
        private void Update()
        {
            if (m_Initialized)
            {
                DeltaTime = Time.deltaTime;
                m_GameUpdaterManagerManager.Update(DeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (m_Initialized)
            {
                m_GameUpdaterManagerManager.LateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (m_Initialized)
            {
                m_GameUpdaterManagerManager.FixedUpdate();
            }
        }
    }
}
