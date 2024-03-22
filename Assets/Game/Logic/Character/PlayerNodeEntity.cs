using Game.Module;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class PlayerNodeEntity: NodeEntity
    {
        public override void Awake()
        {
            AddChild<CharacterModelNodeEntity>(ResourcesPathConfig.ModelPrefabs.RPG_Player);
            AddChild<CharacterControllerNodeEntity>();
        }
    }
}