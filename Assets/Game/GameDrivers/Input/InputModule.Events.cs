using System;
using Rewired;

namespace Game.Module
{
    public partial class InputDriver
    {
        private void RegisterActions()
        {
            AddInputEventDelegate(ProcessJump, InputActionEventType.Update, RewiredConsts.Action.Jump);
        }

        private void UnRegisterActions()
        {
            m_Player.RemoveInputEventDelegate(ProcessJump);
        }
        
        private void AddInputEventDelegate(Action<InputActionEventData> callback, InputActionEventType eventType, int actionId)
        {
            if (m_Player == null) 
                return;
            m_Player.AddInputEventDelegate(callback, UpdateLoopType.Update, eventType, actionId);
        }

        private void ProcessJump(InputActionEventData inputActionEventData)
        {
            if (inputActionEventData.GetButtonDown())
            {
                Jump(true);
            }
            
            if (inputActionEventData.GetButtonUp())
            {
                Jump(false);
            }
        }
    }
}