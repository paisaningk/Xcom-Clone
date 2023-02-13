using System;
using Grid;
using TMPro;
using UnityEngine;

namespace AStar
{
    public class PathFindingGridDebugObject : GridDebugObject
    {
        [SerializeField] private TMP_Text gCostText;
        [SerializeField] private TMP_Text hCostText;
        [SerializeField] private TMP_Text fCostText;
        [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

        private PathNode pathNode;

        public override void SetGridObject(object grid)
        {
            pathNode = (PathNode)grid;
            base.SetGridObject(grid);
        }
        
        public override void SetDebugText()
        {
            base.SetDebugText();
            gCostText.SetText($"G:{pathNode.GetGCost()}");
            hCostText.SetText($"H:{pathNode.GetHCost()}");
            fCostText.SetText($"F:{pathNode.GetFCost()}");
            isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.grey : Color.red;
        }
    }
}