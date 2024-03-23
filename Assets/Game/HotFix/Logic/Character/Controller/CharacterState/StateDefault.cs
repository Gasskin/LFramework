using Game.HotFix.GlobalDefinition;

namespace Game.HotFix.Logic
{
    /// <summary>
    /// 待机状态，普通移动状态
    /// </summary>
    public class StateDefault : CharacterStateBase
    {
        public override bool CanEnterFrom(ECharacterState fromState)
        {
            switch (fromState)
            {
                case ECharacterState.Default:
                case ECharacterState.Jump:
                case ECharacterState.Fall:
                    return false;
            }
            return true;
        }

        public override void OnEnter(ECharacterState fromState)
        {
            Host.GetComponent<DefaultControllerComponent>().Start(fromState);
        }

        public override void OnExit(ECharacterState toState)
        {
            Host.GetComponent<DefaultControllerComponent>().Stop(toState);
        }
    }
}