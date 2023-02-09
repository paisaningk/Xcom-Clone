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

        public void SetGCost(int cost)
        {
            gCost = cost;
        }
        
        public void SetHCost(int cost)
        {
            hCost = cost;
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
        
        public int CalculateFCost()
        {
            return gCost + hCost;
        }

        public void ResetCameFromPathNode()
        {
            cameFromPathNode = null;
        }

        public void SetCameFromPathNode(PathNode pathNode)
        {
            cameFromPathNode = pathNode;
        }

        public PathNode GetCameFromPathNode()
        {
            return cameFromPathNode;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }

        public void SetUpNode()
        {
            SetGCost(int.MaxValue);
            SetHCost(0);
            CalculateFCost();
            ResetCameFromPathNode();
        }
    }
}