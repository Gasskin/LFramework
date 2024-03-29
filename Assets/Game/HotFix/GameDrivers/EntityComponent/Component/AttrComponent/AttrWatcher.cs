﻿namespace Game.HotFix.GameDrivers
{
    public abstract class AttrWatcher
    {
        public bool Changed { get; private set; }
        public abstract uint[] WatchAttrIndex { get; }
        public ECEntity Host { get; private set; }

        public abstract void OnCreate();
        public abstract void OnAttrChanged();

        public void MarkChange(bool change)
        {
            Changed = change;
        }
        
        public void SetHost(ECEntity host)
        {
            Host = host;
        }
    }
}