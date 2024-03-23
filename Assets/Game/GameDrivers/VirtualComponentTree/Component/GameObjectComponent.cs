using UnityEngine;

namespace Game.Module
{
#if UNITY_EDITOR
    public class GameObjectComponent : VComponent
    {
        public GameObject GameObject { get;private set; }

        public override void Awake()
        {
            GameObject = new GameObject(VGameObject.GetType().Name);
            GameObject.AddComponent<ComponentView>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Object.Destroy(GameObject);
        }
        
        public void OnNameChanged(string name)
        {
            GameObject.name = $"{VGameObject.GetType().Name}: {name}";
        }

        public void OnAddComponent(VComponent vComponent)
        {
            var view = GameObject.GetComponent<ComponentView>();
            view.m_Components.Add(vComponent);
        }

        public void OnRemoveComponent(VComponent vComponent)
        {
            var view = GameObject.GetComponent<ComponentView>();
            view.m_Components.Remove(vComponent);
        }

        public void OnAddChild(VGameObject child)
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