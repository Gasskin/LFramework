using Game.HotFix.GameDrivers;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.HotFix.Logic
{
    [DrawEntityProperty]
    public class CharacterModelECEntity: ECEntity
    {
        public GameObject Model { get; private set; }

        public override async void Awake(object initData)
        {
            if (initData is not string path)
            {
                return;
            }

            var asset = await GameComponent.Resource.LoadAssetAsync<GameObject>(path);
            if (asset != null)
            {
                Model = Object.Instantiate(asset);
                GameComponent.Resource.UnloadAsset(asset);
            }
            
            AddComponent<AttrComponent>();
            AddComponent<MoveComponent>();
            AddComponent<GroundCheckComponent>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override string ToString()
        {
            return $"Model: {Model.name}";
        }
    }
}