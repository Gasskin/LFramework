using System;
using System.Linq;

namespace Game.HotFix.GameDrivers
{
    public static class IdFactory
    {
        private static long s_BaseRevertTicks;

        public static long NewInstanceId()
        {
            if (s_BaseRevertTicks == 0)
            {
                var now = DateTime.UtcNow.Ticks;
                var str = now.ToString().Reverse();
                s_BaseRevertTicks = long.Parse(string.Concat(str));
            }
            s_BaseRevertTicks++;
            return s_BaseRevertTicks;
        }
    }
}