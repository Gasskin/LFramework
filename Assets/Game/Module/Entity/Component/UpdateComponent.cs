namespace Game.Module
{
    public class UpdateComponent : EntityComponent
    {
        public override bool DefaultEnable { get; set; } = true;
        
        public override void Update()
        {
            NodeEntity.Update();
        }

        public override void LateUpdate()
        {
            NodeEntity.LateUpdate();
        }
    }
}