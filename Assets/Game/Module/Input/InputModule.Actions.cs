namespace Game.InputModule
{
    public partial class InputModule
    {
        public void Move(EMoveDir dir)
        {
            if (MoveDir == dir) 
            {
                return;
            }
            MoveDir = dir;
        }
    }
}