namespace Game.Module
{
    public abstract class AttrWatcher
    {
        public bool Changed { get; private set; }
        public abstract uint[] WatchAttrIndex { get; }
        public VGameObject Host { get; private set; }

        public abstract void OnCreate();
        public abstract void OnAttrChanged();

        public void MarkChange(bool change)
        {
            Changed = change;
        }
        
        public void SetHost(VGameObject host)
        {
            Host = host;
        }
    }
}