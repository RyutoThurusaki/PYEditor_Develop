using UnityEditor.ShortcutManagement;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class ItempropPlacer : EditorWindow
{
    Vector2 scrollPos_Prefab;
    Vector2 scrollPos_Menu;

    //Dropdownlist
    string[] RotationMode = new string[] { "オリジナル", "視点", "スナップ45", "垂直スナップ45", "ランダムY","ランダムXYZ" };
    int RotationModeInt = 0;
    string[] PositionMode = new string[] { "ベーシック", "端数切捨て", "スナップ0.5", "スナップ1", "ノイズ"};
    int PositionModeInt = 0;
    string[] PositioningMode = new string[] { "マウスベース", "視点ベース"};
    int PositioningModeInt = 0;

    //Raycast
    Camera SceneRaycam = UnityEditor.SceneView.lastActiveSceneView.camera;
    Vector3 ViewPos = new Vector3(0.5f, 0.5f, 0);

    //Prefabselector
    public int SelectPrefabNum = 0;
    public List<GameObject> SetPrefabs = new List<GameObject>(0);
    public List<Texture> PrefabTex = new List<Texture>(0);

    //Spownsetting
    public List<Transform> PrefabPlacepoints = new List<Transform>(0);
    public Transform DefaultPoint;
    public GameObject LatestProp;

    //Situation
    public GameObject GhostProp;
    public bool TryBool = true;
    public bool Buttontoggle = true;


    //debug
    public Vector3 hogehoge;


    [MenuItem("PYEditor/ItempropPlacer")]

    [SerializeField]


    private static void Create()
    {
        GetWindow<ItempropPlacer>("ItempropPlacer");
    }

    private void OnGUI()
    {
        var UISize = 65;
        wantsMouseMove = true;

        //#####全体横配置
        EditorGUILayout.BeginHorizontal();


        //-----Prefab要素縦配置
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos_Prefab, GUILayout.Height(150)))
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
        //-----Prefab要素縦配置オワリ

        //選択中のPrefab画像
        EditorGUILayout.LabelField(new GUIContent(PrefabTex[SelectPrefabNum]), GUILayout.Height(UISize * 2), GUILayout.Width(UISize * 2));

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

            if (GUILayout.Button("レッツプレース！",GUILayout.Width(UISize * 2)))
            {
                PlaceProp();
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
            GhostReload();

            //サムネイル更新
            for (int i = 0; i < SetPrefabs.Count; i++)
            {
                PrefabTex[i] = AssetPreview.GetAssetPreview(SetPrefabs[i]);
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

    private void OnEnable()
    {
        EditorApplication.update += PrefabGhost;
        SceneRaycam = UnityEditor.SceneView.lastActiveSceneView.camera;
    }

    private void OnDisable()
    {
        EditorApplication.update -= PrefabGhost;
        DestroyImmediate(GhostProp);
    }

    public void PrefabGhost()
    {
        //updateで実行

        //ゴーストの位置を更新する
        if (GhostProp == null)
        {
            GhostReload();
        }
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
            //マウスモード
            var ray = SceneRaycam.ViewportPointToRay(Input.mousePosition);
            float depth = -15;
            var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);
            GhostProp.transform.position = SceneRaycam.ScreenToWorldPoint(pos);


        }
    }

    public void GhostReload()
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
                Debug.Log(coli);
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

    public void PlaceProp()
    {
        Transform parent = null;

        //親の設定
        if (PrefabPlacepoints[SelectPrefabNum] != null)
        {
            //スポーン先が個別指定されている場合
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

            Undo.RecordObject(LatestProp.gameObject, "PropPlace");

            LatestProp.transform.SetParent(parent);
            LatestProp.transform.SetPositionAndRotation(GhostProp.transform.position, GhostProp.transform.localRotation);
        }
        else
        {
            Debug.LogError("スポーン先が未指定です。一括設定か個別指定をしてください。 Says PYEditor/ItempropPlacer");
        }
    }
}

#endif