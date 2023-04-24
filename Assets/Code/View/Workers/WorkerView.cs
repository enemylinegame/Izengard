using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), 
    typeof(NavMeshAgent), 
    typeof(CapsuleCollider))]
public class WorkerView : MonoBehaviour, IWorkerView
{
    public void InitPlace(Vector3 place)
    {
        _navigationAgent.Warp(place);
        _animator.Play("Idle");
    }

    public void GoToPlace(Vector3 place)
    {
        _navigationAgent.SetDestination(place);
        _animator.Play("Walk");
    }

    public bool IsOnThePlace()
    {
        return _navigationAgent.remainingDistance < 0.1;
    }

    public void ProduceWork()
    {
        _animator.Play("ProduceWork");
    }

    public void Idle(Vector3 place)
    {
        _animator.Play("Idle");
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _navigationAgent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private Animator _animator;
    private NavMeshAgent _navigationAgent;
}
