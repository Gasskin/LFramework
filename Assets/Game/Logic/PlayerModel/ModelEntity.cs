using Game.Module;
using UnityEngine;
using UnityGameFramework.Runtime;
using Entity = Game.Module.Entity;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class ModelEntity: Entity
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

            AddComponent<MoveComponent>();
            AddComponent<AttrComponent>();

            var attr = GetComponent<AttrComponent>();
            attr.AddAttrWatcher<TransformAttrWatcher>();
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