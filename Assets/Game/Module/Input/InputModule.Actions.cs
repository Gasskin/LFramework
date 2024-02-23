using Game.Logic.Utility;

namespace Game.Module.Input
{
    public partial class InputModule
    {
        public void Move(EMoveDir dir)
        {
            if (MoveDir == dir) 
                return;
            MoveDir = dir;

            var arg = InputEventArgs.Create(EInputType.Move);
            arg.m_MoveDir = MoveDir;
            GameComponent.Event.Fire(this, arg);
        }

        public void Jump(bool press)
        {
            var arg = InputEventArgs.Create(EInputType.Jump);
            arg.m_JumpPress = press;
            GameComponent.Event.Fire(this, arg);
        }
    }
}