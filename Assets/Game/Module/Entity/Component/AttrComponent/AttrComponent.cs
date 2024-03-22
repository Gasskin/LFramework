using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Module
{
    [DrawEntityProperty]
    public class AttrComponent : EntityComponent
    {
        private readonly Dictionary<uint, Variable> m_AttrDic = new();
        private readonly Dictionary<uint, List<AttrWatcher>> m_AttrWatcherDic = new();
        private readonly Dictionary<Type, AttrWatcher> m_Type2AttrWatcher = new();

        public void SetAttr<T>(uint index, T value)
        {
            if (!m_AttrDic.TryGetValue(index, out var attr))
            {
                attr = CreateAttr<T>();
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

        public T GetAttr<T>(uint index, T defaultValue = default)
        {
            if (!m_AttrDic.TryGetValue(index, out var attr))
            {
                attr = CreateAttr<T>();
                attr.SetValue(defaultValue);
                m_AttrDic.Add(index, attr);
            }
            return (T)attr.GetValue();
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
            watcher.SetHost(NodeEntity);
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

            m_Type2AttrWatcher.Add(type, watcher);
            watcher.OnCreate();
        }

        public void RemoveAttrWatcher<T>() where T : AttrWatcher
        {
            var type = typeof(T);
            if (!m_Type2AttrWatcher.TryGetValue(type,out var watcher))
            {
                Log.Warning($"no attr watcher: {type}");
                return;
            }
            for (int i = 0; i < watcher.WatchAttrIndex.Length; i++)
            {
                var index = watcher.WatchAttrIndex[i];
                m_AttrWatcherDic.Remove(index, out _);
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
                str += "\n" + attr;
            }
            str += "\n\n【AttrWatcher】";
            foreach (var watchers in m_AttrWatcherDic)
            {
                str += "\n---------------------------------";
                str += "\n[" + watchers.Key + "]";
                foreach (var watcher in watchers.Value)
                {
                    str += "\n" + watcher;
                }
            }
            return str;
        }
#endif

        private Variable CreateAttr<T>()
        {
            var typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return ReferencePool.Acquire<VarBoolean>();
                case TypeCode.Int16:
                    return ReferencePool.Acquire<VarInt16>();
                case TypeCode.Int32:
                    return ReferencePool.Acquire<VarInt32>();
                case TypeCode.Int64:
                    return ReferencePool.Acquire<VarInt64>();
                case TypeCode.Single:
                    return ReferencePool.Acquire<VarSingle>();
                case TypeCode.Double:
                    return ReferencePool.Acquire<VarDouble>();
                case TypeCode.Object:
                    if (typeof(T) == typeof(Vector2))
                        return ReferencePool.Acquire<VarVector2>();
                    if (typeof(T) == typeof(Vector3))
                        return ReferencePool.Acquire<VarVector3>();
                    break;
            }
            Log.Error($"create type failed: {typeof(T)}  {typeCode}");
            return null;
        }
    }
}