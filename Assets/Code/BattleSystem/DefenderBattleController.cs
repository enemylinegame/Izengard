using UnitSystem;

namespace BattleSystem
{
    public class DefenderBattleController : BaseBattleController
    {
        public DefenderBattleController(TargetFinder targetFinder) : base(targetFinder)
        {
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            
        }

        public override void AddUnit(IUnit unit) { }
    }
}
