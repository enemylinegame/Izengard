using UnityEngine;

public class AnimWorker 
{
    #region Fields
    private const string _isRun = "isRun";
    private const string _isProduction = "Production";
    #endregion


    #region Methods

    public void WorkerIsMove(Animator _animator)
    {
        _animator.SetBool(_isRun, true);
    }

    public void WorkerIsIdle(Animator _animator)
    {
        _animator.SetBool(_isRun, false);
    }

    public void WorkerProduce(Animator _animator)
    {
        _animator.SetTrigger(_isProduction);
    }

    #endregion
}
