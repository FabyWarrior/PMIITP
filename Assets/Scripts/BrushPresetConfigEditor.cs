using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BrushPreset))]
public class BrushPresetConfigEditor : Editor
{
    private BrushPreset _target;
    private List<bool> removeFromList = new List<bool>();

    void OnEnable()
    {
        _target = (BrushPreset)target;
    }

    public override void OnInspectorGUI()
    {
        _target.burstQuantity = EditorGUILayout.IntField("Burst Quantity", _target.burstQuantity);
        if(_target.burstQuantity <= 0) { _target.burstQuantity = 1; }
        EditorGUILayout.LabelField("----------------------------------------");
        _target.randomRotation = EditorGUILayout.Toggle("Random Rotation", _target.randomRotation);
        _target.randomXRotation = EditorGUILayout.Slider("X Rotation", _target.randomXRotation, 0f, 360f);
        _target.randomYRotation = EditorGUILayout.Slider("Y Rotation", _target.randomYRotation, 0f, 360f);
        _target.randomZRotation = EditorGUILayout.Slider("Z Rotation", _target.randomZRotation, 0f, 360f);
        EditorGUILayout.LabelField("----------------------------------------");
        _target.randomXOffset = EditorGUILayout.FloatField("X Offset", _target.randomXOffset);
        if (_target.randomXOffset < 0f) { _target.randomXOffset = 0f; }
        _target.randomYOffset = EditorGUILayout.FloatField("Y Offset", _target.randomYOffset);
        if (_target.randomYOffset < 0f) { _target.randomYOffset = 0f; }
        _target.randomZOffset = EditorGUILayout.FloatField("Z Offset", _target.randomZOffset);
        if (_target.randomZOffset < 0f) { _target.randomZOffset = 0f; }
        EditorGUILayout.LabelField("----------------------------------------");
        _target.spacing = EditorGUILayout.FloatField("Spacing", _target.spacing);
        if(_target.spacing < 0.25f) { _target.spacing = 0.25f; }
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
