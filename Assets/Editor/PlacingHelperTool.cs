using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Linq;

public class PlacingHelperTool : EditorWindow{


	//GameObjects
	List<GameObject> placingGO = new List<GameObject> ();
	List<GameObject> goList = new List<GameObject>();
	List<bool> paintBoolList = new List<bool> ();
	AnimBool gameObjectsFoldoutBool = new AnimBool();

	//Placing
	AnimBool placingFoldoutBool = new AnimBool();
	bool randomRotationX;
	bool randomRotationY;
	bool randomRotationZ;
	bool randomScaleX;
	bool randomScaleY;
	bool randomScaleZ;
	int paintingMask;


	//Painting
	bool clickNAdd;
	bool clickNDrag;

    //Brushes
    AnimBool brushesFoldoutBool = new AnimBool();
    string nameTField;
    BrushPreset currentBrush;

	Vector2 scroll;

    [MenuItem("Window/Placing Helper")]
	static void CreateWindow(){
		GetWindow<PlacingHelperTool> ().Show ();
	}
	
	void OnEnable(){
		gameObjectsFoldoutBool = new AnimBool ();
		gameObjectsFoldoutBool.valueChanged.AddListener (Repaint);
		placingFoldoutBool = new AnimBool ();
		placingFoldoutBool.valueChanged.AddListener (Repaint);
        brushesFoldoutBool = new AnimBool();
        brushesFoldoutBool.valueChanged.AddListener (Repaint);

        //Esto hace que cada vez que se dispare un evento en la ventana Scene, se llame al metodo OnSceneGUI
        SceneView.onSceneGUIDelegate += OnSceneGUI;

		currentBrush = AssetDatabase.LoadAssetAtPath<ToolConfig> ("Assets/Editor/Config.asset").CurrentBrush;
	}

