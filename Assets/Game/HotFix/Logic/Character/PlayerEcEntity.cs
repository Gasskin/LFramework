using Game.HotFix.GameDrivers;
using UnityGameFramework.Runtime;

namespace Game.HotFix.Logic
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