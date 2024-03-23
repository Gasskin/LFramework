using Game.Module;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class PlayerEcEntity: ECEntity
    {
        public override void Awake()
        {
            AddChild<CharacterModelECEntity>(ResourcesPathConfig.ModelPrefabs.RPG_Player);
            AddChild<CharacterControllerEcEntity>();
        }
    }
}