using Game.Module;
using UnityGameFramework.Runtime;
using Entity = Game.Module.Entity;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class PlayerEntity: Entity
    {
        public override void Awake()
        {
            AddChild<ModelEntity>(ResourcesPathConfig.ModelPrefabs.RPG_Character);
            AddChild<PlayerControllerEntity>();
        }
    }
}