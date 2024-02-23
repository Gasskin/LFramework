using Game.Module.Entity;

namespace Game.Logic.PlayerController
{
    public class JumpComponent: EntityComponent,IPlayerControllerComponent
    {
        public override bool DefaultEnable => false;

        public void Enter(EPlayerState fromState)
        {
            Enable = true;
        }

        public void Exit(EPlayerState toState)
        {
            Enable = false;
        }
    }
}