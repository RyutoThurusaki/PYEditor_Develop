using UnityEditor.ShortcutManagement;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class PropPlacerDB : ScriptableObject
{
    public Vector2 scrollPos_Prefab;
    public Vector2 scrollPos_Menu;

    //Dropdownlist
    public string[] RotationMode = new string[] { "オリジナル", "視点", "スナップ45", "垂直スナップ45", "ランダムY","ランダムXYZ" };
    public int RotationModeInt = 0;
    public string[] PositionMode = new string[] { "ベーシック", "端数切捨て", "スナップ0.5", "スナップ1", "ノイズ"};
    public int PositionModeInt = 0;
    public string[] PositioningMode = new string[] { "マウスベース", "視点ベース"};
    public int PositioningModeInt = 0;

    //Raycast
    Camera SceneRaycam = UnityEditor.SceneView.lastActiveSceneView.camera;
    public Vector3 ViewPos = new Vector3(0.5f, 0.5f, 0);

    //Prefabselector
    public List<GameObject> SetPrefabs = new List<GameObject>(0);
    public List<Texture> PrefabTex = new List<Texture>(0);
    public int SelectPrefabNum = 0;

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
                //Debug.Log(coli);
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
            Debug.LogError("スポーン先が未指定です。一括指定か個別指定をしてください。 Says PYEditor/PropPlacerDB");
        }
    }
}

#endif