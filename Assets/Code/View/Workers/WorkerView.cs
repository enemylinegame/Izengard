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
        _animator.Play(IDLE);
    }

    public void GoToPlace(Vector3 place)
    {
        _navigationAgent.SetDestination(place);
        _animator.Play(WALK);
    }

    public bool IsOnThePlace()
    {
        return _navigationAgent.remainingDistance < 1;
    }

    public void ProduceWork()
    {
        _animator.Play(PRODUCE_WORK);
    }

    public void Idle()
    {
        _animator.Play(IDLE);
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
    private const string WALK = "WALK";
    private const string PRODUCE_WORK = "ProduceWork";

}
