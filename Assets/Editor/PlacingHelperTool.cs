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

	[MenuItem("Window/Placing Helper")]
	static void CreateWindow(){
		GetWindow<PlacingHelperTool> ().Show ();
	}

	void OnEnable(){
		gameObjectsFoldoutBool = new AnimBool ();
		gameObjectsFoldoutBool.valueChanged.AddListener (Repaint);
		placingFoldoutBool = new AnimBool ();
		placingFoldoutBool.valueChanged.AddListener (Repaint);

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
	}

    void OnSceneGUI(SceneView sceneView)
    {
        Event current = Event.current;

        //Pregunta si se hizo click izquierdo en la ventana scene, y si hay algun gameObject en la lista para instanciar
        if(current.type == EventType.MouseDown && current.button == 0 && placingGO.Count > 0)
        {
            //Crea un ray que va desde el lugar en el que se hizo click, hacia el mundo.
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hitInfo;

            //Dispara el raycast
            if (Physics.Raycast(mouseRay, out hitInfo, 10000))
            {
                //Instancio el objeto guardado en la lista y guardo la referencia.
                GameObject instObj = (GameObject)PrefabUtility.InstantiatePrefab(placingGO[0]);

                //Posiciono el objeto en el lugar en que pegó el raycast
                instObj.transform.position = hitInfo.point;

                //Establece el nuevo objeto como dirty.
                EditorUtility.SetDirty(instObj);
            }
        }

        //Consume el evento para que no se siga propagando por el editor. Basicamente lo marca como "usado".
        current.Use();
    }

	void OnDisable()
    {
        //Esto remueve el metodo OnSceneGui de la lista de eventos del SceneView.
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

		gameObjectsFoldoutBool.valueChanged.RemoveListener (Repaint);
		placingFoldoutBool.valueChanged.RemoveListener (Repaint);
    }
}
