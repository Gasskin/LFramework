using System.IO;
using Bright.Serialization;
using cfg;
using GameFramework.Fsm;
using GameFramework.Procedure;
using SimpleJSON;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace LFramework
{
    public class TestProcedure : ProcedureBase
    {
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            var tables = new Tables((s => new ByteBuf(File.ReadAllBytes($"Assets/AssetsPackage/LuBan/{s}.bytes"))));
            Log.Error(tables.AITable.Get(201).Desc);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
    }
}
