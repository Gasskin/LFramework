using Game.Module.Entity;

namespace Game.Logic.PlayerController
{
    public interface IPlayerControllerComponent
    {
        public void Enter(EPlayerState fromState);
        public void Exit(EPlayerState toState);
    }
}