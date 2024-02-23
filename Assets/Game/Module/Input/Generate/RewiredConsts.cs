// <auto-generated>
// Rewired Constants
// This list was generated on 2024/2/6 18:02:50
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.
// </auto-generated>

namespace Game.Module {
    public static partial class RewiredConsts {
        public static partial class Action {
            // Default
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Move Horizontal")]
            public const int Move_Horizontal = 0;
            
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Move Vertical")]
            public const int Move_Vertical = 1;
            
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Fire")]
            public const int Fire = 2;
            
            // Battle
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Battle", friendlyName = "左移")]
            public const int Move_Left = 5;
            
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Battle", friendlyName = "右移")]
            public const int Move_Right = 6;
            
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Battle", friendlyName = "跳")]
            public const int Jump = 8;
            
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "Battle", friendlyName = "蹲")]
            public const int Down = 9;
            
            // UI
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "地图")]
            public const int Map = 7;
            
            [Rewired.Dev.ActionIdFieldInfo(categoryName = "UI", friendlyName = "设置")]
            public const int Setting = 10;
            
        }
        public static partial class Category {
            public const int Default = 0;
            
            public const int Battle = 1;
            
            public const int UI = 2;
            
        }
        public static partial class Layout {
            public static partial class Joystick {
                public const int Default = 0;
                
            }
            public static partial class Keyboard {
                public const int Default = 0;
                
            }
            public static partial class Mouse {
                public const int Default = 0;
                
            }
            public static partial class CustomController {
                public const int Default = 0;
                
            }
        }
        public static partial class Player {
            [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "System")]
            public const int System = 9999999;
            
            [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player")]
            public const int Player0 = 0;
            
        }
        public static partial class CustomController {
        }
        public static partial class LayoutManagerRuleSet {
        }
        public static partial class MapEnablerRuleSet {
            public const int UI = 3;
            
            public const int DisableAll = 5;
            
            public const int Battle = 7;
            
        }
    }
}
