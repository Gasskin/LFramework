namespace Game.GlobalDefinition
{
    public enum EModelAttr
    {
        None = 0,
        Position, // Vector3
        Rotation, // Vector3
        IsOnGround, // bool
    }
    
    public static class EModelAttrExtension
    {
        public static uint ToUint(this EModelAttr e)
        {
            return (uint)e;
        }
    }
}