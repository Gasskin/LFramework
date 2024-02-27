using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;

namespace Game.Logic
{
    public class ParabolaMove : MoveModuleBase
    {
        public override void Move()
        {
            var modelAttr = Host.GetComponent<AttrComponent>();
            var controllerAttr = Host.Parent.GetChild<CharacterControllerEntity>().GetComponent<AttrComponent>();
            var delta = GameComponent.GameUpdater.DeltaTime;
            var velocityX = controllerAttr.GetAttr<float>(EAttrType.MoveHorizontalVelocity.ToUint());
            var velocityY = controllerAttr.GetAttr<float>(EAttrType.MoveVerticalVelocity.ToUint());
            
            var pos = modelAttr.GetAttr<Vector3>(EModelAttr.Position.ToUint());
            pos.x += velocityX * delta;
            pos.y += velocityY * delta;
            modelAttr.SetAttr(EModelAttr.Position.ToUint(), pos);

            var moveDir = controllerAttr.GetAttr<float>(EAttrType.MoveDir.ToUint());
            var rotation = moveDir < 0f ? new Vector3(0, 270, 0) : new Vector3(0, 90, 0);
            modelAttr.SetAttr(EModelAttr.Rotation.ToUint(), rotation);
        }
    }
}