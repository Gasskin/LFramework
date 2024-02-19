using Entitas;
using UnityGameFramework.Runtime;

namespace Game.Entitas
{
    [Game]
    public class ViewComponent : IComponent
    {
        public string m_AssetPath;
        public GameObjectInstance m_View;
    }
}