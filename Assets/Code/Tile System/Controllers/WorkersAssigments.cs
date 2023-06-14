using Code.BuildingSystem;

namespace Code.TileSystem
{
    public class WorkersAssigments
    {
        public ICollectable Building;
        public int BusyWorkersCount;    

        public WorkersAssigments(ICollectable building, int busyWorkersCount = 0)
        {
            Building = building;
            BusyWorkersCount = busyWorkersCount;
        }
    }
}