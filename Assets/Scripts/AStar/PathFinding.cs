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
        private int width;
        private int height;
        private float cellSize;
        private GridSystem<PathNode> gridSystem;

        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;

        private void Awake()
        {
            gridSystem = new GridSystem<PathNode>(10, 10, 2f, 
                (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            gridSystem.CreateDebugObject(debugObjectPrefab, transform);
        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            //open list contain all the node queued for searching
            //The next finder will be item in Openlist.
            //For instance, we found the value of b that was the quickest way after using the value of a before,
            //therefore we use the value of b to find the next route.
            var openList = new List<PathNode>();
            
            //closed list contain all the node we have already serched
            var closedList = new List<PathNode>();

            var startNode = gridSystem.GetGridObject(startGridPosition);
            var endNode = gridSystem.GetGridObject(endGridPosition);
            
            openList.Add(startNode);
            
            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
            startNode.CalculateFCost();

            // set up var all 
            for (int x = 0; x < gridSystem.GetWidth(); x++)
            {
                for (int z = 0; z < gridSystem.GetHeight(); z++)
                {
                    var pathNode = gridSystem.GetGridObject(new GridPosition(x, z));

                    pathNode.SetUpNode();

                }
            }
            
            while (openList.Count > 0)
            {
                var currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    // reached final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbourList in GetNeighbourList(currentNode))
                {
                    // already searched
                    if (closedList.Contains(neighbourList))
                    {
                        continue;
                    }

                    var distance = CalculateDistance(currentNode.GetGridPosition(), neighbourList.GetGridPosition());
                    var tentativeGCost = currentNode.GetGCost() + distance;

                    // check is found the best path
                    if (tentativeGCost < neighbourList.GetGCost())
                    {
                        neighbourList.SetCameFromPathNode(currentNode);
                        neighbourList.SetGCost(tentativeGCost);
                        neighbourList.SetHCost(CalculateDistance(currentNode.GetGridPosition(), endNode.GetGridPosition()));
                        neighbourList.CalculateFCost();

                        if (!openList.Contains(neighbourList))
                        {
                            openList.Add(neighbourList);
                        }
                    }
                }
            }
            // no path found 
            return null;
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

            // set is have node
            if (gridPosition.z + 1 < gridSystem.GetWidth())
            {
                var upNode = GetNode(gridPosition.x, gridPosition.z + 1);    
                neighbourList.Add(upNode);
            }

            if (gridPosition.z - 1 >= 0)
            {
                var downNode = GetNode(gridPosition.x, gridPosition.z - 1);  
                neighbourList.Add(downNode);
            }

            if (gridPosition.x - 1 >= 0)
            {
                var leftNode = GetNode(gridPosition.x - 1, gridPosition.z);
                neighbourList.Add(leftNode);
                
                if (gridPosition.z - 1 >= 0)
                {
                    var leftDownNode = GetNode(gridPosition.x - 1, gridPosition.z - 1);
                    neighbourList.Add(leftDownNode);
                }

                if (gridPosition.z + 1 < gridSystem.GetWidth())
                {
                    var leftUpNode = GetNode(gridPosition.x - 1, gridPosition.z + 1);
                    neighbourList.Add(leftUpNode);
                }
            }

            if (gridPosition.x + 1 < gridSystem.GetWidth())
            {
                var rightNode = GetNode(gridPosition.x + 1, gridPosition.z);
                neighbourList.Add(rightNode);
                
                if (gridPosition.z - 1 >= 0)
                {
                    var rightDownNode = GetNode(gridPosition.x + 1, gridPosition.z - 1);
                    neighbourList.Add(rightDownNode);
                }

                if (gridPosition.z + 1 < gridSystem.GetWidth())
                {
                    var rightUpNode = GetNode(gridPosition.x + 1, gridPosition.z + 1);
                    neighbourList.Add(rightUpNode);
                }
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
    }
}