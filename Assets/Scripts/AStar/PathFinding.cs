using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;
using Utils;

namespace AStar
{
    public class PathFinding : Singleton<PathFinding>
    {
        [SerializeField] private Transform debugObjectPrefab;
        [SerializeField] private LayerMask obstaclesLayerMask;
        private int width;
        private int height;
        private float cellSize;
        private GridSystem<PathNode> gridSystem;

        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;

        public void Setup(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            
            gridSystem = new GridSystem<PathNode>(this.width, this.height, this.cellSize, 
                (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

            SetIsWalkable();
            
            //gridSystem.CreateDebugObject(debugObjectPrefab, transform);
        }

        private void SetIsWalkable()
        {
            for (int x = 0; x < this.width; x++)
            {
                for (int z = 0; z < this.height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    var rayCastOffSetDistance = 5;
                    // + v3.down Because the raycast must not be positioned in the box collider.
                    var worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition) +
                                        Vector3.down * rayCastOffSetDistance;
                    if (Physics.Raycast(worldPosition, Vector3.up, rayCastOffSetDistance * 2, obstaclesLayerMask))
                    {
                        GetNode(gridPosition.x, gridPosition.z).SetIsWalkable(false);
                    }
                }
            }
        }

        public  (List<GridPosition> gridPositionList, int pathLength) FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            // open list contain all the node queued for searching
            // The next finder will be item in Openlist.
            // For instance, we found the value of b that was the quickest way after using the value of a before,
            // therefore we use the value of b to find the next route.
            var openList = new List<PathNode>();
            // therefore we use the value of b to find the next route.
            var closedList = new List<PathNode>();

            var startNode = gridSystem.GetGridObject(startGridPosition);
            var endNode = gridSystem.GetGridObject(endGridPosition);
            openList.Add(startNode);
            
            // set up var all 
            for (int x = 0; x < gridSystem.GetWidth(); x++)
            {
                for (int z = 0; z < gridSystem.GetHeight(); z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    var pathNode = gridSystem.GetGridObject(gridPosition);

                    pathNode.SetUpNode();
                }
            }

            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                var currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    // reached final node
                    return (CalculatePath(endNode), endNode.GetFCost());
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    // already searched
                    if (closedList.Contains(neighbourNode))
                    {
                        continue;
                    }

                    if (!neighbourNode.IsWalkable())
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    var distance = CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                    var tentativeGCost = currentNode.GetGCost() + distance;

                    // check is found the best path
                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        neighbourNode.SetCameFromPathNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // No path found
            return (null, 0);
        }

        
        private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
        {
            var gridPositionDistance = gridPositionA - gridPositionB;
            // found min value for * MoveDiagonalCost
            var xDistance = Mathf.Abs(gridPositionDistance.x);
            var zDistance = Mathf.Abs(gridPositionDistance.z);
            
            var remaining = Mathf.Abs(xDistance - zDistance);
            return MoveDiagonalCost * Mathf.Min(xDistance, zDistance) + MoveStraightCost * remaining;

        }

        private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
        {
            var lowestFCostPathNode = pathNodeList[0];

            foreach (var pathNode in pathNodeList)
            {
                if (pathNode.GetFCost() < lowestFCostPathNode.GetFCost())
                {
                    lowestFCostPathNode = pathNode;
                }
            }

            return lowestFCostPathNode;
        }

        private PathNode GetNode(int x, int z)
        {
            return gridSystem.GetGridObject(new GridPosition(x,z));
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            var neighbourList = new List<PathNode>();

            var gridPosition = currentNode.GetGridPosition();

            if (gridPosition.x - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
                if (gridPosition.z - 1 >= 0)
                {
                    // Left Down
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    // Left Up
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }

            if (gridPosition.x + 1 < gridSystem.GetWidth())
            {
                // Right
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
                if (gridPosition.z - 1 >= 0)
                {
                    // Right Down
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }
                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    // Right Up
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }
            }

            if (gridPosition.z - 1 >= 0)
            {
                // Down
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Up
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
            }


            return neighbourList;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            var pathNodeList = new List<PathNode> { endNode };
            var currentNode = endNode;
            // while for get path from end node to start node
            while (currentNode.GetCameFromPathNode() != null)
            {
                pathNodeList.Add(currentNode.GetCameFromPathNode());
                currentNode = currentNode.GetCameFromPathNode();
            }

            pathNodeList.Reverse();

            return pathNodeList.Select(pathNode => pathNode.GetGridPosition()).ToList();
        }

        public bool IsWalkableGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).IsWalkable();
        }

        public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            var gridPositionList = FindPath(startGridPosition, endGridPosition).gridPositionList;
            return gridPositionList != null;
        }

        public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
        {
           return FindPath(startGridPosition, endGridPosition).pathLength;
        }
    }
}