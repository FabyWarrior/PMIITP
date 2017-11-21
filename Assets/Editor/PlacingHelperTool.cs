using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Linq;

public class PlacingHelperTool : EditorWindow{

	//Go
	static List<GameObject> paintedGO;

	//GameObjects
	List<GameObject> placingGO = new List<GameObject> ();
	List<GameObject> goList = new List<GameObject>();
	List<bool> paintBoolList = new List<bool> ();
	AnimBool gameObjectsFoldoutBool = new AnimBool();

	//Placing
	AnimBool placingFoldoutBool = new AnimBool();
	int paintingMask;


	//Painting
	bool clickNAdd;
	bool clickNDrag;
	bool erasing;

    //Brushes
    AnimBool brushesFoldoutBool = new AnimBool();
    BrushPreset currentBrush;

	Vector2 scroll;

    //Styles
    bool clickNewBrush;
    GUIStyle uiStyle = new GUIStyle(EditorStyles.helpBox);
    GUIStyle uiTextStyle = new GUIStyle(EditorStyles.label);
    GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);
    Texture2D uiTexture = new Texture2D(1, 1);
    Color buttonColorNormal = new Color(0.851f, 0.463f, 0.086f);
    Color buttonColorActive = new Color(0.855f, 0.529f, 0.212f);

    [MenuItem("Window/Placing Helper")]
	static void CreateWindow(){
		GetWindow<PlacingHelperTool> ().Show ();
	}
	
	void OnEnable(){
        uiTexture.SetPixel(0, 0, new Color(0.345f, 0.345f, 0.345f));
        uiTexture.Apply();
        uiStyle.normal.background = uiTexture;
        uiStyle.normal.textColor = Color.white;
        uiStyle.fontStyle = FontStyle.Bold;
        uiStyle.alignment = TextAnchor.MiddleCenter;

        uiTextStyle.normal.textColor = Color.white;
        uiTextStyle.fontStyle = FontStyle.Bold;
        uiTextStyle.alignment = TextAnchor.MiddleCenter;

        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.active.textColor = Color.red;
        buttonStyle.fontStyle = FontStyle.Bold;

        paintedGO = paintedGO ?? new List<GameObject> ();
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
        EditorGUILayout.BeginVertical(uiStyle);
		scroll = EditorGUILayout.BeginScrollView (scroll, false, true, GUILayout.Height (position.height - 25), GUILayout.Width (position.width));

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
		brushesFoldoutBool.target = EditorGUILayout.Foldout (brushesFoldoutBool.target, "Brushes");
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		if (EditorGUILayout.BeginFadeGroup (brushesFoldoutBool.faded)) {
			EditorGUILayout.BeginHorizontal ();
            GUI.backgroundColor = clickNewBrush ? buttonColorActive : buttonColorNormal;
            if (GUILayout.Button ("New Brush",buttonStyle)) {
				CreateBrushWindow.CreateBrush ();
                clickNDrag = true;
            }
            else
            {
                clickNewBrush = false;
            }
            GUI.backgroundColor = Color.white;
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.LabelField ("--------------------------------------------------------------------------------", uiTextStyle);

			currentBrush = (BrushPreset)EditorGUILayout.ObjectField (currentBrush, typeof(BrushPreset), false);
			if (currentBrush != null) {
				currentBrush.BurstQuantity = EditorGUILayout.IntField ("Burst Quantity", currentBrush.BurstQuantity);
				if (currentBrush.BurstQuantity <= 0) {
					currentBrush.BurstQuantity = 1;
				}
                EditorGUILayout.LabelField("--------------------------------------------------------------------------------", uiTextStyle);
                currentBrush.RandomRotation = EditorGUILayout.Toggle ("Random Rotation", currentBrush.RandomRotation);
				currentBrush.RandomXRotation = EditorGUILayout.Slider ("X Rotation", currentBrush.RandomXRotation, 0f, 360f);
				currentBrush.RandomYRotation = EditorGUILayout.Slider ("Y Rotation", currentBrush.RandomYRotation, 0f, 360f);
				currentBrush.RandomZRotation = EditorGUILayout.Slider ("Z Rotation", currentBrush.RandomZRotation, 0f, 360f);
                EditorGUILayout.LabelField("--------------------------------------------------------------------------------", uiTextStyle);
                currentBrush.Spread = EditorGUILayout.FloatField ("Spread", currentBrush.Spread);
				if(currentBrush.Spread < 0){
					currentBrush.Spread = 0;
				}
                EditorGUILayout.LabelField("--------------------------------------------------------------------------------", uiTextStyle);
                currentBrush.Spacing = EditorGUILayout.FloatField ("Spacing", currentBrush.Spacing);
				if (currentBrush.Spacing < 0.25f) {
					currentBrush.Spacing = 0.25f;
				}
                EditorGUILayout.LabelField("--------------------------------------------------------------------------------", uiTextStyle);
                EditorGUILayout.LabelField ("Painting Objects");
				var remPlz = new bool[currentBrush.paintingObjs.Count];
				for (int i = 0; i < currentBrush.paintingObjs.Count; i++) {
					EditorGUILayout.BeginHorizontal ();
					currentBrush.paintingObjs [i] = (GameObject)EditorGUILayout.ObjectField (currentBrush.paintingObjs [i], typeof(GameObject), true, GUILayout.Width (100));
					remPlz [i] = GUILayout.Button ("Remove");
					EditorGUILayout.EndHorizontal ();
				}
				for (int i = remPlz.Length - 1; i >= 0; i--) {
					if (remPlz [i]) {
						currentBrush.paintingObjs.RemoveAt (i);
					}
				}
				var objToAdd = (GameObject)EditorGUILayout.ObjectField (null, typeof(GameObject), true);
				if (objToAdd != null) {
					currentBrush.paintingObjs.Add (objToAdd);
				}
			} else {
				EditorGUILayout.HelpBox ("No brush selected", MessageType.Warning);
			}
		}
		EditorGUILayout.EndFadeGroup ();
        EditorGUILayout.EndVertical();

		string[] layers = new string[32]; //32 = maxLayers
		for (int i = 0; i < 32; i++) {
			layers [i] = LayerMask.LayerToName (i);
		}
		paintingMask = EditorGUILayout.MaskField ("Painting Layer", paintingMask, layers.Reverse ().SkipWhile (x => x == "").Reverse ().ToArray (), GUILayout.Width (position.width - 25));


		//Them buttons stuff
		EditorGUILayout.BeginHorizontal ();
		GUI.backgroundColor = clickNAdd ? buttonColorActive : buttonColorNormal;
		if (GUILayout.Button (clickNAdd ? "Stop painting" : "Click Paint", buttonStyle, GUILayout.Width ((position.width - 25) / 3))) {
			clickNAdd = !clickNAdd;
			clickNDrag = false;
			erasing = false;
		}
		GUI.backgroundColor = clickNDrag ? buttonColorActive: buttonColorNormal;
        if (GUILayout.Button (clickNDrag ? "Stop painting" : "Click n' Drag", buttonStyle, GUILayout.Width ((position.width - 25) / 3))) {
			clickNDrag = !clickNDrag;
			clickNAdd = false;
			erasing = false;
		}
		GUI.backgroundColor = erasing ? buttonColorActive: buttonColorNormal;
        if (GUILayout.Button (erasing ? "Stop Erasing" : "Erase", buttonStyle, GUILayout.Width ((position.width - 25) / 3))) {
			erasing = !erasing;
			clickNAdd = false;
			clickNDrag = false;
		}
		GUI.backgroundColor = Color.white;
		EditorGUILayout.EndHorizontal ();
        GUI.backgroundColor = Color.Lerp(Color.white, Color.red,0.4f);
		if(GUILayout.Button("Clear Eraser",buttonStyle)){
			paintedGO = new List<GameObject> ();
		}
		EditorGUILayout.LabelField ("Eraser GO's: " + paintedGO.Count);

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
				for (int i = 0; i < currentBrush.BurstQuantity; i++) {
					Spawn (hitInfo.point);
				}     
				c.Use ();
			}
		}
		if (clickNDrag) {
			HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
			RaycastHit rc;
			var hit = Physics.Raycast (HandleUtility.GUIPointToWorldRay (c.mousePosition), out rc, 10000, paintingMask);
			if (c.type == EventType.mouseDown && c.button == 0)
				mouseDown = true;
			else if (c.type == EventType.mouseUp)
				mouseDown = false;

			if(hit){
				Handles.DrawWireDisc (rc.point, rc.normal, currentBrush.Spread);
				if(mouseDown){
					distAcumSqrd += Vector3.SqrMagnitude (lastMousePos - rc.point);
					lastMousePos = rc.point;
					if(distAcumSqrd >= currentBrush.Spacing.Sqr()){		//Se cumple la distancia del brush??
						distAcumSqrd = 0;
						for (int i = 0; i < currentBrush.BurstQuantity; i++) {
							Spawn (rc.point);
						}   
					}
				}
			}
		}
		if (erasing) {
			HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
			RaycastHit rc;
			var hit = Physics.Raycast (HandleUtility.GUIPointToWorldRay (c.mousePosition), out rc, 10000, paintingMask);
			if (c.type == EventType.mouseDown && c.button == 0)
				mouseDown = true;
			else if (c.type == EventType.mouseUp)
				mouseDown = false;
			if (hit) {
				Handles.DrawWireDisc (rc.point, rc.normal, currentBrush.Spread * 1.2f);
				if (mouseDown) {
					var hitGO = Physics.OverlapSphere (rc.point, currentBrush.Spread * 1.2f).Select(x => x.gameObject).ToList();
					var borrables = hitGO.Where (x => paintedGO.Any (y => x.gameObject == y)).ToList ();
					Debug.Log ("Hit:" + hitGO.Count);
					Debug.Log ("Borrables: " + borrables.Count);
					for (int i = 0; i < borrables.Count; i++) {
						paintedGO.Remove (borrables[i].gameObject);
						DestroyImmediate (borrables[i].gameObject);
					}
				}
			}
		}
		Repaint ();
		SceneView.lastActiveSceneView.Repaint ();
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

	void Spawn(Vector3 pos){
		var GO = (GameObject)PrefabUtility.InstantiatePrefab (currentBrush.paintingObjs [Random.Range (0, currentBrush.paintingObjs.Count)]);
		Undo.RegisterCreatedObjectUndo (GO, "Object Painted");
		GO.transform.position = pos + new Vector3 (
			Random.Range (-currentBrush.Spread, currentBrush.Spread),
			Random.Range (-currentBrush.Spread, currentBrush.Spread),
			Random.Range (-currentBrush.Spread, currentBrush.Spread));
		if (currentBrush.RandomRotation) {
			GO.transform.rotation = new Quaternion (
				Random.Range (-currentBrush.RandomXRotation, currentBrush.RandomXRotation),
				Random.Range (-currentBrush.RandomYRotation, currentBrush.RandomYRotation),
				Random.Range (-currentBrush.RandomZRotation, currentBrush.RandomZRotation), 0);
		}
		paintedGO.Add (GO);
	}
    
	void OnDisable() {
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
