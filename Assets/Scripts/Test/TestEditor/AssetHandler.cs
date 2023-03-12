using UnityEditor;
using UnityEditor.Callbacks;

namespace Test.TestEditor
{
    /// <summary>
    /// using double click to open window editor 
    /// </summary>
    public class AssetHandler
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameDataObject;
            if (obj)
            {
                GameDataObjectEditorWindow.Open(obj);
                return true;
            }

            return false;
        }
    }
}