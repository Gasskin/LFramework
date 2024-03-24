using System;
using GameFramework;
using GameFramework.GameDriver;

namespace UnityGameFramework.Runtime
{
    public class GameDriverComponent: GameFrameworkComponent
    {
        private IGameDriverManager m_GameDriverManager;

        protected override void Awake()
        {
            base.Awake();
            m_GameDriverManager = GameFrameworkEntry.GetModule<IGameDriverManager>();
        }

        private void Update()
        {
            m_GameDriverManager.OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            m_GameDriverManager.OnLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            m_GameDriverManager.OnFixedUpdate?.Invoke();
        }
        
        public void SetUpdateAction(Action action)
        {
            m_GameDriverManager.SetUpdateAction(action);
        }

        public void SetLateUpdateAction(Action action)
        {
            m_GameDriverManager.SetLateUpdateAction(action);
        }

        public void SetFixedUpdateAction(Action action)
        {
            m_GameDriverManager.SetFixedUpdateAction(action);
        }
        
        public void SetShutDownAction(Action action)
        {
            m_GameDriverManager.SetShutDownAction(action);
        }
    }
}
