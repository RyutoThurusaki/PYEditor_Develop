using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PropPlacer_Editor : EditorWindow
{
    public Vector2 scrollPos_Prefab;
    public Vector2 scrollPos_Menu;
    public int UISize = 100;

    //Dropdownlist
    public string[] RotationMode = new string[] { "オリジナル", "視点", "スナップ", "ランダム" };
    public int RotationModeInt = 0;
    public string[] PositionMode = new string[] { "ベーシック", "端数切捨て", "スナップ", "ランダム" };
    public int PositionModeInt = 0;
    public string[] PositioningMode = new string[] { "マウスベース", "視点ベース" };
    public int PositioningModeInt = 0;

    //Raycast
    Camera SceneRaycam;
    public Vector3 ViewPos = new Vector3(0.5f, 0.5f, 0);

    //Prefabselector
    public List<GameObject> SetPrefabs = new List<GameObject>(0);
    public List<Texture> PrefabTex = new List<Texture>(0);
    public int SelectPrefabNum = 0;

    //Spownsetting
    public List<Transform> PrefabPlacepoints = new List<Transform>(10);
    public Transform DefaultPoint;
    public GameObject LatestProp;

    //Situation
    public GameObject GhostProp;
    public bool TryBool = true;
    public bool Buttontoggle = true;
    public bool RunStatus = true;

    //Debug


    [MenuItem("PYEditor/PropPlacer")]

    private static void Create()
    {
        GetWindow<PropPlacer_Editor>("PropPlacer");
    }
    public void OnGUI()
    {
        //-----全体横配置
        EditorGUILayout.BeginHorizontal();

        //=====InfoBox用縦配置
        EditorGUILayout.BeginVertical();

        //-----Prefab要素横配置
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos_Prefab, GUILayout.Height(150), GUILayout.Height(UISize * 2f)))
        {
            scrollPos_Prefab = scrollView.scrollPosition;

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < SetPrefabs.Count; i++)
            {
                EditorGUILayout.BeginVertical();


                if (GUILayout.Button(new GUIContent(PrefabTex[i]), GUILayout.Width(UISize), GUILayout.Height(UISize)))
                {
                    SelectPrefabNum = i;
                }

                EditorGUILayout.Space(5);

                SetPrefabs[i] = (GameObject)EditorGUILayout.ObjectField("", SetPrefabs[i], typeof(GameObject), false, GUILayout.Width(UISize), GUILayout.Height(20));

                EditorGUILayout.Space(5);

                PrefabPlacepoints[i] = (Transform)EditorGUILayout.ObjectField("", PrefabPlacepoints[i], typeof(Transform), true, GUILayout.Width(UISize));

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndHorizontal();
        }
        //-----Prefab要素横配置オワリ


        if (PrefabPlacepoints[SelectPrefabNum] != null)
        {
            EditorGUILayout.HelpBox("aaa", MessageType.Info, true);
        }
        else
        {
            EditorGUILayout.HelpBox("このPrefabはスポーン先に一括設定が使用されます。", MessageType.Warning, true);
        }

        EditorGUILayout.EndVertical();
        //=====InfoBox用縦配置オワリ

        //=====メニュー縦配置
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos_Menu, GUILayout.Width(UISize * 2.3f)))
        {
            scrollPos_Menu = scrollView.scrollPosition;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(5);

            //一括でスポーン先を設定するフィールド
            EditorGUILayout.LabelField("スポーン先一括設定");
            DefaultPoint = (Transform)EditorGUILayout.ObjectField("", DefaultPoint, typeof(Transform), true, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //Positioning設定
            EditorGUILayout.LabelField("ポジションモード");
            PositioningModeInt = EditorGUILayout.Popup("", PositioningModeInt, PositioningMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //回転姿勢の指定
            EditorGUILayout.LabelField("Rotation挙動");
            RotationModeInt = EditorGUILayout.Popup("", RotationModeInt, RotationMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //回転姿勢の指定
            EditorGUILayout.LabelField("Position挙動");
            PositionModeInt = EditorGUILayout.Popup("", PositionModeInt, PositionMode, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //-----増減ボタン横並び
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("－", GUILayout.Width(UISize)))
            {
                if (SetPrefabs.Count > 1)
                {
                    int count = SetPrefabs.Count - 1;
                    SetPrefabs.RemoveAt(count);
                    PrefabTex.RemoveAt(count);
                    PrefabPlacepoints.RemoveAt(count);
                }
            }

            if (GUILayout.Button("＋", GUILayout.Width(UISize)))
            {
                SetPrefabs.Add(null);
                PrefabTex.Add(null);
                PrefabPlacepoints.Add(null);
            }

            GUILayout.EndHorizontal();
            //-----増減ボタン横並びオワリ

            if (RunStatus)
            {
                if (GUILayout.Button("STOP!!", GUILayout.Width(UISize * 2)))
                {
                    OnDisable();
                    RunStatus = !RunStatus;
                }
            }
            else
            {
                if (GUILayout.Button("START!!", GUILayout.Width(UISize * 2)))
                {
                    OnEnable();
                    RunStatus = !RunStatus;
                }
            }

            EditorGUILayout.Space(5);

            if (GUILayout.Button("レッツプレース！(F)", GUILayout.Width(UISize * 2)))
            {
                PlaceProp();
            }

            EditorGUILayout.Space(5);

            //UISize調整　30以下には出来ない
            UISize = EditorGUILayout.IntField("UISize", UISize, GUILayout.Width(UISize * 2));

            if (UISize < 29)
            {
                UISize = 30;
            }

            EditorGUILayout.EndVertical();
            //=====メニュー縦配置オワリ
        }



        EditorGUILayout.EndHorizontal();
        //-----全体横配置オワリ

        //ゴーストの変更
        if (GUI.changed)
        {
            GhostReload();

            //サムネイル更新
            for (int i = 0; i < SetPrefabs.Count; i++)
            {
                if (SetPrefabs[i] != null)
                {
                    PrefabTex[i] = AssetPreview.GetAssetPreview(SetPrefabs[i]);
                }
                else
                {
                    //Debug.LogError("itsnull");
                }
            }
        }
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= PropPlacer_Update;
        DestroyImmediate(GhostProp);
    }
    private void OnEnable()
    {
        SceneView.duringSceneGui += PropPlacer_Update;

        if (SetPrefabs.Count > 0)
        {
            //何もしない
        }
        else
        {
            SetPrefabs.Add(null);
            PrefabTex.Add(null);
            PrefabPlacepoints.Add(null);
        }
    }
    public void PropPlacer_Update(SceneView sceneView)
    {
        //updateで実行

        //Nullチェック達
        if (GhostProp == null)
        {
            GhostReload();
        }
        if (SceneRaycam == null)
        {
            SceneRaycam = UnityEditor.SceneView.lastActiveSceneView.camera;
        }

        //ゴーストの位置を更新する
        else if (SetPrefabs[SelectPrefabNum] != null && PositioningModeInt == 1)
        {
            //視点モード
            var ray = SceneRaycam.ViewportPointToRay(ViewPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GhostProp.transform.position = hit.point;
            }
        }
        else if (SetPrefabs[SelectPrefabNum] != null && PositioningModeInt == 0)
        {

            RaycastHit hit;

            //マウスモード
            var ray = SceneRaycam.ViewportPointToRay(Input.mousePosition);
            float depth = 15;
            //var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);
            GhostProp.transform.position = SceneRaycam.ScreenToWorldPoint(Input.mousePosition);




            //ショートカットキー
            var keyinput = Event.current;

            if (keyinput.type == EventType.KeyDown && keyinput.keyCode == KeyCode.F)
            {
                PlaceProp();
            }
        }
    }

    public void PlaceProp()
    {
        Transform parent = null;

        //親の設定
        if (PrefabPlacepoints[SelectPrefabNum] != null)
        {
            //スポーン先が個別指定されている場合
            //個別指定が優先される
            parent = PrefabPlacepoints[SelectPrefabNum];
        }
        else if (DefaultPoint != null)
        {
            //スポーン先が個別指定されておらず、共通親が設定されている場合
            parent = DefaultPoint;
        }

        //スポーン処理
        if (parent != null)
        {
            LatestProp = (GameObject)PrefabUtility.InstantiatePrefab(SetPrefabs[SelectPrefabNum]);

            LatestProp.transform.SetParent(parent);
            LatestProp.transform.SetPositionAndRotation(GhostProp.transform.position, GhostProp.transform.rotation);

            Undo.RegisterCreatedObjectUndo(LatestProp.gameObject, "PropPlace");
        }
        else
        {
            Debug.LogError("スポーン先が未指定です。一括指定か個別指定をしてください。 Says PYEditor/PropPlacerDB");
        }
    }

    private void GhostReload()
    {
        //ゴーストのリロード用
        if (SetPrefabs[SelectPrefabNum] != null)
        {
            DestroyImmediate(GhostProp);

            GhostProp = (GameObject)PrefabUtility.InstantiatePrefab(SetPrefabs[SelectPrefabNum]) as GameObject;

            //ゴーストのコライダーを削除
            Collider[] GhostinCollider = GhostProp.GetComponentsInChildren<Collider>();
            foreach (Collider coli in GhostinCollider)
            {
                GameObject.DestroyImmediate(coli);
            }
        }
        else
        {
            if (GhostProp != null)
            {
                DestroyImmediate(GhostProp);
            }
        }
    }
}
#endif