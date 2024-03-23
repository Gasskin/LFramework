using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.GameUpdater;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class GameDriverComponent: GameFrameworkComponent
    {
        public float DeltaTime { get; private set; }
        
        private IGameDriverManager m_GameDriverManager;
        private bool m_Initialized;

        protected override void Awake()
        {
            base.Awake();
            m_GameDriverManager = GameFrameworkEntry.GetModule<IGameDriverManager>();
        }

        public async UniTask InitAsync()
        {
            if (m_Initialized)
            {
                return;
            }
            await m_GameDriverManager.InitAsync();
            m_Initialized = true;
        }

        public T GetModule<T>() where T : GameDriverBase
        {
            return m_GameDriverManager.GetModule<T>();
        }
        
        private void Update()
        {
            if (m_Initialized)
            {
                DeltaTime = Time.deltaTime;
                m_GameDriverManager.Update(DeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (m_Initialized)
            {
                m_GameDriverManager.LateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (m_Initialized)
            {
                m_GameDriverManager.FixedUpdate();
            }
        }
    }
}
