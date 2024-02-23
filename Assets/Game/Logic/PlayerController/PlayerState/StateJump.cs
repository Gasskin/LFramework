namespace Game.Logic.PlayerController
{
    public class StateJump : PlayerStateBase
    {
        public override void OnEnter(EPlayerState fromState)
        {
            Host.GetComponent<JumpComponent>().Enter(fromState);
        }

        public override void OnExit(EPlayerState toState)
        {
            Host.GetComponent<JumpComponent>().Exit(toState);
        }

        public override bool CanEnterFrom(EPlayerState fromState)
        {
            switch (fromState)
            {
                case EPlayerState.None:
                case EPlayerState.Default:
                    return true;
            }
            return false;
        }
    }
}