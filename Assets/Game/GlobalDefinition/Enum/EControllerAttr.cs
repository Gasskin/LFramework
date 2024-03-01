namespace Game.GlobalDefinition
{
    public enum EControllerAttr
    {
        None = 0,
        MoveMode, // EMoveMode
        MoveDir, // float 
        MoveSpeed, // float
        CharacterState, // ECharacterState
    }

    public static class EControllerAttrExtension
    {
        public static uint ToUint(this EControllerAttr attr)
        {
            return (uint)attr;
        }
    }
}