using Game.GlobalDefinition;

namespace Game.Logic
{
    public class StateFall: CharacterStateBase
    {
        public override void OnEnter(ECharacterState fromState)
        {
        }

        public override void OnExit(ECharacterState toState)
        {
        }

        public override bool CanEnterFrom(ECharacterState fromState)
        {
            return false;
        }
    }
}