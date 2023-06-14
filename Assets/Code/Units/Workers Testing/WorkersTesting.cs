using System.Collections.Generic;
using UnityEngine;

public class WorkersTesting : MonoBehaviour
{
    List<int> _works;
    Vector3 _targetWorkPlace;
    Vector3 _targetMinePlace;

    WorkersTeamController _controller;
    void Start()
    {
        Vector3 homePosition = new Vector3(5, 0, 5);

        GameObject.CreatePrimitive(PrimitiveType.Cube)
            .transform.position = homePosition;

        WorkersTeamConfig config = 
            Resources.Load<WorkersTeamConfig>("WorkersTeam");

        _controller = new WorkersTeamController(
            config);

        _works = new List<int>();

        //_targetMinePlace = new Vector3(30, 0, 30);
        //_works.Add(_controller.SendWorkerToMine(
        //    homePosition, _targetPlace, new WorkerTestWork()));

        _targetWorkPlace = new Vector3(30, 0, 5);
        
        //_works.Add(_controller.SendWorkerToWork(
        //    homePosition, _targetWorkPlace, 
        //    new WorkerTestBegin(),
        //    new WorkerTestWork(),
        //    new WorkerTestEnd()));
    }

    void Update()
    {
        _controller.OnUpdate(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var workerId in _works)
                _controller.CancelWork(workerId);

        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _controller.Pause();
            Debug.Log("Paused");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _controller.Resume();
            Debug.Log("Resumed");
        }
    }

    private void OnDestroy()
    {
        _controller.Dispose();
    }
}
