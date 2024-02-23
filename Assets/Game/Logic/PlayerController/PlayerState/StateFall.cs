namespace Game.Logic.PlayerController
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