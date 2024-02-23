using Game.GlobalDefinition;

namespace Game.Logic
{
    public interface IPlayerControllerComponent
    {
        public void Enter(EPlayerState fromState);
        public void Exit(EPlayerState toState);
    }
}