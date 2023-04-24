

using UnityEngine;

public interface IWorkerView
{
    void InitPlace(Vector3 place);
    void GoToPlace(Vector3 place);
    bool IsOnThePlace();
    void ProduceWork();
    void Activate();
    void Deactivate();
}
