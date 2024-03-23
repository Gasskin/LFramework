using GameFramework;

namespace UnityGameFramework.Runtime
{
    public class AssetManager: GameFrameworkModule,IAssetManager
    {
        private string m_ReadOnlyPath;
        public string ReadOnlyPath
        {
            get => m_ReadOnlyPath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    GameFrameworkLog.Error("AssetManager非法的只读路径");
                    return;
                }
                m_ReadOnlyPath = value;
            }
        }

        private string m_ReadWritePath;
        public string ReadWritePath
        {
            get => m_ReadWritePath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    GameFrameworkLog.Error("AssetManager非法的读写路径");
                    return;
                }
                m_ReadWritePath = value;
            }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        internal override void Shutdown()
        {
        }

    }
}