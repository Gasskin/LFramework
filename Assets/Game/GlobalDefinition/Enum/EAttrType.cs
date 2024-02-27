namespace Game.GlobalDefinition
{
    public enum EAttrType
    {
        None = 0,
        MoveMode,
        MoveDir,
        MoveHorizontalVelocity,
        MoveVerticalVelocity,
        CharacterState,
    }
    
    public static class EAttrTypeExtension
    {
        public static uint ToUint(this EAttrType e)
        {
            return (uint)e;
        }
    }
}