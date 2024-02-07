
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
public partial class ItemTable
{
    private readonly System.Collections.Generic.Dictionary<int, ItemConfig> _dataMap;
    private readonly System.Collections.Generic.List<ItemConfig> _dataList;
    
    public ItemTable(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, ItemConfig>();
        _dataList = new System.Collections.Generic.List<ItemConfig>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            ItemConfig _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = ItemConfig.DeserializeItemConfig(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, ItemConfig> DataMap => _dataMap;
    public System.Collections.Generic.List<ItemConfig> DataList => _dataList;

    public ItemConfig GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public ItemConfig Get(int key) => _dataMap[key];
    public ItemConfig this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

