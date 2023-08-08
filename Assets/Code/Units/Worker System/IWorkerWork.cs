

public interface IWorkerWork
{
    bool Produce(float deltaTime);
    bool IsProductionSuccess { get; }
    void Dispose();
}