    void OnGUI(){
		EditorGUILayout.LabelField ("Helper", EditorStyles.helpBox);

		EditorGUILayout.BeginVertical ();
		scroll = EditorGUILayout.BeginScrollView (scroll, false, true, GUILayout.Height (position.height - 25), GUILayout.Width(position.width));
        /*//GameObjects Stuff
		gameObjectsFoldoutBool.target = EditorGUILayout.Foldout (gameObjectsFoldoutBool.target, "Game objects");
		if(EditorGUILayout.BeginFadeGroup (gameObjectsFoldoutBool.faded)){
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("",GUILayout.Width(position.width / 3));
			EditorGUILayout.LabelField ("Paint");
			EditorGUILayout.EndHorizontal ();
			var removeGo = new bool[goList.Count];
			for (int i = 0; i < goList.Count; i++) {
				removeGo [i] = false;
				EditorGUILayout.BeginHorizontal ();
				goList [i] = (GameObject)EditorGUILayout.ObjectField (goList [i], typeof(GameObject), true, GUILayout.Width(position.width / 3));
				var newBool = EditorGUILayout.Toggle(paintBoolList[i], GUILayout.Width(25));
				if (!paintBoolList [i] && newBool){
					placingGO.Add (goList [i]);
				} else if(paintBoolList[i] && !newBool){
					placingGO.Remove (goList [i]);
				}
				paintBoolList [i] = newBool;
				if (GUILayout.Button ("Remove"))
					removeGo [i] = true;
				EditorGUILayout.EndHorizontal ();
			}
			for (int i = removeGo.Length - 1; i >= 0; i--) {
				if (removeGo [i])
					goList.RemoveAt (i);
			}
			var newgo = (GameObject)EditorGUILayout.ObjectField (null, typeof(GameObject), true);
			if(newgo != null){
				goList.Add (newgo);
				paintBoolList.Add (false);
			}
		}
		EditorGUILayout.EndFadeGroup ();

        //Placing Stuff
		placingFoldoutBool.target = EditorGUILayout.Foldout (placingFoldoutBool.target, "Placing");
		if(EditorGUILayout.BeginFadeGroup(placingFoldoutBool.faded)){  EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("", GUILayout.Width(position.width / 2));
			EditorGUILayout.LabelField("X", GUILayout.Width(25));
			EditorGUILayout.LabelField("Y", GUILayout.Width(25));
			EditorGUILayout.LabelField("Z", GUILayout.Width(25));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Random Rotation", GUILayout.Width(position.width / 2));
			randomRotationX = EditorGUILayout.Toggle(randomRotationX, GUILayout.Width(25));
			randomRotationY = EditorGUILayout.Toggle(randomRotationY, GUILayout.Width(25));
			randomRotationZ = EditorGUILayout.Toggle(randomRotationZ, GUILayout.Width(25));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Random Scale", GUILayout.Width(position.width / 2));
			randomScaleX = EditorGUILayout.Toggle(randomScaleX, GUILayout.Width(25));
			randomScaleY = EditorGUILayout.Toggle(randomScaleY, GUILayout.Width(25));
			randomScaleZ = EditorGUILayout.Toggle(randomScaleZ, GUILayout.Width(25));
			EditorGUILayout.EndHorizontal();

		}
		EditorGUILayout.EndFadeGroup ();*/

        //Brushes
        brushesFoldoutBool.target = EditorGUILayout.Foldout(brushesFoldoutBool.target, "Brushes");
		if (EditorGUILayout.BeginFadeGroup (brushesFoldoutBool.faded)) {
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("New Brush")) {
				CreateBrushWindow.CreateBrush ();
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.LabelField ("-------------------------------------------");
			currentBrush = (BrushPreset)EditorGUILayout.ObjectField (currentBrush, typeof(BrushPreset), false);
        

			currentBrush.burstQuantity = EditorGUILayout.IntField ("Burst Quantity", currentBrush.burstQuantity);
			if (currentBrush.burstQuantity <= 0) {
				currentBrush.burstQuantity = 1;
			}
			EditorGUILayout.LabelField ("----------------------------------------");
			currentBrush.randomRotation = EditorGUILayout.Toggle ("Random Rotation", currentBrush.randomRotation);
			currentBrush.randomXRotation = EditorGUILayout.Slider ("X Rotation", currentBrush.randomXRotation, 0f, 360f);
			currentBrush.randomYRotation = EditorGUILayout.Slider ("Y Rotation", currentBrush.randomYRotation, 0f, 360f);
			currentBrush.randomZRotation = EditorGUILayout.Slider ("Z Rotation", currentBrush.randomZRotation, 0f, 360f);
			EditorGUILayout.LabelField ("----------------------------------------");
			currentBrush.randomXOffset = EditorGUILayout.FloatField ("X Offset", currentBrush.randomXOffset);
			if (currentBrush.randomXOffset < 0f) {
				currentBrush.randomXOffset = 0f;
			}
			currentBrush.randomYOffset = EditorGUILayout.FloatField ("Y Offset", currentBrush.randomYOffset);
			if (currentBrush.randomYOffset < 0f) {
				currentBrush.randomYOffset = 0f;
			}
			currentBrush.randomZOffset = EditorGUILayout.FloatField ("Z Offset", currentBrush.randomZOffset);
			if (currentBrush.randomZOffset < 0f) {
				currentBrush.randomZOffset = 0f;
			}
			EditorGUILayout.LabelField ("----------------------------------------");
			currentBrush.spacing = EditorGUILayout.FloatField ("Spacing", currentBrush.spacing);
			if (currentBrush.spacing < 0.25f) {
				currentBrush.spacing = 0.25f;
			}
			EditorGUILayout.LabelField ("----------------------------------------");
			EditorGUILayout.LabelField ("Painting Objects");
			var remPlz = new List<bool> ();
			for (int i = 0; i < currentBrush.paintingObjs.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				currentBrush.paintingObjs [i] = (GameObject)EditorGUILayout.ObjectField (currentBrush.paintingObjs [i], typeof(GameObject), true, GUILayout.Width (100));
				remPlz [i] = GUILayout.Button ("Remove");
				EditorGUILayout.EndHorizontal ();
			}
			for (int i = remPlz.Count - 1; i >= 0; i--) {
				if (remPlz [i]) {
					currentBrush.paintingObjs.RemoveAt (i);
					remPlz.RemoveAt (i);
				}
			}
			var objToAdd = (GameObject)EditorGUILayout.ObjectField (null, typeof(GameObject), true);
			if (objToAdd != null) {
				currentBrush.paintingObjs.Add (objToAdd);
				remPlz.Add (false);
			}
		}
        EditorGUILayout.EndFadeGroup();

		string[] layers = new string[32]; //32 = maxLayers
		for (int i = 0; i < 32; i++) {
			layers[i] = LayerMask.LayerToName (i);
		}
		paintingMask = EditorGUILayout.MaskField ("Painting Layer", paintingMask,layers.Reverse().SkipWhile(x => x == "").Reverse().ToArray(), GUILayout.Width(position.width - 25));


        //Them buttons stuff
		EditorGUILayout.BeginHorizontal ();
		GUI.color = clickNAdd? Color.green: Color.white;
		if (GUILayout.Button(clickNAdd ? "Stop painting" : "Click Paint", GUILayout.Width(position.width / 2 - 15))){
			clickNAdd = !clickNAdd;
			clickNDrag = false;
		}
		GUI.color = clickNDrag ? Color.green : Color.white;
		if (GUILayout.Button (clickNDrag ? "Stop painting" : "Click n' Drag", GUILayout.Width(position.width / 2 - 15))) {
			clickNDrag = !clickNDrag;
			clickNAdd = false;
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

    void OnSceneGUI(SceneView sceneView)
    {
		Event c = Event.current;

		if (clickNAdd && c.type == EventType.MouseDown && c.button == 0 && currentBrush != null) {
			Ray mouseRay = HandleUtility.GUIPointToWorldRay (c.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast (mouseRay, out hitInfo, 10000)) {
				for (int i = 0; i < currentBrush.burstQuantity; i++) {
					GameObject instObj = (GameObject)PrefabUtility.InstantiatePrefab (currentBrush.paintingObjs [Random.Range (0, currentBrush.paintingObjs.Count)]);
					instObj.transform.position = hitInfo.point + new Vector3 (
						Random.Range (-currentBrush.randomXOffset, currentBrush.randomXOffset),
						Random.Range (-currentBrush.randomYOffset, currentBrush.randomYOffset),
						Random.Range (-currentBrush.randomZOffset, currentBrush.randomZOffset));
					if (currentBrush.randomRotation) {
						instObj.transform.rotation = new Quaternion (
							Random.Range (-currentBrush.randomXRotation, currentBrush.randomXRotation),
							Random.Range (-currentBrush.randomYRotation, currentBrush.randomYRotation),
							Random.Range (-currentBrush.randomZRotation, currentBrush.randomZRotation), 0);
					}
					EditorUtility.SetDirty (instObj);
				}     
				c.Use ();
			}
		}
		if (clickNDrag) {
			HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
			if (c.type == EventType.mouseDown && c.button == 0)
				mouseDown = true;
			else if (c.type == EventType.mouseUp && c.button == 0)
				mouseDown = false;
			if(mouseDown){
				RaycastHit rc;
				if(Physics.Raycast(HandleUtility.GUIPointToWorldRay(c.mousePosition), out rc, 10000,paintingMask)){
					distAcumSqrd += Vector3.SqrMagnitude (lastMousePos - rc.point);
					lastMousePos = rc.point;
					if(distAcumSqrd >= currentBrush.spacing.Sqr()){		//Se cumple la distancia del brush??
						distAcumSqrd = 0;
						for (int i = 0; i < currentBrush.burstQuantity; i++)
						{
							GameObject instObj = (GameObject)PrefabUtility.InstantiatePrefab(currentBrush.paintingObjs[Random.Range(0, currentBrush.paintingObjs.Count)]);
							instObj.transform.position = rc.point + new Vector3 (
								Random.Range (-currentBrush.randomXOffset, currentBrush.randomXOffset),
								Random.Range (-currentBrush.randomYOffset, currentBrush.randomYOffset),
								Random.Range (-currentBrush.randomZOffset, currentBrush.randomZOffset));
							if(currentBrush.randomRotation)
							{
								instObj.transform.rotation = new Quaternion (
									Random.Range (-currentBrush.randomXRotation, currentBrush.randomXRotation),
									Random.Range (-currentBrush.randomYRotation, currentBrush.randomYRotation),
									Random.Range (-currentBrush.randomZRotation, currentBrush.randomZRotation), 0);
							}
							EditorUtility.SetDirty(instObj);
						}   
					}
				}
			}
		}
	}

	//Drag'n'Paint vars
	bool mouseDown;
	Vector3 lastMousePos;
	float distAcumSqrd;

	Vector3 GetMousePosInWorld(Vector2 mousePos) {
		return Vector3.zero;
	}
	Vector3 GetMouseNormal(Vector2 mousePos) {
		return Vector3.zero;
	}
    
	void OnDisable()
    {
        //Esto remueve el metodo OnSceneGui de la lista de eventos del SceneView.
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

		gameObjectsFoldoutBool.valueChanged.RemoveListener (Repaint);
		placingFoldoutBool.valueChanged.RemoveListener (Repaint);
        brushesFoldoutBool.valueChanged.RemoveListener(Repaint);
    }

	ToolConfig conf;

	void OnDestroy() {
		if (currentBrush != null) {
			var configAsset = AssetDatabase.LoadAssetAtPath<ToolConfig> ("Assets/Editor/Config.asset");
			if (configAsset == null) {
				conf = ScriptableObject.CreateInstance<ToolConfig> ();
				conf.CurrentBrush = currentBrush;
				AssetDatabase.CreateAsset (conf, "Assets/Editor/Config.asset");
			} else {
				configAsset.CurrentBrush = currentBrush;
				AssetDatabase.SaveAssets ();
			}
		}
	}
}
