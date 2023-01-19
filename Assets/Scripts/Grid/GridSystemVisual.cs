using System;
using System.Collections.Generic;
using System.Linq;
using UnitAction;
using UnitClass;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Grid
{
    public class GridSystemVisual : Singleton<GridSystemVisual>
    {
        [SerializeField] private GridSystemVisualSingle gridSystemVisualSinglePrefab;
        [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
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
            
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;
        }

        private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, EventArgs e)
        {
            UpdateGridVisual();
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
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

        public void ShowGridPositionList(List<GridPosition> gridPositionsList, GridVisualType gridVisualType)
        {
            var material = GetGridVisualTypeMaterial(gridVisualType);
            foreach (var gridPosition in gridPositionsList)
            {
                gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(material);
            }
        }

        public void UpdateGridVisual()
        {
            HideAllGridPosition();

            var selectedAction = UnitActionSystem.Instance.GetSelectAction();

            if (selectedAction != null)
            {
                var gridVisualType = GridVisualType.White;
                
                var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

                switch (selectedAction)
                {
                    default:
                    case MoveAction moveAction:
                        gridVisualType = GridVisualType.White;
                        break;
                    case SpinAction spinAction:
                        gridVisualType = GridVisualType.Blue;
                        break;
                    case ShootAction shootAction:
                        gridVisualType = GridVisualType.Red;
                        ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), 
                            GridVisualType.RedSoft);
                        break;
                }
                
                ShowGridPositionList(selectedAction.GetValidActionGridPositionsList(), gridVisualType);
            }
        }

        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();

            for (var x = -range; x <= range; x++)
            {
                for (var z = -range; z <= range; z++)
                {
                    var testGridPosition = gridPosition + new GridPosition(x, z);
                    
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }
                    
                    var testDistance = math.abs(x) + math.abs(z);
                    if (testDistance > range)
                    {
                        continue;
                    }

                    gridPositionList.Add(testGridPosition);
                }
            }
            
            ShowGridPositionList(gridPositionList, gridVisualType);
        }

        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            return gridVisualTypeMaterialList.FirstOrDefault(T => T.gridVisualType == gridVisualType).material;
        }
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft,
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

}