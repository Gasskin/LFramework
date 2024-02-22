using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Logic.Utility;
using Game.Module.Entity;
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
            m_PlayerStateDic.Add(EPlayerState.Default, new DefaultState());

            foreach (var state in m_PlayerStateDic)
            {
                state.Value.SetHost(this);
            }

            AddComponent<UpdateComponent>();
            AddComponent<AttrComponent>();
            AddComponent<PrepareMoveComponent>();
            AddComponent<MoveComponent>();

            GameComponent.Event.Subscribe(Const.EventId.InputMove, OnInputMove);
        }

        public override void OnDestroy()
        {
            GameComponent.Event.Unsubscribe(Const.EventId.InputMove, OnInputMove);
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
            if (e is InputMoveEventArgs arg)
            {
                var state = m_PlayerStateDic[EPlayerState.Default];
                if (state.CanEnterFrom(m_CurrentState)) 
                {
                    SwitchState(EPlayerState.Default);
                }
            }
        }
    }
}