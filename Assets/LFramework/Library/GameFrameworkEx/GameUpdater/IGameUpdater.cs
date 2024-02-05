
using Cysharp.Threading.Tasks;

namespace GameFramework.GameUpdater
{
    public interface IGameUpdater
    {
        public UniTask InitAsync();

        public T GetModule<T>() where T : GameModuleBase;
        
        public void Update();

        public void LateUpdate();

        public void FixedUpdate();
    }
}
