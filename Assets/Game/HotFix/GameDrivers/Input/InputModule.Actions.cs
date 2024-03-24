using System;
using Game.HotFix.GlobalDefinition;
using Game.HotFix.Utility;

namespace Game.HotFix.GameDrivers
{
    public partial class InputDriver
    {
        public void Move(float dir)
        {
            if (Math.Abs(MoveDir - dir) < 0.001f) 
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