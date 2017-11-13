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
		newBrush.BurstQuantity = EditorGUILayout.IntField("Burst Quantity", newBrush.BurstQuantity);
		if(newBrush.BurstQuantity <= 0)  newBrush.BurstQuantity = 1; 
		EditorGUILayout.LabelField("----------------------------------------");
		newBrush.RandomRotation = EditorGUILayout.Toggle("Random Rotation", newBrush.RandomRotation);
		newBrush.RandomXRotation = EditorGUILayout.Slider("X Rotation", newBrush.RandomXRotation, 0f, 360f);
		newBrush.RandomYRotation = EditorGUILayout.Slider("Y Rotation", newBrush.RandomYRotation, 0f, 360f);
		newBrush.RandomZRotation = EditorGUILayout.Slider("Z Rotation", newBrush.RandomZRotation, 0f, 360f);
		EditorGUILayout.LabelField("----------------------------------------");
		newBrush.Spread = EditorGUILayout.FloatField ("Spread", newBrush.Spread);
		if (newBrush.Spread < 0f) { newBrush.Spread = 0f; }
		EditorGUILayout.LabelField("----------------------------------------");
		newBrush.Spacing = EditorGUILayout.FloatField("Spacing", newBrush.Spacing);
		if(newBrush.Spacing < 0.25f)  newBrush.Spacing = 0.25f; 
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
