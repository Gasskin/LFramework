namespace Game.HotFix.GameDrivers
{
    public class UpdateComponent : ECComponent
    {
        public override bool DefaultEnable { get; set; } = true;
        
        public override void Update()
        {
            Entity.Update();
        }

        public override void LateUpdate()
        {
            Entity.LateUpdate();
        }
    }
}