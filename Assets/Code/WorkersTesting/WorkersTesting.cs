using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersTesting : MonoBehaviour
{
    WorkersTeamController _controller;
    void Start()
    {
        WorkersTeamConfig config = Resources.Load<WorkersTeamConfig>("WorkersTeam");


        WorkerTeamViewFactory workersFactory = new WorkerTeamViewFactory();
        IWorkerTeamView teamView = 
            workersFactory.CreateWorkerTeamView(config);

        _controller = new WorkersTeamController(
            config, new Vector3(5, 0, 5), teamView);

        _controller.SendTeamToWork(new Vector3(30, 0, 30));
        _controller.SendSingleWorkerToPlace(new Vector3(30, 0, 0));
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
