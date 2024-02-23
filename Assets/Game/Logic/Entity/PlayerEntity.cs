using Game.Logic.PlayerController;
using Game.Module.Entity;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class PlayerEntity: Module.Entity.Entity
    {
        public override void Awake()
        {
            AddChild<ModelEntity>(ResourcesPathConfig.ModelPrefabs.RPG_Character);
            AddChild<PlayerControllerEntity>();
        }
    }
}