namespace Game.Module.Entity
{
    public class UpdateComponent : EntityComponent
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