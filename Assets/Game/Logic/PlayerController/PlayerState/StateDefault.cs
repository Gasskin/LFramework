namespace Game.Logic.PlayerController
{
    public class StateDefault : PlayerStateBase
    {
        public override bool CanEnterFrom(EPlayerState fromState)
        {
            switch (fromState)
            {
                case EPlayerState.Skill:
                    return false;
            }
            return true;
        }

        public override void OnEnter(EPlayerState fromState)
        {
            var prepareMove = Host.GetComponent<PrepareMoveComponent>();
            if (prepareMove != null)
                prepareMove.Enter(fromState);

            var move = Host.GetComponent<MoveComponent>();
            if (move != null)
                move.Enter(fromState);
        }

        public override void OnExit(EPlayerState toState)
        {
            var prepareMove = Host.GetComponent<PrepareMoveComponent>();
            if (prepareMove != null)
                prepareMove.Exit(toState);
            
            var move = Host.GetComponent<MoveComponent>();
            if (move != null)
                move.Exit(toState);
        }
    }
}