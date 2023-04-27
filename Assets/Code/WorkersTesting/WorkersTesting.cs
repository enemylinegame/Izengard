using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersTesting : MonoBehaviour
{
    WorkersTeamController _controller;
    void Start()
    {
        WorkersTeamConfig config = Resources.Load<WorkersTeamConfig>("WorkersTeam");

        _controller = new WorkersTeamController(
            config);

        _controller.SendWorkerToPlace(new Vector3(5, 0, 5), new Vector3(30, 0, 30));
        _controller.SendWorkerToWork(new Vector3(5, 0, 5), new Vector3(30, 0, 5));
    }

    void Update()
    {
        _controller.OnUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {
        _controller.Dispose();
    }
}
