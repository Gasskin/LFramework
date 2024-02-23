using Game.Module.Entity;

namespace Game.Logic.PlayerController
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