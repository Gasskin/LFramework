using System.Collections.Generic;
using Game.GlobalDefinition;
using Game.Module;
using GameFramework.Event;

namespace Game.Logic
{
    public class CharacterControllerNodeEntity : NodeEntity
    {
        private Dictionary<ECharacterState, CharacterStateBase> m_PlayerStateDic = new();
        private ECharacterState m_CurrentState = ECharacterState.None;
        private AttrComponent m_Attr;

        public override void Awake()
        {
            m_PlayerStateDic.Add(ECharacterState.Default, new StateDefault());
            m_PlayerStateDic.Add(ECharacterState.Jump, new StateJump());
            m_PlayerStateDic.Add(ECharacterState.Fall, new StateFall());
            
            foreach (var state in m_PlayerStateDic)
                state.Value.SetHost(this);

            AddComponent<UpdateComponent>();
            m_Attr = AddComponent<AttrComponent>();
            
            AddComponent<DefaultControllerComponent>();
            AddComponent<JumpControllerComponent>();
            AddComponent<FallControllerComponent>();
            
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

        private void SwitchState(ECharacterState toState)
        {
            if (!m_PlayerStateDic.TryGetValue(toState,out var state))
                return;
            if (m_PlayerStateDic.TryGetValue(m_CurrentState, out var current)) 
                current.OnExit(toState);
            state.OnEnter(m_CurrentState);
            m_CurrentState = toState;
            m_Attr.SetAttr((uint)EControllerAttr.CharacterState, toState);
        }

        private void OnInputMove(object sender, GameEventArgs e)
        {
            if (e is InputEventArgs { InputType: EInputType.Move }) 
            {
                var state = m_PlayerStateDic[ECharacterState.Default];
                if (state.CanEnterFrom(m_CurrentState)) 
                {
                    SwitchState(ECharacterState.Default);
                }
            }
        }

        private void OnInputJump(object sender, GameEventArgs e)
        {
            if (e is InputEventArgs { InputType: EInputType.Jump }) 
            {
                var state = m_PlayerStateDic[ECharacterState.Jump];
                if (state.CanEnterFrom(m_CurrentState)) 
                {
                    SwitchState(ECharacterState.Jump);
                }
            }
        }
    }
}