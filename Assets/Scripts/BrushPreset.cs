using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrushPreset : ScriptableObject
{
    public List<GameObject> paintingObjs;
	public string Name;
    public int BurstQuantity;
    public float Spacing;
    public float Spread;
    public bool RandomRotation;
    [Range(0f, 360f)] public float RandomXRotation;
    [Range(0f, 360f)] public float RandomYRotation;
    [Range(0f, 360f)] public float RandomZRotation;

	public BrushPreset(){
		paintingObjs = new List<GameObject> ();
		Name = "";
	}

}
