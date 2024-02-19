using Cysharp.Threading.Tasks;
using GameFramework.GameUpdater;

namespace Game.Entitas
{
    public class EntitasModule: GameModuleBase
    {
    #region Override
        public override int Priority => (int)EModulePriority.None;
        public override async UniTask InitAsync()
        {
            m_GameContext = Contexts.sharedInstance.game;
            m_SystemEntry.Add(new ViewSystem(m_GameContext));
            m_SystemEntry.Add(new CtrlMoveSystem());
            m_SystemEntry.Add(new MoveSystem());
            
            m_SystemEntry.Initialize();
            
            // TODO
            PlayerEntity = m_GameContext.CreateEntity();
            PlayerEntity.AddEntityType(EEntityType.Player);
            
            await UniTask.CompletedTask;
        }

        public override void Update(float delta)
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
    #endregion

        public GameEntity PlayerEntity { get; private set; }
        
        private readonly Feature m_SystemEntry = new ();
        private GameContext m_GameContext;
    }
}
