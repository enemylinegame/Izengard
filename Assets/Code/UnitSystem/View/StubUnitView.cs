namespace Izengard.UnitSystem.View
{
    public class StubUnitView : BaseUnitView
    {
        public override void ChangeHealth(int hpValue) { }

        public override void ChangeSize(float sizeValue) { }

        public override void ChangeSpeed(float speedValue) { }
        protected override void OnSetTransform()
        {
            UnityEngine.Debug.LogWarning("Can't set transform on StubUnitView. Check unit view initialization!");
        }

        protected override void OnSetUnitAnimator() { }

        protected override void OnSetUnitNavigation() { }
    }
}
