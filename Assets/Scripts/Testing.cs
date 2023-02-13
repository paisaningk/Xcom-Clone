using System;
using System.Collections;
using System.Collections.Generic;
using AStar;
using Grid;
using UnitClass;
using UnityEngine;
using Utils;

public class Testing : MonoBehaviour
{

    private void Update()
   {
       if (Input.GetKeyDown(KeyCode.T))
       {
           var levelGrid = LevelGrid.Instance;
           var mouseGridPosition = levelGrid.GetGridPosition(MousePosition.Instance.GetPointPosition());
           var startGridPosition = new GridPosition(0, 0);

           var gridPositionList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition).gridPositionList;

           for (var i = 0; i < gridPositionList.Count; i++)
           {
               var start = levelGrid.GetWorldPosition(gridPositionList[i]) + new Vector3(0, 1, 0);
               var end = levelGrid.GetWorldPosition(gridPositionList[i + 1]) + new Vector3(0, 1, 0);
               Debug.DrawLine(start, end, Color.green, 20f);
           }
       }
   }
}
