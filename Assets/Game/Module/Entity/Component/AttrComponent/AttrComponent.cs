using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Game.Module.Entity
{
    [DrawEntityProperty]
    public class AttrComponent : EntityComponent
    {
        private Dictionary<uint, Variable> m_AttrDic = new();
        private Dictionary<uint, List<AttrWatcher>> m_AttrWatcherDic = new();
        private Dictionary<Type, AttrWatcher> m_Type2AttrWatcher = new();

        public void SetAttr<T>(uint index, T value)
        {
            if (!m_AttrDic.TryGetValue(index, out var attr))
            {
                var attrType = (EAttrType)index;
                attr = CreateAttr(attrType);
                if (attr == null)
                    return;
                m_AttrDic.Add(index, attr);
            }
            if (attr != null && !Equals(attr.GetValue(), value))
            {
                attr.SetValue(value);
                if (m_AttrWatcherDic.TryGetValue(index, out var watchers))
                {
                    foreach (var watcher in watchers)
                    {
                        watcher.MarkChange(true);
                    }
                }
            }
        }

        public T GetAttr<T>(uint index)
        {
            if (m_AttrDic.TryGetValue(index, out var attr))
            {
                return (T)attr.GetValue();
            }
            return default;
        }

        public void AddAttrWatcher<T>() where T : AttrWatcher
        {
            var type = typeof(T);
            if (m_Type2AttrWatcher.ContainsKey(type))
            {
                Log.Error($"duplicate attr watcher: {type}");
                return;
            }
            var watcher = Activator.CreateInstance<T>();
            watcher.SetHost(Entity);
            for (int i = 0; i < watcher.WatchAttrIndex.Length; i++)
            {
                var index = watcher.WatchAttrIndex[i];
                if (!m_AttrWatcherDic.TryGetValue(index, out var watchers))
                {
                    watchers = new List<AttrWatcher>();
                    m_AttrWatcherDic.Add(index, watchers);
                }
                watchers.Add(watcher);
            }
        }

        public override void LateUpdate()
        {
            foreach (var watchers in m_AttrWatcherDic)
            {
                foreach (var watcher in watchers.Value)
                {
                    if (watcher.Changed)
                    {
                        watcher.MarkChange(false);
                        watcher.OnAttrChanged();
                    }
                }
            }
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            var str = "【Attr】";
            foreach (var attr in m_AttrDic)
            {
                str += "\n" + (EAttrType)attr.Key + " : " + attr.Value;
            }
            str += "\n\n【AttrWatcher】";
            foreach (var watchers in m_AttrWatcherDic)
            {
                str += "\n---------------------------------";
                str += "\n[" + (EAttrType)watchers.Key + "]";
                foreach (var watcher in watchers.Value)
                {
                    str += "\n" + watcher;
                }
            }
            return str;
        }
#endif

        private Variable CreateAttr(EAttrType attrType)
        {
            switch (attrType)
            {
                case EAttrType.MoveDir:
                    return ReferencePool.Acquire<VarInt32>();
            }
            return null;
        }
    }
}