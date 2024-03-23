using Game.GlobalDefinition;
using Game.Module;

namespace Game.Logic
{
    public class StateFall: CharacterStateBase
    {
        public override void OnEnter(ECharacterState fromState)
        {
            Host.GetComponent<FallControllerComponent>().Start(fromState);
        }

        public override void OnExit(ECharacterState toState)
        {
            Host.GetComponent<FallControllerComponent>().Stop(toState);
        }

        public override bool CanEnterFrom(ECharacterState fromState)
        {
            switch (fromState)
            {
                case ECharacterState.Jump:
                    return true;
            }
            return false;
        }

        public override bool AutoExit()
        {
            var modelAttr = Host.Parent.GetChild<CharacterModelEcEntity>().GetComponent<AttrComponent>();
            var isOnGround = modelAttr.GetAttr(EModelAttr.IsOnGround.ToUint(), false);
            return isOnGround;
        }

        public override ECharacterState GetAutoExitState()
        {
            return ECharacterState.Default;
        }
    }
}