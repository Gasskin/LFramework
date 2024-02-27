namespace Game.GlobalDefinition
{
    public enum EModelAttr
    {
        None = 0,
        Position,
        Rotation,
        IsOnGround,
    }
    
    public static class EModelAttrExtension
    {
        public static uint ToUint(this EModelAttr e)
        {
            return (uint)e;
        }
    }
}