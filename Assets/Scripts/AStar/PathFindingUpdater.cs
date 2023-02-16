using System;
using Destructible;
using UnityEngine;

namespace AStar
{
    public class PathFindingUpdater : MonoBehaviour
    {
        private void Start()
        {
            DestructibleCrate.OnAnyDestroy += DestructibleCrate_OnAnyDestroy;
        }

        private void DestructibleCrate_OnAnyDestroy(object sender, EventArgs e)
        {
            var destructibleCrate = sender as DestructibleCrate;
            
            if (destructibleCrate)
            {
                PathFinding.Instance.SetWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
            }
        }
    }
}