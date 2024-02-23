using Game.GlobalDefinition;

namespace Game.Logic
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
                case EPlayerState.Jump:
                    return false;
                case EPlayerState.Fall:
                    return false;
            }
            return false;
        }

        public override void OnEnter(EPlayerState fromState)
        {
            Host.GetComponent<DefaultComponent>().Enter(fromState);
        }

        public override void OnExit(EPlayerState toState)
        {
            Host.GetComponent<DefaultComponent>().Exit(toState);
        }
    }
}