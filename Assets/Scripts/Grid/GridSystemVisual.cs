using System;
using System.Collections.Generic;
using UnitClass;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Grid
{
    public class GridSystemVisual : Singleton<GridSystemVisual>
    {
        [SerializeField] private GridSystemVisualSingle gridSystemVisualSinglePrefab;
        private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

        public void Start()
        {
            var  levelGrid = LevelGrid.Instance;
            gridSystemVisualSingleArray = new GridSystemVisualSingle
            [
                levelGrid.GetWidth(),
                levelGrid.GetHeight()
            ];
            
            for (var x = 0; x < levelGrid.GetWidth(); x++)
            {
                for (var z = 0; z < levelGrid.GetHeight(); z++)
                {
                    var girdPosition = new GridPosition(x, z);
                    var instantiate = Instantiate(gridSystemVisualSinglePrefab, 
                        levelGrid.GetWordPosition(girdPosition), quaternion.identity, transform);
                    gridSystemVisualSingleArray[x, z] = instantiate;
                }
            }
        }

        public void Update()
        {
            UpdateGridVisual();
        }

        public void HideAllGridPosition()
        {
            foreach (var gridSystemVisualSingle in gridSystemVisualSingleArray)
            {
                gridSystemVisualSingle.Close();
            }
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionsList)
        {
            foreach (var gridPosition in gridPositionsList)
            {
                gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
            }
        }

        public void UpdateGridVisual()
        {
            HideAllGridPosition();

            var selectedAction = UnitActionSystem.Instance.GetSelectAction();

            if (selectedAction != null)
            {
                ShowGridPositionList(selectedAction.GetValidActionGridPositionsList());
            }
        }
    }
    
}