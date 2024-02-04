
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public partial class GlobalTable
{
    private readonly System.Collections.Generic.Dictionary<string, GlobalConfig> _dataMap;
    private readonly System.Collections.Generic.List<GlobalConfig> _dataList;
    
    public GlobalTable(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<string, GlobalConfig>();
        _dataList = new System.Collections.Generic.List<GlobalConfig>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            GlobalConfig _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = GlobalConfig.DeserializeGlobalConfig(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Name, _v);
        }
    }

    public System.Collections.Generic.Dictionary<string, GlobalConfig> DataMap => _dataMap;
    public System.Collections.Generic.List<GlobalConfig> DataList => _dataList;

    public GlobalConfig GetOrDefault(string key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public GlobalConfig Get(string key) => _dataMap[key];
    public GlobalConfig this[string key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

