
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;
using System.Threading.Tasks;

namespace cfg
{
public partial class Tables
{
    public GlobalTable GlobalTable {get;private set;}

    public Tables() { }

    public async Task LoadAsync(System.Func<string, Task<JSONNode>> loader)
    {
        GlobalTable = new GlobalTable(await loader("globaltable"));
        ResolveRef();
    }
    
    private void ResolveRef()
    {
        GlobalTable.ResolveRef(this);
    }
}

}
