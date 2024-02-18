
using Entitas;

namespace Game.Entitas
{
    public class GameObjectSystem: BaseSystem<GameEntity>
    {
        public GameObjectSystem(IContext<GameEntity> context) : base(context)
        {
        }

        public GameObjectSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }

        public override ICollector<GameEntity> CreateTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GameObject);
        }
    }
}