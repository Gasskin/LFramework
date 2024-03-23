using Cysharp.Threading.Tasks;

namespace GameFramework.GameUpdater
{
    public abstract class GameDriverBase
    {
        public abstract int Priority { get; }

        public abstract UniTask InitAsync();

        public abstract void Update(float delta);

        public abstract void LateUpdate();

        public abstract void FixedUpdate();

        public abstract void ShutDown();
    }
}
