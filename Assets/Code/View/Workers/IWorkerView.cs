

using UnityEngine;

public interface IWorkerView
{
    void InitPlace(Vector3 place);
    void GoToPlace(Vector3 place);
    bool IsOnThePlace();
    void ProduceWork();
    void DragToPlace(Vector3 place);
    void Pause();
    void ResumeWalk();
    void ResumeWork();
    void ResumeDrag();
    void Activate();
    void Deactivate();
}
