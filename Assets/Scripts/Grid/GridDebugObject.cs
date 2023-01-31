using System;
using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text gridPositionText;
        private object gridObject;

        public virtual void SetGridObject(object gObject)
        {
           gridObject = gObject;
           SetDebugText();
        }

        public virtual void SetDebugText()
        {
            gridPositionText.text = gridObject.ToString();
        }
    }
}