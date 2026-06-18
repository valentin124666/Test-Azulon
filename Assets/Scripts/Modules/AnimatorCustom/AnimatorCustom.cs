using Modules.Interfaces;
using UnityEngine;

namespace Modules.AnimatorCustom
{
    public abstract class AnimatorCustom : MonoBehaviour, IAnimatorCustom
    {
        [SerializeField] protected Animator _animator;
        
        protected virtual void Registration()
        {
        }

        public void SetAnimation(int number)
        {
            _animator.SetTrigger(number);
        }
        public void SetAnimation(string name)
        {
            _animator.SetTrigger(name);
        }
        public void ResetTriggerAnimation(string name)
        {
            _animator.ResetTrigger(name);
        }
    }
}