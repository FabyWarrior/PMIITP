using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrushPreset : ScriptableObject
{
    public List<GameObject> paintingObjs;
    public int burstQuantity;
    public bool burstMode;
    public float spacing;
    public float randomXOffset;
    public float randomYOffset;
    public float randomZOffset;
    public bool randomRotation;
    [Range(0f, 360f)] public float randomXRotation;
    [Range(0f, 360f)] public float randomYRotation;
    [Range(0f, 360f)] public float randomZRotation;
}
