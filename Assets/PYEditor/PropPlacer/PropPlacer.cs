using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PropPlacer
{
    //シーンビュー上の処理

    static PropPlacer()
    {
        SceneView.duringSceneGui += OnGui;
    }

    private static void OnGui(SceneView sceneView)
    {

        Handles.BeginGUI();

        ShowButtons(sceneView.position.size);

        Handles.EndGUI();
    }

    /// <summary>
    /// ボタンの描画関数
    /// </summary>
    private static void ShowButtons(Vector2 sceneSize)
    {
        var UISize = 65;
        PropPlacerDB propPlacerDB = Resources.Load<PropPlacerDB>("PropPlacerDB");

    int hoge = 5;
        for(int i = 0; i < hoge; i++)
        {
            if (GUILayout.Button("hogehoge"))
            {
                Debug.Log("Press " + i + " Key");
            }
        }

        //#####全体横配置
        EditorGUILayout.BeginHorizontal();


        //-----Prefab要素縦配置
        using (var scrollView = new EditorGUILayout.ScrollViewScope(propPlacerDB.scrollPos_Prefab, GUILayout.Height(150)))
        {
            propPlacerDB.scrollPos_Prefab = scrollView.scrollPosition;

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < propPlacerDB.SetPrefabs.Count; i++)
            {
                EditorGUILayout.BeginVertical();


                if (GUILayout.Button(new GUIContent(propPlacerDB.PrefabTex[i]), GUILayout.Width(UISize), GUILayout.Height(UISize)))
                {
                    propPlacerDB.SelectPrefabNum = i;
                }

                EditorGUILayout.Space(5);

                propPlacerDB.SetPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("", propPlacerDB.SetPrefabs[i], typeof(GameObject), false, GUILayout.Width(UISize), GUILayout.Height(20));

                EditorGUILayout.Space(5);

                propPlacerDB.PrefabPlacepoints[i] = (Transform)EditorGUILayout.ObjectField("", propPlacerDB.PrefabPlacepoints[i], typeof(Transform), true, GUILayout.Width(UISize));

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndHorizontal();
        }
        //-----Prefab要素縦配置オワリ

        //選択中のPrefab画像
        EditorGUILayout.LabelField(new GUIContent(propPlacerDB.PrefabTex[propPlacerDB.SelectPrefabNum]), GUILayout.Height(UISize * 2), GUILayout.Width(UISize * 2));

        //=====メニュー縦配置
        using (var scrollView = new EditorGUILayout.ScrollViewScope(propPlacerDB.scrollPos_Menu, GUILayout.Width(UISize * 2.3f)))
        {
            propPlacerDB.scrollPos_Menu = scrollView.scrollPosition;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(5);

            //一括でスポーン先を設定するフィールド
            EditorGUILayout.LabelField("スポーン先一括設定");
            propPlacerDB.DefaultPoint = (Transform)EditorGUILayout.ObjectField("", propPlacerDB.DefaultPoint, typeof(Transform), true, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //Positioning設定
            EditorGUILayout.LabelField("ポジションモード");
            propPlacerDB.PositioningModeInt = EditorGUILayout.Popup("", propPlacerDB.PositioningModeInt, propPlacerDB.PositioningMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //回転姿勢の指定
            EditorGUILayout.LabelField("Rotation挙動");
            propPlacerDB.RotationModeInt = EditorGUILayout.Popup("", propPlacerDB.RotationModeInt, propPlacerDB.RotationMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //回転姿勢の指定
            EditorGUILayout.LabelField("Position挙動");
            propPlacerDB.PositionModeInt = EditorGUILayout.Popup("", propPlacerDB.PositionModeInt, propPlacerDB.PositionMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            if (GUILayout.Button("レッツプレース！", GUILayout.Width(UISize * 2)))
            {
                //PlaceProp();
            }

            EditorGUILayout.Space(5);

            EditorGUILayout.EndVertical();
        }
        //=====メニュー縦配置オワリ


        EditorGUILayout.EndHorizontal();
        //#####全体横配置オワリ


        //ゴーストの変更
        if (GUI.changed)
        {
            //GhostReload();

            //サムネイル更新
            for (int i = 0; i < propPlacerDB.SetPrefabs.Count; i++)
            {
                propPlacerDB.PrefabTex[i] = AssetPreview.GetAssetPreview(propPlacerDB.SetPrefabs[i]);
            }
        }

        //以下ショートカット
        var key = Event.current;
        var validate = key.type == EventType.KeyDown;

        if (validate && key.keyCode == KeyCode.Alpha0)
        {
            Debug.Log("hogehoge");
        }
    }
}