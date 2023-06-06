using System;

namespace CombatSystem.DefenderStates
{
    public class DefenderInBarrack : DefenderStateBase
    {
        
        public DefenderInBarrack(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate) : 
            base(defenderUnit, setStateDelegate)
        {
            
        }

        public override void StartState()
        {
             _defenderUnit.DefenderGameObject.SetActive(false);
        }

        public override void ExitFromBarrack()
        {
            _defenderUnit.DefenderGameObject.SetActive(true);
            _setState(DefenderState.Going);
        }
    }
}