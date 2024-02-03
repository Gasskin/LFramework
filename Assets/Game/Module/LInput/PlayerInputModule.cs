using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LFramework
{
    public class PlayerInputModule : LFrameworkModule
    {
        internal override int Priority => (int)EModulePriority.None;

        internal override async UniTask InitAsync()
        {
            await UniTask.CompletedTask;
        }

        internal override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Log.Error(999);
            }
        }

        internal override void LateUpdate()
        {
        }

        internal override void FixedUpdate()
        {
        }

        internal override void ShutDown()
        {
        }
    }
}