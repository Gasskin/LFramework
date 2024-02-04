using UnityGameFramework.Runtime;

namespace LFramework
{
    public class LFrameworkComponent: GameFrameworkComponent
    {
        private void Update()
        {
            LFrameworkEntry.Update();
        }

        private void LateUpdate()
        {
            LFrameworkEntry.LateUpdate();
        }

        private void FixedUpdate()
        {
            LFrameworkEntry.FixedUpdate();
        }

        private void OnDestroy()
        {
            LFrameworkEntry.Shutdown();
        }
    }
}