﻿using Game.HotFix.GlobalDefinition;
using Game.HotFix.GameDrivers;

namespace Game.HotFix.Logic
{
    public abstract class CharacterStateBase
    {
        public ECEntity Host { get; private set; }

        public void SetHost(ECEntity host)
        {
            Host = host;
        }
        
        /// <summary>
        /// 是否可以进入当前状态
        /// </summary>
        /// <param name="fromState">来自哪个状态</param>
        /// <returns></returns>
        public virtual bool CanEnterFrom(ECharacterState fromState)
        {
            return false;
        }

        /// <summary>
        /// 自动进入
        /// </summary>
        /// <returns></returns>
        public virtual bool AutoEnter(ECharacterState currentState)
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

        public virtual void OnEnter(ECharacterState fromState)
        {
            
        }

        public virtual void OnExit(ECharacterState toState)
        {
            
        }
        
        public virtual ECharacterState GetAutoExitState()
        {
            return ECharacterState.None;
        }
    }
}