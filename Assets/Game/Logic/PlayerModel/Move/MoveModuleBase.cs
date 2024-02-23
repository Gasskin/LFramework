using Game.Module;

namespace Game.Logic
{
    public abstract class MoveModuleBase
    {
        public Entity Host { get; private set; }

        public abstract void Move();
        
        public void SetHost(Entity host)
        {
            Host = host;
        }
    }
}