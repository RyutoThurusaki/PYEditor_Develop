using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class RotationRandamizer : EditorWindow
{
    public Transform TargetObjects = null;

    public Vector3 SnapRotation;


    [MenuItem("PYEditor/RotationRandamizer")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<RotationRandamizer>("RotationRandamizer");
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            EditorGUILayout.PrefixLabel("設定");

            EditorGUILayout.Space(20);

            TargetObjects = (Transform)EditorGUILayout.ObjectField("ターゲットオブジェクトの親", TargetObjects, typeof(Transform), true);
            EditorGUILayout.HelpBox("ターゲットのオブジェクトは１つの空きオブジェクトを親としてまとめ、指定できます。", MessageType.Info);

            EditorGUILayout.Space(20);

            EditorGUILayout.PrefixLabel("角度指定");

            SnapRotation = EditorGUILayout.Vector3Field("スナップRotation", SnapRotation);
            EditorGUILayout.HelpBox("指定した角度×0～360のランダムな値　が最終的なRotationになります", MessageType.Info);

            if (GUILayout.Button("レッツランダム！！"))
            {
                Randamize();
            }
        }
    }

    private void Randamize()
    {
        if (TargetObjects == null)
        {
            Debug.LogError("ランダム化するオブジェクトの親を指定してください Says PYEditor/GlobalRotationResetter");
        }
        else
        {
            //子供のオブジェクトを全取得
            Component[] ChildlenTransforms = TargetObjects.GetComponentsInChildren(typeof(Transform));

            //子供のQuatanionをランダム化する i=1なのは親自身を省くため
            Undo.RecordObjects(ChildlenTransforms, "Changed children rotation");

            for (int i = 1; i < ChildlenTransforms.Length; i++)
            {
                Transform transform = (Transform)ChildlenTransforms[i];

                Vector3 resultrot = SnapRotation * Random.Range(0, 360);

                transform.rotation = Quaternion.Euler(resultrot);

            }
        }
    }
}

#endif