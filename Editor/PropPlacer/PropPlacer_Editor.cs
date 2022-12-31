using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PropPlacer_Editor : EditorWindow
{
    private Vector2 scrollPos_Prefab;
    private Vector2 scrollPos_Menu;
    private int UISize = 100;

    //Dropdownlist
    private string[] PositioningModes = new string[] { "マウスベース", "視点ベース" };
    private int PositioningModeInt = 0;
    private string[] RotationModes = new string[] { "オリジナル", "スナップ", "ランダム" };
    private int RotationModeInt = 0;
    private string[] PositionModes = new string[] { "ベーシック",  "スナップ", "ランダム" };
    private int PositionModeInt = 0;
    private string[] Uisizeints = new string[] { "30", "40", "50", "60"};
    private int UisizeModeint = 0;

    //VectorsSetting
    private Vector2 Rotatesnap;
    private Vector3 Rotaterandam;
    private Vector3 Positionsnap;
    private Vector3 Positionrandam;

    private Vector3 RandamResult;

    //Raycast
    Camera SceneRaycam;
    private Vector3 ViewPos = new Vector3(0.5f, 0.5f, 0);

    //Prefabselector
    private List<GameObject> SetPrefabs = new List<GameObject>(0);
    private List<Texture> PrefabTex = new List<Texture>(0);
    private int SelectPrefabNum = 0;

    //Spownsetting
    private List<Transform> PrefabPlacepoints = new List<Transform>(10);
    private Transform DefaultPoint;
    private GameObject LatestProp;
    private bool RoundedDown = false;

    //Situation
    private GameObject GhostProp;
    private GameObject ParentProp;
    private bool TryBool = true;
    private bool Buttontoggle = true;
    private bool RunStatus = true;

    //Debug
    int count = 0;

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
            EditorGUILayout.HelpBox("個別設定でスポーンします。", MessageType.Info, true);
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
            PositioningModeInt = EditorGUILayout.Popup("", PositioningModeInt, PositioningModes, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            //回転姿勢の指定
            EditorGUILayout.LabelField("Rotation挙動");
            RotationModeInt = EditorGUILayout.Popup("", RotationModeInt, RotationModes, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            if (RotationModeInt == 1)
            {
                Rotatesnap = EditorGUILayout.Vector2Field("スナップの角度", Rotatesnap, GUILayout.Width(UISize * 2));
            }
            else if (RotationModeInt == 2)
            {
                Rotaterandam = EditorGUILayout.Vector3Field("ランダムの角度", Rotaterandam, GUILayout.Width(UISize * 2));
            }
            EditorGUILayout.Space(5);


            //座標の指定
            EditorGUILayout.LabelField("Position挙動");
            PositionModeInt = EditorGUILayout.Popup("", PositionModeInt, PositionModes, GUILayout.Width(UISize * 2));

            EditorGUILayout.Space(5);

            if (PositionModeInt == 1)
            {
                Positionsnap = EditorGUILayout.Vector3Field("スナップの角度", Positionsnap, GUILayout.Width(UISize * 2));
            }
            else if (PositionModeInt == 2)
            {
                Positionrandam = EditorGUILayout.Vector3Field("ランダムの角度", Positionrandam, GUILayout.Width(UISize * 2));
            }

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
        DestroyImmediate(ParentProp);
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

    private void PropPlacer_Update(SceneView sceneView)
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

        //Key input
        //ショートカットキー
        var keyinput = Event.current;

        //ゴーストの位置を更新する
        if (SetPrefabs[SelectPrefabNum] != null)
        {
            //PositionUpdate
             if (PositioningModeInt == 0)
            {
                //マウスモード
                Vector3 subtract = new Vector3(Event.current.mousePosition.x, Screen.height - Event.current.mousePosition.y - 40, 0);
                Ray ray = SceneRaycam.ScreenPointToRay(subtract);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    ParentProp.transform.position = hit.point;

                    if (keyinput.type == EventType.KeyDown && keyinput.keyCode == KeyCode.F)
                    {
                        PlaceProp();
                        RandamResult = Vec3Random(Rotaterandam);
                    }
                }

                GhostProp.transform.localPosition = Vector3.zero;

            }
            else if (PositioningModeInt == 1)
            {
                //視点モード
                var ray = SceneRaycam.ViewportPointToRay(ViewPos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    ParentProp.transform.position = hit.point;

                    if (keyinput.type == EventType.KeyDown && keyinput.keyCode == KeyCode.F)
                    {
                        PlaceProp();
                        RandamResult = Vec3Random(Rotaterandam);
                    }
                }

                GhostProp.transform.localPosition = Vector3.zero;

            }


             //RotationUpdate
             if (RotationModeInt == 0)
            {
                //オリジナル

            }
             else if (RotationModeInt == 1)
            {
                //スナップ

                var tra = GhostProp.transform;
                var targetY = ParentProp.transform.InverseTransformPoint(SceneRaycam.transform.position);
                var targetX = GhostProp.transform.InverseTransformPoint(SceneRaycam.transform.position);

                Quaternion angleY = Quaternion.AngleAxis(Mathf.Atan2(targetY.x, targetY.z) * Mathf.Rad2Deg, Vector3.up);
                Quaternion angleX = Quaternion.AngleAxis(Mathf.Atan2(targetX.z, targetX.y) * Mathf.Rad2Deg, Vector3.right);

                //ParentProp.transform.rotation.y + angleY
                ParentProp.transform.rotation = QuaRound(ParentProp.transform.rotation * angleY, Rotatesnap.y);
                GhostProp.transform.localRotation = QuaRound(GhostProp.transform.localRotation * angleX, Rotatesnap.x);

            }
            else if (RotationModeInt == 2)
            {
                //ランダム
                GhostProp.transform.rotation = Quaternion.Euler(RandamResult);
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
            //個別指定が優先される　else ifなことに注意
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
        //親を生成
        if (ParentProp == null)
        {
            ParentProp = new GameObject("Dont delete this_PYEditor");
        }
        Debug.Log("Ghost Refresh say PYEditor");
        //ゴーストのリロード用
        if (SetPrefabs[SelectPrefabNum] != null)
        {
            DestroyImmediate(GhostProp);

            GhostProp = (GameObject)PrefabUtility.InstantiatePrefab(SetPrefabs[SelectPrefabNum]) as GameObject;

            //ゴーストの初期化
            GhostProp.transform.SetParent(ParentProp.transform);
            GhostProp.transform.position = new Vector3(0, 0, 0);

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

    private Vector3 Vec3Random(Vector3 snap)
    {
        Vector3 randamvec = new Vector3(
                                                                Random.Range(1, 361),
                                                                Random.Range(1, 361),
                                                                Random.Range(1, 361)
                                                                );

        return Vec3Round(randamvec, 0, snap);
    }

    private Quaternion QuaRound(Quaternion quain, float cut)
    {
        Vector3 src = quain.eulerAngles;
        Vector3 dst = Vec3Round(src, cut);
        return Quaternion.Euler(dst);
    }

    private Vector3 Vec3Round(Vector3 vecin, float cut = 0, Vector3 veccut = default(Vector3))
    {
        //veccut省略時は(0,0,0)で初期化される
        Vector3 dst = new Vector3(
                                            Mathf.Round(vecin.x / (cut + veccut.x)) * (cut + veccut.x),
                                            Mathf.Round(vecin.y / (cut + veccut.y)) * (cut + veccut.y),
                                            Mathf.Round(vecin.z / (cut + veccut.z)) * (cut + veccut.x));
        return dst;
    }
}
#endif