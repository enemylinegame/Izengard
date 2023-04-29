namespace Code.TileSystem
{
    public class WorkersAssigments
    {
        public Building Building;
        public int BusyWorkersCount;

        public WorkersAssigments(Building building, int busyWorkersCount = 0)
        {
            Building = building;
            BusyWorkersCount = busyWorkersCount;
        }
    }
}