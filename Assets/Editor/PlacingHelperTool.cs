using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class PlacingHelperTool : EditorWindow{


	//GameObjects
	List<GameObject> placingGO = new List<GameObject> ();
	List<GameObject> goList = new List<GameObject>();
	List<bool> paintBoolList = new List<bool> ();
	AnimBool gameObjectsFoldoutBool = new AnimBool();

	//Placing
	AnimBool placingFoldoutBool = new AnimBool();
	bool RandomRotationX;
	bool RandomRotationY;
	bool RandomRotationZ;

    //Brushes
    AnimBool brushesFoldoutBool = new AnimBool();
    string nameTField;
    BrushPreset currentBrush;
    float timeTillNextStroke;

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
	}

    void OnGUI(){
		EditorGUILayout.LabelField ("Helper", EditorStyles.helpBox);

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

		placingFoldoutBool.target = EditorGUILayout.Foldout (placingFoldoutBool.target, "Placing");
		if(EditorGUILayout.BeginFadeGroup(placingFoldoutBool.faded)){
			RandomRotationX = EditorGUILayout.Toggle ("Random X Rotation", RandomRotationX);
			RandomRotationY = EditorGUILayout.Toggle ("Random Y Rotation", RandomRotationY);
			RandomRotationZ = EditorGUILayout.Toggle ("Random Z Rotation", RandomRotationZ);
		}
		EditorGUILayout.EndFadeGroup ();

        //Brushes
        brushesFoldoutBool.target = EditorGUILayout.Foldout(brushesFoldoutBool.target, "Brushes");
        if (EditorGUILayout.BeginFadeGroup(brushesFoldoutBool.faded))
        {
            EditorGUILayout.BeginHorizontal();
            nameTField = EditorGUILayout.TextField(nameTField,GUILayout.Width(200));
            if (GUILayout.Button("New Brush",GUILayout.Width(100)))
            {
                BrushPresetFactory.CreateBrush<BrushPreset>(nameTField);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("-------------------------------------------");
            currentBrush = (BrushPreset)EditorGUILayout.ObjectField(currentBrush, typeof(BrushPreset), false);
        }
        EditorGUILayout.EndFadeGroup();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Event current = Event.current;
        
        if(current.type == EventType.MouseDown && current.button == 0 && currentBrush != null)
        {
            if (currentBrush.burstMode)
            {
                Ray mouseRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(mouseRay, out hitInfo, 10000))
                {
                    for (int i = 0; i < currentBrush.burstQuantity; i++)
                    {
                        GameObject instObj = (GameObject)PrefabUtility.InstantiatePrefab(currentBrush.paintingObjs[Random.Range(0, currentBrush.paintingObjs.Count + 1)]);
                        instObj.transform.position = hitInfo.point + new Vector3(Random.Range(0, currentBrush.randomXOffset),
                                                                                 Random.Range(0, currentBrush.randomYOffset),
                                                                                 Random.Range(0, currentBrush.randomZOffset));
                        if(currentBrush.randomRotation)
                        {
                            instObj.transform.rotation = new Quaternion(Random.Range(0, currentBrush.randomXRotation),
                                                                        Random.Range(0, currentBrush.randomYRotation),
                                                                        Random.Range(0, currentBrush.randomZRotation), 0);
                        }
                        EditorUtility.SetDirty(instObj);
                    }     
                    current.Use();
                }
            }
            else
            {

            }
        }
    }

	void OnDisable()
    {
        //Esto remueve el metodo OnSceneGui de la lista de eventos del SceneView.
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

		gameObjectsFoldoutBool.valueChanged.RemoveListener (Repaint);
		placingFoldoutBool.valueChanged.RemoveListener (Repaint);
        brushesFoldoutBool.valueChanged.RemoveListener(Repaint);
    }
}
