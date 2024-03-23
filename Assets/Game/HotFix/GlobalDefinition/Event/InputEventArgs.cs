using GameFramework;
using GameFramework.Event;

namespace Game.HotFix.GlobalDefinition
{
    public class InputEventArgs:GameEventArgs
    {
    #region Override
        public override int Id => Const.EventId.Input;

        public override void Clear()
        {
            InputType = EInputType.None;
        }
    #endregion

        public EInputType InputType { get; private set; }
        public float m_MoveDir;
        public bool m_JumpPress;
        
        public static InputEventArgs Create(EInputType type)
        {
            var args = ReferencePool.Acquire<InputEventArgs>();
            args.InputType = type;
            return args;
        }
    }
}