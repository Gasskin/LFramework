
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
public sealed partial class LocalizationConfig : Luban.BeanBase
{
    public LocalizationConfig(JSONNode _buf) 
    {
        { if(!_buf["key"].IsString) { throw new SerializationException(); }  Key = _buf["key"]; }
        { if(!_buf["cn"].IsString) { throw new SerializationException(); }  Cn = _buf["cn"]; }
        { if(!_buf["en"].IsString) { throw new SerializationException(); }  En = _buf["en"]; }
    }

    public static LocalizationConfig DeserializeLocalizationConfig(JSONNode _buf)
    {
        return new LocalizationConfig(_buf);
    }

    public readonly string Key;
    public readonly string Cn;
    public readonly string En;
   
    public const int __ID__ = -1901143205;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "key:" + Key + ","
        + "cn:" + Cn + ","
        + "en:" + En + ","
        + "}";
    }
}

}
