using System;
using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        private GridObject gridObject;

        public void SetGridPosition(GridObject grid)
        {
           gridObject = grid;
           SetDebugText();
        }

        public void SetDebugText()
        {
            tmpText.text = gridObject.ToString();
        }
    }
}