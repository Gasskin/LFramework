using Game.GlobalDefinition;

namespace Game.Logic
{
    public interface ICharacterControllerComponent
    {
        public void Enter(ECharacterState fromState);
        public void Exit(ECharacterState toState);
    }
}