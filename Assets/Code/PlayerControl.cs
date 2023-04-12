using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    private Camera _mainCamera;
    private NavMeshAgent _meshAgent;
    void Start()
    {
        _mainCamera = Camera.main;
        _meshAgent = GetComponent<NavMeshAgent>();

    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                _meshAgent.SetDestination(hit.point);
            }
            
        }
    }
}
