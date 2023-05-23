using System.Collections.Generic;
using UnityEngine;


public interface IWorkerTeamView
{
    IList<IWorkerView> WorkersViews { get; set; }
}
