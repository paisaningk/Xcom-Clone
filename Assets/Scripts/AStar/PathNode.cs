using Grid;

namespace AStar
{
    public class PathNode
    {
        private GridPosition gridPosition;
        private int gCost = 0;
        private int hCost = 0;
        private int fCost = 0;
        private PathNode cameFromPathNode;

        public PathNode(GridPosition gridPosition)
        {
            this.gridPosition = gridPosition;
        }
        
        public override string ToString()
        {
            return gridPosition.ToString();
        }

        public int GetGCost()
        {
            return gCost;
        }
        
        public int GetHCost()
        {
            return hCost;
        }
        
        public int GetFCost()
        {
            return fCost;
        }
    }
}