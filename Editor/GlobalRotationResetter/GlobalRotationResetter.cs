using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class GlobalRotationResetter : EditorWindow
{
    public Transform TargetObjects = null;

    public Vector3 SetRotation;

    [MenuItem("PYEditor/GlobalRotationResetter")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<GlobalRotationResetter>("GlobalRotationResetter");
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            EditorGUILayout.PrefixLabel("設定");

            EditorGUILayout.Space(20);

            TargetObjects = (Transform)EditorGUILayout.ObjectField("ターゲットオブジェクトの親", TargetObjects, typeof(Transform), true);
            EditorGUILayout.HelpBox("ターゲットオブジェクトは１つの空きオブジェクトを親としてまとめ、指定できます。", MessageType.Info);

            EditorGUILayout.Space(20);

            SetRotation = EditorGUILayout.Vector3Field("角度設定", SetRotation);


            if (GUILayout.Button("レッツリセット！！"))
            {
                ResetRot();
            }
            EditorGUILayout.HelpBox("リセットされるのはResetrotTrickコンポーネントが付いたオブジェクトのみです。\n付いていないオブジェクトは無視されるので注意してください。", MessageType.Warning);
        }
    }

    private void ResetRot()
    {
        if (TargetObjects == null)
        {
            Debug.LogError("リセットするオブジェクトの親を指定してください Says PYEditor/GlobalRotationResetter");
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

                if (transform.GetComponent(typeof(ResetrotTrick)) != null)
                {
                    transform.rotation = Quaternion.Euler(SetRotation);
                }
                else
                {
                    //Trickコンポーネントが付いていないので無視
                }
            }
        }
    }
}

#endif