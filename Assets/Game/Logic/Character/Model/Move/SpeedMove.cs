using Game.GlobalDefinition;
using MenteBacata.ScivoloCharacterController;

namespace Game.Logic
{
    public class SpeedMove : MoveModuleBase
    {
        private float m_DeltaTime;
        private float m_MoveDir;
        private float m_MoveSpeed;
        
        public override void Move()
        {
            PrepareAttr();
            var dir = (Forward * m_MoveDir).normalized;
            var velocity = dir * m_MoveSpeed;
            // velocity.y = 0;
            // velocity.z = 0;
            TryResolveOverlap();
            SetMoverMode(CharacterMover.Mode.Walk);
            DoMove(velocity * m_DeltaTime);
        }

        private void PrepareAttr()
        {
            m_DeltaTime = GameComponent.GameUpdater.DeltaTime;
            m_MoveDir = ControllerAttr.GetAttr<float>(EControllerAttr.MoveDir.ToUint());
            m_MoveSpeed = ControllerAttr.GetAttr<float>(EControllerAttr.MoveSpeed.ToUint());
        }
    }
}