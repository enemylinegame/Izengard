namespace BattleSystem
{
    public abstract class BaseBattleController : IOnController, IOnUpdate, IOnFixedUpdate
    {
        protected readonly TargetFinder targetFinder;

        public BaseBattleController(TargetFinder targetFinder)
        {
            this.targetFinder = targetFinder;
        }

        public abstract void OnUpdate(float deltaTime);

        public abstract void OnFixedUpdate(float fixedDeltaTime);
    }
}
