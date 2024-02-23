using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Logic.Utility;
using Game.Module.Entity;
using Game.Module.Input;
using GameFramework.Event;
using Entity = Game.Module.Entity.Entity;

namespace Game.Logic.PlayerController
{
    public class PlayerControllerEntity : Entity
    {
        private Dictionary<EPlayerState, PlayerStateBase> m_PlayerStateDic = new();
        private EPlayerState m_CurrentState = EPlayerState.None;

        public override void Awake()
        {
            m_PlayerStateDic.Add(EPlayerState.Default, new StateDefault());
            m_PlayerStateDic.Add(EPlayerState.Jump, new StateJump());
            m_PlayerStateDic.Add(EPlayerState.Fall, new StateFall());
            
            foreach (var state in m_PlayerStateDic)
                state.Value.SetHost(this);

            AddComponent<UpdateComponent>();
            AddComponent<AttrComponent>();
            AddComponent<DefaultComponent>();
            AddComponent<JumpComponent>();
            
            GameComponent.Event.Subscribe(Const.EventId.Input, OnInputMove);
            GameComponent.Event.Subscribe(Const.EventId.Input, OnInputJump);
        }

        public override void OnDestroy()
        {
            GameComponent.Event.Unsubscribe(Const.EventId.Input, OnInputMove);
            GameComponent.Event.Unsubscribe(Const.EventId.Input, OnInputJump);
        }

        public override void Update()
        {
            // 自动进入状态
            foreach (var state in m_PlayerStateDic)
            {
                if (state.Value.AutoEnter(m_CurrentState))
                {
                    SwitchState(state.Key);
                    break;
                }
            }

            // 当前状态自动退出
            if (m_PlayerStateDic.TryGetValue(m_CurrentState, out var current))
            {
                if (current.AutoExit())
                {
                    SwitchState(current.GetAutoExitState());
                }
            }
        }

        private void SwitchState(EPlayerState toState)
        {
            if (!m_PlayerStateDic.TryGetValue(toState,out var state))
                return;
            if (m_PlayerStateDic.TryGetValue(m_CurrentState, out var current)) 
                current.OnExit(toState);
            state.OnEnter(m_CurrentState);
            m_CurrentState = toState;
        }

        private void OnInputMove(object sender, GameEventArgs e)
        {
            if (e is InputEventArgs { InputType: EInputType.Move }) 
            {
                var state = m_PlayerStateDic[EPlayerState.Default];
                if (state.CanEnterFrom(m_CurrentState)) 
                {
                    SwitchState(EPlayerState.Default);
                }
            }
        }

        private void OnInputJump(object sender, GameEventArgs e)
        {
            if (e is InputEventArgs { InputType: EInputType.Jump }) 
            {
                var state = m_PlayerStateDic[EPlayerState.Jump];
                if (state.CanEnterFrom(m_CurrentState)) 
                {
                    SwitchState(EPlayerState.Jump);
                }
            }
        }
    }
}