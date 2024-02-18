using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;

namespace Game.Entitas
{
    public class EntitasModule: GameModuleBase
    {
        public override int Priority => (int)EModulePriority.None;
        public override async UniTask InitAsync()
        {
            m_ContextGame = Contexts.sharedInstance.game;
            m_SystemEntry.Add(new GameObjectSystem(m_ContextGame));
            m_SystemEntry.Initialize();
            await UniTask.CompletedTask;
        }

        public override void Update()
        {
            m_SystemEntry.Execute();
        }

        public override void LateUpdate()
        {
            m_SystemEntry.Cleanup();
        }

        public override void FixedUpdate()
        {
        }

        public override void ShutDown()
        {
            m_SystemEntry.TearDown();
        }

        private Feature m_SystemEntry = new ();
        private GameContext m_ContextGame;
    }
}
