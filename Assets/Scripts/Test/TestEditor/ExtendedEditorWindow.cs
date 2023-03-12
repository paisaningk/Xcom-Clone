using UnityEditor;
using UnityEngine;

namespace Test.TestEditor
{
    public class ExtendedEditorWindow : EditorWindow
    {
        [Header("Value for set up window")]
        protected SerializedObject serializedObject;
        protected SerializedProperty currentProperty;

        [Header("Check what is we selected")]
        private string selectedPropertyPath;
        protected SerializedProperty selectedProperty;
        
        /// <summary>
        /// show all data from property 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="drawChildren">show child</param>
        protected void DrawProperties(SerializedProperty property, bool drawChildren)
        {
            var lastPropPath = string.Empty;
            
            foreach (SerializedProperty variable in property)
            {
                if (variable.isArray && variable.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    variable.isExpanded = EditorGUILayout.Foldout(variable.isExpanded, variable.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (variable.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(variable, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && variable.propertyPath.Contains(lastPropPath))
                    {
                        continue;
                    }

                    lastPropPath = variable.propertyPath;
                    EditorGUILayout.PropertyField(variable, drawChildren);
                }
            }
        }

        /// <summary>
        /// show button left side 
        /// </summary>
        /// <param name="prop">what prop we need to show</param>
        protected void DrawSideBarButton(SerializedProperty prop)
        {
            
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.displayName))
                {
                    selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            }
        }

        /// <summary>
        /// show data follow property name 
        /// </summary>
        /// <param name="propName">Property name i want to show data</param>
        /// <param name="relative">is it a relative ex. class a.adc</param>
        protected void DrawField(string propName, bool relative)
        {
            if (relative && currentProperty != null)
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propName), true);
            }
            else if (serializedObject != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), true);        
            }
        }

        /// <summary>
        /// apply value we chane 
        /// </summary>
        protected void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}