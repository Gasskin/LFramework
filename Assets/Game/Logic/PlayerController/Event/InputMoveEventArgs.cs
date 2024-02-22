using Game.GlobalDefinition;
using Game.Module.Input;
using GameFramework;
using GameFramework.Event;

namespace Game.Logic.PlayerController
{
    public class InputMoveEventArgs:GameEventArgs
    {
    #region Override
        public override int Id => Const.EventId.InputMove;

        public override void Clear()
        {
            
        }
    #endregion

        public EMoveDir MoveDir { get; private set; }
        
        public static InputMoveEventArgs Create(EMoveDir dir)
        {
            var args = ReferencePool.Acquire<InputMoveEventArgs>();
            args.MoveDir = dir;
            return args;
        }
    }
}