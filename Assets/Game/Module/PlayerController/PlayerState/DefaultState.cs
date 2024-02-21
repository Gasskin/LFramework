namespace Game.Module.PlayerController
{
    public class DefaultState : PlayerStateBase
    {
        public override bool CanEnterFrom(EPlayerState fromState)
        {
            switch (fromState)
            {
                case EPlayerState.StateSkill:
                    return false;
            }
            return false;
        }

        public override void OnEnter(EPlayerState fromState)
        {
            var move = Host.GetComponent<MoveComponent>();
            if (move != null)
            {
                move.Enable = true;
            }
        }

        public override void OnExit(EPlayerState toState)
        {
            base.OnExit(toState);
        }
    }
}