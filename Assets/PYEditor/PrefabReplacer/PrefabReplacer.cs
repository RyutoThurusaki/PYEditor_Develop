using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class PrefabReplacer : EditorWindow
{
    public Transform TargetObjects = null;
    public Transform TargetPrefab = null;
    public Object ReplacePrefab = null;

    private int ReplaceCount = 0;
    private bool consent = false;

    [MenuItem("PYEditor/PrefabReplacer")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<PrefabReplacer>("PrefabReplacer");
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            EditorGUILayout.PrefixLabel("設定");

            EditorGUILayout.Space(20);

            TargetObjects = (Transform)EditorGUILayout.ObjectField("ターゲットオブジェクトの親",TargetObjects, typeof(Transform), true);
            EditorGUILayout.HelpBox("リプレイス元のオブジェクトは１つの空きオブジェクトを親としてまとめ、指定できます。", MessageType.Info);

            EditorGUILayout.Space(20);

            TargetPrefab = (Transform)EditorGUILayout.ObjectField("リプレイス元のPrefab", TargetPrefab, typeof(Transform), true);

            EditorGUILayout.Space(20);

            ReplacePrefab = (Object)EditorGUILayout.ObjectField("リプレイス先のPrefab", ReplacePrefab, typeof(Object), true);

            EditorGUILayout.Space(20);


            if (consent)
            {
                EditorGUILayout.HelpBox("Undo機能未実装なので必ずシーンセーブした上で実行すること。", MessageType.Warning);

                if (GUILayout.Button("レッツリプレイス！！"))
                {
                    Search();
                    ReplaceCount = 0;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Undo機能未実装なので必ずシーンセーブした上で実行すること。\\ セーブ後下記にチェックを入れてください。", MessageType.Warning);
            }
            consent = EditorGUILayout.Toggle("了解した", consent);
        }
    }

    private void Search()
    {
        if (TargetObjects == null)
        {
            Debug.LogError("ターゲットを指定してください。 Says PYEditor/PrefabReplacer");
        }
        else
        {
            //Prefab探し
            Transform[] Objects = TargetObjects.GetComponentsInChildren<Transform>();

            Undo.RecordObjects(Objects, "Changed prefab");

            for (int i = 0; i < Objects.Length; i++)
            {
                //ヌルチェック
                if (Objects[i] != null)
                {
                    //Prefabの同一性チェック
                    if (AssetDatabase.GetAssetPath(TargetPrefab) == AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(Objects[i])))
                    {
                        //Debug.Log(TargetPrefab + "を" + ReplacePrefab +"にリプレースしました");

                        //コピー
                        var parent = Objects[i].transform.parent;
                        var pos = Objects[i].transform.localPosition;
                        var rot = Objects[i].transform.localRotation;
                        var sca = Objects[i].transform.localScale;

                        DestroyImmediate(Objects[i].gameObject);

                        //生成
                        GameObject newobj = (GameObject)PrefabUtility.InstantiatePrefab(ReplacePrefab);

                        //ペースト
                        newobj.transform.SetParent(parent);
                        newobj.transform.localPosition = pos;
                        newobj.transform.localRotation = rot;
                        newobj.transform.localScale = sca;

                        Objects[i] = newobj.transform;
                        ReplaceCount++;

                    }
                    else
                    {
                        //何もしない
                    }
                }
            }

            Debug.Log(ReplaceCount + "個のPrefabのリプレースを完了しました。");
        }

    }
}

#endif