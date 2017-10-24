using System.IO;
using UnityEditor;
using UnityEngine;


public static class BrushPresetFactory
{
    public static void CreateBrush<T>(string name) where T : ScriptableObject
    {
        T obj = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Editor/Brushes/" + name.ToString() + "Brush" + ".asset");
        AssetDatabase.CreateAsset(obj, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = obj;
    }
}
