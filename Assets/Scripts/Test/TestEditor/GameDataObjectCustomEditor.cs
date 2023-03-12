using UnityEditor;
using UnityEngine;

namespace Test.TestEditor
{
    
    [CustomEditor(typeof(GameDataObject))]
    public class GameDataObjectCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var gameDataObject = (GameDataObject)target;
            if (GUILayout.Button("Open Editor"))
            {
                GameDataObjectEditorWindow.Open(gameDataObject);
            }
        }
    }
}