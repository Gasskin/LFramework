using GameFramework;
using YooAsset;

namespace UnityGameFramework.Runtime
{
    public class VarAsyncOperationBase: Variable<AsyncOperationBase>
    {
        /// <summary>
        /// 初始化 UnityEngine.GameObject 变量类的新实例。
        /// </summary>
        public VarAsyncOperationBase()
        {
        }

        /// <summary>
        /// 从 AsyncOperationBase 到 VarAsyncOperationBase 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarAsyncOperationBase(AsyncOperationBase value)
        {
            VarAsyncOperationBase varValue = ReferencePool.Acquire<VarAsyncOperationBase>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 VarAsyncOperationBase 变量类到 AsyncOperationBase 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator AsyncOperationBase(VarAsyncOperationBase value)
        {
            return value.Value;
        }
    }
}