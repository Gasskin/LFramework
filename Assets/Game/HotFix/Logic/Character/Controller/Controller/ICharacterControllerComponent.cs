using Game.HotFix.GlobalDefinition;

namespace Game.HotFix.Logic
{
    public interface ICharacterControllerComponent
    {
        public void Start(ECharacterState fromState);
        public void Stop(ECharacterState toState);
    }
}