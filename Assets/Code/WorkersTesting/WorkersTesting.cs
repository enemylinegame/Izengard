using System.Collections.Generic;
using UnityEngine;

public class WorkersTesting : MonoBehaviour
{
    Dictionary<int, int> _works;
    Vector3 _targetWorkPlace;
    Vector3 _targetPlace;

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

        _controller.OnMissionCompleted += OnMissionCompleted;

        _works = new Dictionary<int, int>();

        _targetPlace = new Vector3(30, 0, 30);
        int resourceAmount = 0;
        _works.Add(_controller.SendWorkerToPlace(
            homePosition, _targetPlace), 0);

        _targetWorkPlace = new Vector3(30, 0, 5);
        resourceAmount = 50;
        _works.Add(_controller.SendWorkerToWork(
            homePosition, _targetWorkPlace), resourceAmount);
    }

    private void OnMissionCompleted(int workId)
    {
        if (0 == _works[workId])
        {
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere).transform.position = _targetPlace;
            return;
        }

        Debug.Log($"Completed task {workId}, " +
            $"received resource: {_works[workId]}");
    }

    void Update()
    {
        _controller.OnUpdate(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var kvp in _works)
                _controller.CancelWork(kvp.Key);
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
        _controller.OnMissionCompleted -= OnMissionCompleted;
        _controller.Dispose();
    }
}
