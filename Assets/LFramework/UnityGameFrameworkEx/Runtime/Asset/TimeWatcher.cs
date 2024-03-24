using System.Diagnostics;

namespace GameFramework.Asset
{
    public class TimeWatcher : Stopwatch, IReference
    {
        public void Clear()
        {
            Reset();
        }
    }
}