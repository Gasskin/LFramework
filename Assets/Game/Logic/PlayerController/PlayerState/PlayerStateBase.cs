using Game.Module.Entity;

namespace Game.Logic.PlayerController
{
    public abstract class PlayerStateBase
    {
        public Entity Host { get; private set; }



        public void SetHost(Entity host)
        {
            Host = host;
        }
        
        /// <summary>
        /// 是否可以进入当前状态
        /// </summary>
        /// <param name="fromState">来自哪个状态</param>
        /// <returns></returns>
        public virtual bool CanEnterFrom(EPlayerState fromState)
        {
            return false;
        }

        /// <summary>
        /// 自动进入
        /// </summary>
        /// <returns></returns>
        public virtual bool AutoEnter(EPlayerState currentState)
        {
            return false;
        }

        /// <summary>
        /// 自动退出
        /// </summary>
        /// <returns></returns>
        public virtual bool AutoExit()
        {
            return false;
        }

        public virtual void OnEnter(EPlayerState fromState)
        {
            
        }

        public virtual void OnExit(EPlayerState toState)
        {
            
        }
        
        public virtual EPlayerState GetAutoExitState()
        {
            return EPlayerState.None;
        }
    }
}