using Cysharp.Threading.Tasks;

namespace LFramework
{
    /// <summary>
    /// 业务层模块的抽象
    /// </summary>
    public abstract class LFrameworkModule
    {
        public abstract int Priority { get; }

        public abstract UniTask InitAsync();

        public abstract void Update();

        public abstract void LateUpdate();

        public abstract void FixedUpdate();

        public abstract void ShutDown();
    }
}