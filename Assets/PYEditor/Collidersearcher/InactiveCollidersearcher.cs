using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class InactiveCollidersearcher : EditorWindow
{
    public Transform TargetObjects = null;

    [MenuItem("PYEditor/InactiveCollidersearcher")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<InactiveCollidersearcher>("InactiveCollidersearcher");
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            EditorGUILayout.PrefixLabel("設定");

            EditorGUILayout.Space(20);

            TargetObjects = (Transform)EditorGUILayout.ObjectField("ターゲットオブジェクトの親",TargetObjects, typeof(Transform), true);
            EditorGUILayout.HelpBox("検索先のオブジェクトは１つの空きオブジェクトを親としてまとめ、指定できます。", MessageType.Info);

            EditorGUILayout.Space(20);

            

            EditorGUILayout.Space(20);

            if (GUILayout.Button("レッツサーチ！！"))
            {
                Search();
            }
        }
    }

    private void Search()
    {
        if (TargetObjects == null)
        {
            Debug.LogError("ターゲットを指定してください。 Says PYEditor/InactiveCollidersearcher");
        }
        else
        {
            //コライダー探し
            Collider[] collide = TargetObjects.GetComponentsInChildren<Collider>();

            for (int i = 0; i < collide.Length; i++)
            {

                if(collide[i].enabled == false)
                {
                    Debug.Log("アーニャ知ってる、このコライダーinactiveなってる。　Says PYEditor/InactiveCollidersearcher", collide[i]);
                }
            }
        }

    }
}

#endif