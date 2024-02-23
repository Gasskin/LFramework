using Game.GlobalDefinition;

namespace Game.Logic
{
    public class StateFall: PlayerStateBase
    {
        public override void OnEnter(EPlayerState fromState)
        {
        }

        public override void OnExit(EPlayerState toState)
        {
        }

        public override bool CanEnterFrom(EPlayerState fromState)
        {
            return false;
        }
    }
}