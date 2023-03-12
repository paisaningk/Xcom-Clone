using System;
using UnityEditor;
using UnityEngine;

namespace Test.TestEditor
{
    public class GameDataObjectEditorWindow : ExtendedEditorWindow
    {
        private bool isTest;
        public static void Open(GameDataObject gameDataObject)
        {
            var window = GetWindow<GameDataObjectEditorWindow>("Game Data Editor");
            window.serializedObject = new SerializedObject(gameDataObject);
        }

        public void OnGUI()
        {
            currentProperty = serializedObject.FindProperty("myTest");
            
            Draw(currentProperty);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxHeight(150), GUILayout.Width(50));
            
            DrawSideBarButton(currentProperty);
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box",  GUILayout.ExpandHeight(this));

            if (selectedProperty != null)
            {
                DrawSelectPropertiesPaned();
            }
            else
            {
                EditorGUILayout.LabelField($"Select an item from the list");
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            Apply();

        }

        private void DrawSelectPropertiesPaned()
        {
            currentProperty = selectedProperty;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("true", EditorStyles.toolbarButton))
            {
                isTest = true;
            }
            
            if (GUILayout.Button("false", EditorStyles.toolbarButton))
            {
                isTest = false;
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical("box");

            if (isTest)
            {
                DrawField("name", true);
            }

            DrawField("b", true);
            DrawField("c", true);

            EditorGUILayout.EndHorizontal(); 

        }

        private void Draw(SerializedProperty serializedProperty)
        {
            EditorGUILayout.BeginHorizontal();

            Show(serializedProperty);
            
            if (GUILayout.Button("Add New Item")) 
            {
                serializedProperty.arraySize++;
            }

            if (GUILayout.Button("Del Item")) 
            {
                serializedProperty.arraySize--;
            }
            
            EditorGUILayout.EndHorizontal();
        }


        private static void Show (SerializedProperty list) 
        {
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
        }
    }
}