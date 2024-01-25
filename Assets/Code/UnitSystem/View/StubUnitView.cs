namespace UnitSystem.View
{
    public class StubUnitView : BaseUnitView
    {
        public override void ChangeHealth(int hpValue) { }

        public override void ChangeSize(float sizeValue) { }

        public override void ChangeSpeed(float speedValue) { }
        protected override void SetTransform()
        {
            UnityEngine.Debug.LogWarning("Can't set transform on StubUnitView. Check unit view initialization!");
        }

        protected override void SetUnitAnimator() { }

        protected override void SetUnitNavigation() { }

        protected override void SetCollision() { }

        public override void SetUnitName(string name) { }
    }
}
