using System.Diagnostics;

namespace GameFramework.Asset
{
    public class LoadWatcher : Stopwatch, IReference
    {
        public void Clear()
        {
            Reset();
        }
    }
}