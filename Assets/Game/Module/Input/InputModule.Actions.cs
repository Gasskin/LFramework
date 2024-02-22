using Game.Logic;
using Game.Logic.PlayerController;
using Game.Logic.Utility;
using Game.Module.Entity;
using UnityEngine;

namespace Game.Module.Input
{
    public partial class InputModule
    {
        public void Move(EMoveDir dir)
        {
            if (MoveDir == dir) 
                return;
            MoveDir = dir;
  
            GameComponent.Event.Fire(this, InputMoveEventArgs.Create(dir));
        }
    }
}