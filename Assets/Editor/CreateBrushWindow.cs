using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateBrushWindow : EditorWindow {

	BrushPreset newBrush;
	List<bool> removeFromList;
	Vector2 scroll;

	public static void CreateBrush(){
		GetWindow<CreateBrushWindow> ().Show();
	}

	void OnEnable(){
		newBrush = ScriptableObject.CreateInstance<BrushPreset> ();
		removeFromList = new List<bool> ();
	}

	void OnGUI(){
		scroll = EditorGUILayout.BeginScrollView (scroll, false, false);
		newBrush.Name = EditorGUILayout.TextField ("Name: ", newBrush.Name);
		newBrush.burstQuantity = EditorGUILayout.IntField("Burst Quantity", newBrush.burstQuantity);
		if(newBrush.burstQuantity <= 0) { newBrush.burstQuantity = 1; }
		EditorGUILayout.LabelField("----------------------------------------");
		newBrush.randomRotation = EditorGUILayout.Toggle("Random Rotation", newBrush.randomRotation);
		newBrush.randomXRotation = EditorGUILayout.Slider("X Rotation", newBrush.randomXRotation, 0f, 360f);
		newBrush.randomYRotation = EditorGUILayout.Slider("Y Rotation", newBrush.randomYRotation, 0f, 360f);
		newBrush.randomZRotation = EditorGUILayout.Slider("Z Rotation", newBrush.randomZRotation, 0f, 360f);
		EditorGUILayout.LabelField("----------------------------------------");
		newBrush.randomXOffset = EditorGUILayout.FloatField("X Offset", newBrush.randomXOffset);
		if (newBrush.randomXOffset < 0f) { newBrush.randomXOffset = 0f; }
		newBrush.randomYOffset = EditorGUILayout.FloatField("Y Offset", newBrush.randomYOffset);
		if (newBrush.randomYOffset < 0f) { newBrush.randomYOffset = 0f; }
		newBrush.randomZOffset = EditorGUILayout.FloatField("Z Offset", newBrush.randomZOffset);
		if (newBrush.randomZOffset < 0f) { newBrush.randomZOffset = 0f; }
		EditorGUILayout.LabelField("----------------------------------------");
		newBrush.spacing = EditorGUILayout.FloatField("Spacing", newBrush.spacing);
		if(newBrush.spacing < 0.25f) { newBrush.spacing = 0.25f; }
		EditorGUILayout.LabelField("----------------------------------------");
		EditorGUILayout.LabelField("Painting Objects");

		for (int i = 0; i < newBrush.paintingObjs.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			newBrush.paintingObjs[i] = (GameObject)EditorGUILayout.ObjectField(newBrush.paintingObjs[i], typeof(GameObject), true, GUILayout.Width(100));
			removeFromList [i] = GUILayout.Button ("Remove");
			EditorGUILayout.EndHorizontal();
		}
		for (int i = removeFromList.Count - 1; i >= 0; i--) {
			if (removeFromList[i]) {
				newBrush.paintingObjs.RemoveAt(i);
				removeFromList.RemoveAt(i);
			}
		}
		var objToAdd = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), true);
		if (objToAdd != null) {
			newBrush.paintingObjs.Add(objToAdd);
			removeFromList.Add(false);
		}
		Repaint();

		EditorGUILayout.BeginHorizontal ();

		GUI.color = newBrush.Name == "" ? Color.grey : Color.white;
		if(GUILayout.Button(new GUIContent("Create", newBrush.Name == "" ? "Write a Name": "Creates the brush")) && newBrush.Name != "" ){
			AssetDatabase.CreateAsset (newBrush, "Assets/Editor/Brushes/" + newBrush.Name + ".Asset");
			Close ();
		}
		GUI.color = Color.Lerp (Color.red, Color.white, .5f);
		if(GUILayout.Button("Cancel")){
			Close ();
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndScrollView ();
	}

	void OnDestroy(){
		Selection.activeObject = AssetDatabase.LoadAssetAtPath ("Assets/Editor/Brushes/" + newBrush.Name + ".Asset", typeof(Object));
		newBrush = null;
		removeFromList = null;
	}

}
