using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BrushPreset))]
public class BrushPresetConfigEditor : Editor
{
    BrushPreset _target;
    List<bool> removeFromList = new List<bool>();

    void OnEnable()
    {
        _target = (BrushPreset)target;
    }

    public override void OnInspectorGUI()
    {
        _target.BurstQuantity = EditorGUILayout.IntField("Burst Quantity", _target.BurstQuantity);
        if(_target.BurstQuantity <= 0) { _target.BurstQuantity = 1; }
        EditorGUILayout.LabelField("----------------------------------------");
        _target.RandomRotation = EditorGUILayout.Toggle("Random Rotation", _target.RandomRotation);
        _target.RandomXRotation = EditorGUILayout.Slider("X Rotation", _target.RandomXRotation, 0f, 360f);
        _target.RandomYRotation = EditorGUILayout.Slider("Y Rotation", _target.RandomYRotation, 0f, 360f);
        _target.RandomZRotation = EditorGUILayout.Slider("Z Rotation", _target.RandomZRotation, 0f, 360f);
        EditorGUILayout.LabelField("----------------------------------------");
		_target.Spread = EditorGUILayout.FloatField ("Spacing", _target.Spread);
		if(_target.Spread < 0)  _target.Spread = 0;
        EditorGUILayout.LabelField("----------------------------------------");
        _target.Spacing = EditorGUILayout.FloatField("Spacing", _target.Spacing);
        if(_target.Spacing < 0.25f) { _target.Spacing = 0.25f; }
        EditorGUILayout.LabelField("----------------------------------------");
        EditorGUILayout.LabelField("Painting Objects");
        for (int i = 0; i < _target.paintingObjs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _target.paintingObjs[i] = (GameObject)EditorGUILayout.ObjectField(_target.paintingObjs[i], typeof(GameObject), true, GUILayout.Width(100));
            if (GUILayout.Button("Remove"))
            {
                removeFromList[i] = true;
            }
            EditorGUILayout.EndHorizontal();
        }
        for (int i = removeFromList.Count - 1; i >= 0; i--)
        {
            if (removeFromList[i])
            {
                _target.paintingObjs.RemoveAt(i);
                removeFromList.RemoveAt(i);
            }
        }
        var objToAdd = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), true);
        if (objToAdd != null)
        {
            _target.paintingObjs.Add(objToAdd);
            removeFromList.Add(false);
        }
        Repaint();
    }
}
