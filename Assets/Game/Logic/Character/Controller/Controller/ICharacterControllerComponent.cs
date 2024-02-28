using Game.GlobalDefinition;

namespace Game.Logic
{
    public interface ICharacterControllerComponent
    {
        public void Start(ECharacterState fromState);
        public void Stop(ECharacterState toState);
    }
}