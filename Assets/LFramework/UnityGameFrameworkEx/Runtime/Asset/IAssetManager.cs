namespace UnityGameFramework.Runtime
{
    public interface IAssetManager
    {
        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        public string ReadOnlyPath { get; set; }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        public string ReadWritePath { get; set; }
    }
}