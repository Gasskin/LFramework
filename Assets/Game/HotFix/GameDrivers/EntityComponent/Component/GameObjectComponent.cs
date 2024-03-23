using UnityEngine;

namespace Game.HotFix.GameDrivers
{
#if UNITY_EDITOR
    public class GameObjectComponent : ECComponent
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

        public void OnAddComponent(ECComponent ecComponent)
        {
            var view = GameObject.GetComponent<ComponentView>();
            view.m_Components.Add(ecComponent);
        }

        public void OnRemoveComponent(ECComponent ecComponent)
        {
            var view = GameObject.GetComponent<ComponentView>();
            view.m_Components.Remove(ecComponent);
        }

        public void OnAddChild(ECEntity child)
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
#endif
}