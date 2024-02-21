using Game.Logic.Utility;
using Game.Module.Entity;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Logic
{
    [DrawEntityProperty]
    public class ModelEntity: Module.Entity.Entity
    {
        private GameObject model;
        
        public override async void Awake(object initData)
        {
            if (initData is not string path)
            {
                return;
            }

            var asset = await GameComponent.Resource.LoadAssetAsync<GameObject>(path);
            if (asset != null)
            {
                model = Object.Instantiate(asset);
                GameComponent.Resource.UnloadAsset(asset);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override string ToString()
        {
            return $"Model: {model.name}";
        }
    }
}