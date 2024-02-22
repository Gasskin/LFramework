using System.Collections.Generic;
using Game.Logic.Utility;
using Game.Module.Entity;
using Game.Module.Input;

namespace Game.Logic.PlayerController
{
    public class PrepareMoveComponent : EntityComponent, IPlayerControllerComponent
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

        public override void Update()
        {
            var attrComp = Entity.GetComponent<AttrComponent>();
            
            var dir = GameModule.Input.MoveDir;
            if (dir == EMoveDir.None)
            {
                attrComp.SetAttr((uint)EAttrType.MoveMode, EMoveMode.None);
                return;
            }

            var velocity = 6f;
            var moveDir = dir == EMoveDir.Left ? -1f : 1f;

            attrComp.SetAttr((uint)EAttrType.MoveMode, (int)EMoveMode.SpeedMove);
            attrComp.SetAttr((uint)EAttrType.MoveDir, moveDir);
            attrComp.SetAttr((uint)EAttrType.MoveVelocity, velocity);
        }
    }
}