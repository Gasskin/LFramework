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
            AddChild<CharacterModelEntity>(ResourcesPathConfig.ModelPrefabs.RPG_Player);
            AddChild<CharacterControllerEntity>();
        }
    }
}