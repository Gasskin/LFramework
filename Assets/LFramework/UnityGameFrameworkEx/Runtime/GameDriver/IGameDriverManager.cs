
using Cysharp.Threading.Tasks;

namespace GameFramework.GameUpdater
{
    public interface IGameDriverManager
    {
        public UniTask InitAsync();

        public T GetModule<T>() where T : GameDriverBase;
        
        public void Update(float delta);

        public void LateUpdate();

        public void FixedUpdate();
    }
}
