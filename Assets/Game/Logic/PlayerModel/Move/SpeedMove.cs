using Game.GlobalDefinition;
using Game.Logic;
using Game.Module;
using UnityEngine;

namespace Game.Logic
{
    public class SpeedMove : MoveModuleBase
    {
        public override void Move()
        {
            var attr = Host.GetComponent<AttrComponent>();
            if (attr == null)
                return;

            var delta = GameComponent.GameUpdater.DeltaTime;
            var moveDir = attr.GetAttr<float>((uint)EAttrType.MoveDir);
            var velocity = attr.GetAttr<float>((uint)EAttrType.MoveVelocity);
            var motion = moveDir * velocity * delta;

            if (motion == 0f)
                return;

            var modelEntity = Host.Parent.GetChild<ModelEntity>();
            if (modelEntity == null)
                return;

            var modelAttr = modelEntity.GetComponent<AttrComponent>();
            var pos = modelAttr.GetAttr<Vector3>((uint)EModelAttr.Position);
            pos.x += motion;
            modelAttr.SetAttr((uint)EModelAttr.Position, pos);

            if (moveDir != 0f)
            {
                var rotation = moveDir < 0f ? new Vector3(0, 270, 0) : new Vector3(0, 90, 0);
                modelAttr.SetAttr((uint)EModelAttr.Rotation, rotation);
            }
        }
    }
}