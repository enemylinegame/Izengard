using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), 
    typeof(NavMeshAgent), 
    typeof(CapsuleCollider))]
public class WorkerView : MonoBehaviour, IWorkerView
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private NavMeshAgent _navigationAgent;

    public void InitPlace(Vector3 place)
    {
        _navigationAgent.Warp(place);
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
        return _navigationAgent.remainingDistance < 1;
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

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private const string IDLE = "Idle";
    private const string WALK = "Walk";
    private const string PRODUCE_WORK = "ProduceWork";
    private const string DRAG_BACK = "DragBack";

}
