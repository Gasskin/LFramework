using UnityEngine;

namespace Game.Module.Entity
{
    public class GameObjectComponent : EntityComponent
    {
        public GameObject GameObject { get;private set; }

        public override void Awake()
        {
            GameObject = new GameObject(Entity.GetType().Name);
            GameObject.AddComponent<ComponentView>();
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            Object.Destroy(GameObject);
        }
        
        public void OnNameChanged(string name)
        {
            GameObject.name = $"{Entity.GetType().Name}: {name}";
        }

        public void OnAddComponent(EntityComponent entityComponent)
        {
            var view = GameObject.GetComponent<ComponentView>();
            view.m_Components.Add(entityComponent);
        }

        public void OnRemoveComponent(EntityComponent entityComponent)
        {
            var view = GameObject.GetComponent<ComponentView>();
            view.m_Components.Remove(entityComponent);
        }

        public void OnAddChild(Entity child)
        {
            if (child.GetComponent<GameObjectComponent>() != null)
            {
                child.GetComponent<GameObjectComponent>().GameObject.transform.SetParent(GameObject.transform);
            }
        }

        public override string ToString()
        {
            return GameObject.name;
        }
    }
}