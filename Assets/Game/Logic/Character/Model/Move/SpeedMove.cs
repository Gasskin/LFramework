using Game.GlobalDefinition;
using Game.Module;
using UnityEngine;

namespace Game.Logic
{
    public class SpeedMove : MoveModuleBase
    {
        public override void Move()
        {
            var modelAttr = Host.GetComponent<AttrComponent>();
            var controllerAttr = Host.Parent.GetChild<CharacterControllerEntity>().GetComponent<AttrComponent>();

            var delta = GameComponent.GameUpdater.DeltaTime;
            var moveDir = controllerAttr.GetAttr<float>(EAttrType.MoveDir.ToUint());
            var velocity = controllerAttr.GetAttr<float>(EAttrType.MoveHorizontalVelocity.ToUint());
            var motion = moveDir * velocity * delta;

            if (motion == 0f)
                return;

            var pos = modelAttr.GetAttr<Vector3>(EModelAttr.Position.ToUint());
            pos.x += motion;
            modelAttr.SetAttr(EModelAttr.Position.ToUint(), pos);

            if (moveDir != 0f)
            {
                var rotation = moveDir < 0f ? new Vector3(0, 270, 0) : new Vector3(0, 90, 0);
                modelAttr.SetAttr(EModelAttr.Rotation.ToUint(), rotation);
            }
        }
    }
}