using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), 
    typeof(NavMeshAgent))]
public class WorkerView : MonoBehaviour, IWorkerView
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private NavMeshAgent _navigationAgent;

    [SerializeField]
    private float _distanceToBeginWork;


    public Action OnCollideWithOtherWorker { get; set; }

    public void InitPlace(Vector3 place)
    {
        _navigationAgent.Warp(place);
        _animator.SetTrigger(WALK);
    }

    public void ContinueWalkFromCurrentPlace()
    {
        _animator.SetTrigger(WALK);
    }

    public void GoToPlace(Vector3 place)
    {
        _animator.ResetTrigger(IDLE);
        _animator.SetTrigger(WALK);
        _navigationAgent.SetDestination(place);
    }

    public bool IsOnThePlace()
    {
        if (_navigationAgent.remainingDistance <= _distanceToBeginWork)
        {
            _navigationAgent.ResetPath();
            return true;
        }

        return false;
    }

    public void ProduceWork()
    {
        _animator.ResetTrigger(WALK);
        _animator.SetTrigger(PRODUCE_WORK);
    }

    public void DragToPlace(Vector3 place)
    {
        _animator.ResetTrigger(IDLE);
        _animator.ResetTrigger(PRODUCE_WORK);
        _animator.SetTrigger(DRAG_BACK);
        _navigationAgent.SetDestination(place);
    }

    public void Idle()
    {
        _animator.ResetTrigger(WALK);
        _animator.ResetTrigger(PRODUCE_WORK);
        _animator.SetTrigger(IDLE);
    }

    public void Pause()
    {
        _navigationAgent.isStopped = true;
        Idle();
    }

    private void Resume(string targetAction)
    {
        _animator.ResetTrigger(IDLE);
        _animator.SetTrigger(targetAction);
        _navigationAgent.isStopped = false;
    }

    public void ResumeWalk()
    {
        Resume(WALK);
    }

    public void ResumeWork()
    {
        Resume(PRODUCE_WORK);
    }

    public void ResumeDrag()
    {
        Resume(DRAG_BACK);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(
            out IWorkerView opponent))
            return;

        Debug.Log("workers colliding");
        OnCollideWithOtherWorker?.Invoke();
    }

    private const string IDLE = "Idle";
    private const string WALK = "Walk";
    private const string PRODUCE_WORK = "ProduceWork";
    private const string DRAG_BACK = "DragBack";
}
