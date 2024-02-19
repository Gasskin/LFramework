using Entitas;
using Game.InputModule;
using Game.Utility;
using UnityEngine;

namespace Game.Entitas
{
    public class MoveSystem: IExecuteSystem,IInitializeSystem
    {
        private GameContext m_Context;
        private IGroup<GameEntity> m_Group;
        
        public void Initialize()
        {
            m_Context = Contexts.sharedInstance.game;
            m_Group = m_Context.GetGroup(GameMatcher.AllOf(GameMatcher.MoveDir, GameMatcher.MoveSpeed, GameMatcher.View));
        }
        
        public void Execute()
        {
            foreach (var e in m_Group.GetEntities())
            {
                var delta = GameComponent.GameUpdater.DeltaTime;
                var dir = e.moveDir.m_MoveDir == EMoveDir.Left ? 1 : -1;
                var motion = e.moveSpeed.m_MoveSpeed * dir * delta;

                var go = e.view.m_View.Target as GameObject;
                go.transform.SetPositionX(go.transform.position.x + motion);
            }
        }
    }
}