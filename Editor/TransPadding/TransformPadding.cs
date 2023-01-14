using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class TransformPadding : EditorWindow
{
    public Transform TargetObjects = null;

    public bool MeshOnly = true;
    public bool SuruDoubleRenderer = true;
    public bool ToScale = false;
    public bool SamePadding = true;
    public bool trickonly = false;
    public Vector3 Scaling = new Vector3(1, 1, 1);

    //オフセットたち
    public Vector3 PaddingOffsets = new Vector3(1,1,1);
    public Transform Pivot;

    [MenuItem("PYEditor/TransformPadding")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<TransformPadding>("TransformPadding");
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            EditorGUILayout.PrefixLabel("設定");

            EditorGUILayout.Space(20);

            TargetObjects = (Transform)EditorGUILayout.ObjectField("ターゲットオブジェクトの親",TargetObjects, typeof(Transform), true);
            EditorGUILayout.HelpBox("パディングするオブジェクトは１つの空きオブジェクトを親としてまとめ、指定できます。", MessageType.Info);

            EditorGUILayout.Space(20);

            EditorGUILayout.BeginVertical(GUI.skin.box);//オフセット用ボックス開始
                PaddingOffsets = EditorGUILayout.Vector3Field("パディング倍率", PaddingOffsets);

                EditorGUILayout.Space(10);

                Pivot = (Transform)EditorGUILayout.ObjectField("パディングする基準点", Pivot, typeof(Transform), true);
            EditorGUILayout.EndVertical();//オフセット用ボックスオワリ

            EditorGUILayout.Space(20);

            if (trickonly)
            {
                EditorGUILayout.HelpBox("トリックオンリーのため非表示にされた設定があります。",MessageType.None);
            }
            else
            {
                MeshOnly = EditorGUILayout.Toggle("MeshRenderer以外を無視", MeshOnly, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space(10);
                SuruDoubleRenderer = EditorGUILayout.Toggle("多重構造のMeshRendererを無視", SuruDoubleRenderer);
                EditorGUILayout.Space(10);
            }
            ToScale = EditorGUILayout.Toggle("連動してScaleも変える",ToScale);

            EditorGUILayout.Space(10);

            if (ToScale)
            {
                SamePadding = EditorGUILayout.Toggle("└パディングと同じ倍率を使用", SamePadding);
                if (SamePadding != true)
                {
                    Scaling = EditorGUILayout.Vector3Field("　└スケール倍率", Scaling);
                }
                else
                {
                    Scaling = PaddingOffsets;
                }
            }
            else
            {
                Scaling = new Vector3(1, 1, 1);
            }
            EditorGUILayout.Space(20);

            trickonly = EditorGUILayout.Toggle("Trickオンリーモード", trickonly);


            EditorGUILayout.Space(20);

            if (GUILayout.Button("レッツパディング！！"))
            {
                Padding();
            }

            EditorGUILayout.HelpBox("パディングはCtrl+ZのUndoでもとに戻せます。\n 動作しない場合コンソールにエラーが出ていないかご確認ください。 ", MessageType.Info);
            
            if (trickonly)
            {
                EditorGUILayout.HelpBox("PaddingTrickコンポーネントがついたオブジェクトのみをパディングします。 \n コンポーネントのつけ忘れにご注意ください。 ", MessageType.Warning);
            }
        }
    }

    private void Padding()
    {
        if (TargetObjects == null || Pivot == null)
        {
            Debug.LogError("パディングするオブジェクトの親と基準点の両方を指定してください。 Says PYEditor/TransformPadding");
        }
        else
        {
            //子供のオブジェクトを全取得
            Component[] ChildlenTransforms = TargetObjects.GetComponentsInChildren(typeof(Transform));

            //子供のTransformにOffsetを全かけする i=1なのは親自身を省くため
            Undo.RecordObjects(ChildlenTransforms, "Changed children position");


            if (trickonly)
            {
                for (int i = 1; i < ChildlenTransforms.Length; i++)
                {
                    Transform transform = (Transform)ChildlenTransforms[i];

                    //変更するオブジェクト＝原点　でないかのチェック＆トリック判定
                    if (transform.gameObject != Pivot.gameObject && transform.GetComponent(typeof(PaddingTrick)) != null)
                    {
                        adoption(transform);
                    }
                    else
                    {
                        if (transform.gameObject == Pivot.gameObject && transform.GetComponent(typeof(PaddingTrick)) != null)
                        {
                            //Pivotと同一のオブジェクトは動かさないので無視、スケールだけセットする
                            transform.localScale = Vector3.Scale(transform.localScale, Scaling);
                        }
                    }
                }
            }
            else
            {

                for (int i = 1; i < ChildlenTransforms.Length; i++)
                {
                    Transform transform = (Transform)ChildlenTransforms[i];

                    //変更するオブジェクト＝原点　でないかのチェック
                    if (transform.gameObject != Pivot.gameObject)
                    {
                        //メッシュの付いていないオブジェクトの扱いについて
                        if (MeshOnly != true || transform.GetComponent(typeof(MeshRenderer)) || transform.GetComponent(typeof(SkinnedMeshRenderer)))
                        {
                            //メッシュの入れ子の扱いについて
                            if (SuruDoubleRenderer != true || transform.parent.GetComponent(typeof(MeshRenderer)) == null && transform.parent.GetComponent(typeof(SkinnedMeshRenderer)) == null)
                            {
                                adoption(transform);
                            }
                            else
                            {
                                //親にメッシュがいるので無視する
                            }
                        }
                        else
                        {
                            //メッシュが付いていないので無視
                        }

                    }
                    else
                    {
                        //Pivotと同一のオブジェクトは動かさないので無視、スケールだけセットする
                        transform.localScale = Vector3.Scale(transform.localScale, Scaling);
                    }
                }
            }
            Debug.Log("子オブジェクト間のパディングを完了します", TargetObjects);
        }

    }
    private void adoption(Transform transform)
    {
        //実際にPositionを書き換えるスクリプト

        //ピポットに弟子入り
        var parent = transform.parent;
        transform.parent = Pivot.transform;

        //成長期
        Vector3 setpos = Vector3.Scale(transform.localPosition, PaddingOffsets);
        transform.localPosition = setpos;

        transform.localScale = Vector3.Scale(transform.localScale, Scaling);

        //元の親に帰す
        transform.parent = parent;
    }
}

#endif