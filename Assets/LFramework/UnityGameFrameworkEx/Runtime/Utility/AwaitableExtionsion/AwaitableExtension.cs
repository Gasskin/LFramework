﻿namespace UnityGameFramework.Runtime
{
    public static partial class AwaitableExtension
    {
        static AwaitableExtension()
        {
            RegisterSceneEvent();
            RegisterOpenUIEvent();
        }
    }
}