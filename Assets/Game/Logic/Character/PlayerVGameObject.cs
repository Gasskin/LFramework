using Game.Module;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class PlayerVGameObject: VGameObject
    {
        public override void Awake()
        {
            AddChild<CharacterModelVGameObject>(ResourcesPathConfig.ModelPrefabs.RPG_Player);
            AddChild<CharacterControllerVGameObject>();
        }
    }
}