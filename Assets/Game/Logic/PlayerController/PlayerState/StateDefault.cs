namespace Game.Logic.PlayerController
{
    /// <summary>
    /// 待机状态，普通移动状态
    /// </summary>
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
            Host.GetComponent<DefaultComponent>();
        }

        public override void OnExit(EPlayerState toState)
        {
        }
    }
}