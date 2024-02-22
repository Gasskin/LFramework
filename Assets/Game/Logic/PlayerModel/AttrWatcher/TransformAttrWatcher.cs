using Game.Module.Entity;
using UnityEngine;

namespace Game.Logic.PlayerModel
{
    public class TransformAttrWatcher : AttrWatcher
    {
        public override uint[] WatchAttrIndex { get; } =
        {
            (uint)EModelAttr.Position,
            (uint)EModelAttr.Rotation,
        };

        public override void OnCreate()
        {
            if (Host is ModelEntity model)
            {
                model.Model.transform.position = Vector3.zero;
                model.Model.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
        }

        public override void OnAttrChanged()
        {
            if (Host is not ModelEntity model)
                return;
            var attr = Host.GetComponent<AttrComponent>();
            var position = attr.GetAttr<Vector3>((uint)EModelAttr.Position);
            var rotation = attr.GetAttr<Vector3>((uint)EModelAttr.Rotation);
            model.Model.transform.position = position;
            model.Model.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}