using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class StateJump : CharacterStateBase
    {
        public override void OnEnter(ECharacterState fromState)
        {
            Host.GetComponent<JumpControllerComponent>().Start(fromState);
        }

        public override void OnExit(ECharacterState toState)
        {
            Host.GetComponent<JumpControllerComponent>().Stop(toState);
        }

        public override bool CanEnterFrom(ECharacterState fromState)
        {
            switch (fromState)
            {
                case ECharacterState.None:
                case ECharacterState.Default:
                    return true;
            }
            return false;
        }

        public override bool AutoExit()
        {
            return false;
        }

        public override ECharacterState GetAutoExitState()
        {
            return ECharacterState.Fall;
        }
    }
}