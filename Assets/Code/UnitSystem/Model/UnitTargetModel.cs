using UnitSystem.View;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitTargetModel
    {
        private Vector3 _positionedTarget = Vector3.zero;
        private BaseUnitView _unitTarget;

        public Vector3 PositionedTarget => _positionedTarget;
        public BaseUnitView UnitTarget => _unitTarget;

        public UnitTargetModel()
        {
           
        }

        public void SetPositionedTarget(Vector3 target)
        {
            _positionedTarget = target;
        }

        public void SetUnitTarget(BaseUnitView target)
        {
            _unitTarget = target;
        }
    }
}
