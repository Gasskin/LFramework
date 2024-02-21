using Game.Module.Entity;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    public class MoveDirAttrWatcher: AttrWatcher
    {
        public override uint[] WatchAttrIndex { get; } =
        {
            (uint)EAttrType.MoveDir
        };
        
        public override void OnAttrChanged()
        {
            var comp = Host.GetComponent<AttrComponent>();
            var moveDir = comp.GetAttr<int>((uint)EAttrType.MoveDir);
            Log.Error($"OnAttrChanged: {moveDir}");
        }
    }
}