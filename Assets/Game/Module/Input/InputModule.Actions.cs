namespace Game.Module.Input
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