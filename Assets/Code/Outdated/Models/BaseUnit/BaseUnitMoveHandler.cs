using Controllers.BaseUnit;
using UnityEngine;
using UnityEngine.AI;
using Views.BaseUnit;

public sealed class BaseUnitMoveHandler: UnitHandler, IOnUpdate
{
    private readonly UnitMovement _unitMovement;
    private readonly BaseUnitController _baseUnitController;


    public BaseUnitMoveHandler(UnitMovement unitMovement,BaseUnitController baseUnitController)
    {
        _unitMovement = unitMovement;
        _baseUnitController = baseUnitController;
   
    }

    public void OnUpdate(float deltaTime)
    {
        _unitMovement.navMeshAgent.autoTraverseOffMeshLink = false;
        if (_unitMovement.navMeshAgent.isOnOffMeshLink)
        {
            OffMeshLinkData data = _unitMovement.navMeshAgent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos;
            _baseUnitController.NormalSpeed(_unitMovement.navMeshAgent, endPos, deltaTime);
        
        }
        else if (_unitMovement.navMeshAgent.destination.x == _unitMovement.transform.position.x &&
                 _unitMovement.navMeshAgent.destination.z == _unitMovement.transform.position.z)
        {
            StoppedAtPosition();
        }
          
    }
      
    public override IUnitHandler Handle()
    {
        _baseUnitController.CurrentUnitHandler = GetCurrent();
        _unitMovement.SetThePointWhereToGo();
        return this;
    }
    private void StoppedAtPosition()
    {
        if (_unitMovement.CountOfSequence+1 < _baseUnitController.MoveCounter)
            _unitMovement.CountOfSequence++;
        else
            _unitMovement.CountOfSequence = 0;
        base.Handle();
    }

  
}
