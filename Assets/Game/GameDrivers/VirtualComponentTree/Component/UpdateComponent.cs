namespace Game.Module
{
    public class UpdateComponent : VComponent
    {
        public override bool DefaultEnable { get; set; } = true;
        
        public override void Update()
        {
            VGameObject.Update();
        }

        public override void LateUpdate()
        {
            VGameObject.LateUpdate();
        }
    }
}