using System.Collections.Generic;
using Entitas;
using UnityGameFramework.Runtime;

namespace Game.Entitas
{
    public abstract class BaseSystem<T> : ReactiveSystem<T>,IInitializeSystem,ICleanupSystem,ITearDownSystem,IExecuteSystem where T : class, IEntity
    {
    #region Override
        protected BaseSystem(IContext<T> context) : base(context)
        {
        }

        protected BaseSystem(ICollector<T> collector) : base(collector)
        {
        }

        protected override ICollector<T> GetTrigger(IContext<T> context)
        {
            return CreateTrigger(context);
        }

        protected override bool Filter(T entity)
        {
            return true;
        }

        protected override void Execute(List<T> entities)
        {
            Log.Error("Execute 111");
        }

        void IExecuteSystem.Execute()
        {
            Log.Error("Execute 222");
        }

        public void Initialize()
        {
        }

        public void Cleanup()
        {
        }

        public void TearDown()
        {
        }
    #endregion

        public abstract ICollector<T> CreateTrigger(IContext<T> context);
    }
}
