using System.Collections.Generic;
using UnityEngine;

public class WorkerTeamViewFactory
{
    public IWorkerTeamView CreateWorkerTeamView(WorkersTeamConfig config)
    {
        GameObject teamGameObject =
           new GameObject("WorkerTeam");

        WorkerTeamView teamView =
            teamGameObject.AddComponent<WorkerTeamView>();

        teamView.WorkersViews = new List<IWorkerView>();

        for (int i = 0; i < config.WorkersAmount; ++i)
        {
            GameObject workerGameObject = GameObject.Instantiate(
                   config.WorkerPrefab, teamGameObject.transform);

            IWorkerView workerView = workerGameObject.GetComponent<IWorkerView>();
            teamView.WorkersViews.Add(workerView);
        }

        return teamView;
    }
}
