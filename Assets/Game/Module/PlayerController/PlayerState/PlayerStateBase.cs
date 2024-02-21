namespace Game.Module.PlayerController
{
    public abstract class PlayerStateBase
    {
        public Entity.Entity Host { get; private set; }
        
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
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public virtual bool AutoEnter(float deltaTime)
        {
            return false;
        }

        /// <summary>
        /// 自动退出
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public virtual bool AutoExit(float deltaTime)
        {
            return false;
        }

        public virtual void OnEnter(EPlayerState fromState)
        {
            
        }

        public virtual void OnExit(EPlayerState toState)
        {
            
        }
    }
}