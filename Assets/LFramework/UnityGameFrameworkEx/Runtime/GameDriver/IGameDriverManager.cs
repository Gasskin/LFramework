using System;

namespace GameFramework.GameDriver
{
    public interface IGameDriverManager
    {
        Action OnUpdate { get;  set; }
        
        Action OnLateUpdate { get;  set; }
        
        Action OnFixedUpdate { get;  set; }
        
        Action OnShutDown { get; set; }
        
        void SetUpdateAction(Action action);
        void SetLateUpdateAction(Action action);
        void SetFixedUpdateAction(Action action);
        void SetShutDownAction(Action action);
    }
}