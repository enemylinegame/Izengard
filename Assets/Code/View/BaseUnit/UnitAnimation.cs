using UnityEngine;

namespace Views.BaseUnit
{
    public class UnitAnimation : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Animator _animator;

        #endregion


        #region Methods

        public void SetAnimationState(string animationName)
        {
            //_animator.Play(animationName);
        }

        #endregion
        
    }
}