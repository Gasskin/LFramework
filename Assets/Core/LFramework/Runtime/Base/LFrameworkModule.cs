using Cysharp.Threading.Tasks;

namespace LFramework
{
    /// <summary>
    /// 业务层模块的抽象
    /// </summary>
    public abstract class LFrameworkModule
    {
        internal abstract int Priority { get; }
        
        internal abstract UniTask InitAsync();
        
        internal abstract void Update();

        internal abstract void LateUpdate();
        
        internal abstract void FixedUpdate();

        internal abstract void ShutDown();
    }
}