using Entitas;
using Game.InputModule;
using Game.Utility;

namespace Game.Entitas
{
    public class CtrlMoveSystem: IExecuteSystem,IInitializeSystem
    {
        private GameContext m_Context;
        private InputModule.InputModule m_InputModule;
        private IGroup<GameEntity> m_Group;
        
        public void Initialize()
        {
            m_Context = Contexts.sharedInstance.game;
            m_InputModule = GameModule.Input;
            m_Group = m_Context.GetGroup(GameMatcher.EntityType);
        }
        
        public void Execute()
        {
            // 只会有一个PlayerEntity
            foreach (var e in m_Group.GetEntities())
            {
                if (e.entityType.m_EntityType == EEntityType.Player)
                {
                    CtrlMove(e);
                    break;
                }
            }
        }

        private void CtrlMove(GameEntity player)
        {
            var isMoving = m_InputModule.MoveDir != EMoveDir.None;
            if (isMoving)
            {
                var moveSpeed = 1f;
                player.ReplaceMoveDir(m_InputModule.MoveDir);
                player.ReplaceMoveSpeed(moveSpeed);
            }
            else
            {
                if (player.hasMoveDir)
                {
                    player.RemoveMoveDir();
                }
                if (player.hasMoveSpeed)
                {
                    player.RemoveMoveSpeed();
                }
            }
        }
    }
}