using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class StateJump : CharacterStateBase
    {
        public override void OnEnter(ECharacterState fromState)
        {
            Host.GetComponent<JumpControllerComponent>().Enter(fromState);
        }

        public override void OnExit(ECharacterState toState)
        {
            Host.GetComponent<JumpControllerComponent>().Exit(toState);
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
            // var modelAttr = Host.Parent.GetChild<CharacterModelEntity>().GetComponent<AttrComponent>();
            // if (modelAttr.GetAttr<bool>(EModelAttr.IsOnGround.ToUint()))
            // {
            //     return true;
            // }
            return false;
        }

        public override ECharacterState GetAutoExitState()
        {
            return ECharacterState.Default;
        }
    }
}