using System;
using Grid;
using UnityEngine;

namespace Destructible
{
    public class DestructibleCrate : MonoBehaviour
    {
        public static event EventHandler OnAnyDestroy;

        private GridPosition gridPosition;

        public void Start()
        { 
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }


        public void Damage()
        {
            Destroy(gameObject);

            OnAnyDestroy?.Invoke(this, EventArgs.Empty);
        }
    }
}