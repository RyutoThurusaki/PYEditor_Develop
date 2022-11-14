using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MaterialRandamaizer : EditorWindow
{
    public Transform TargetObjects = null;

    public List<Material> MatArray = new List<Material>(0);

    [MenuItem("PYEditor/MaterialRandamaizer")]

    [SerializeField]
    private static void Create()
    {
        GetWindow<MaterialRandamaizer>("MaterialRandamaizer");
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope())
        {
            TargetObjects = (Transform)EditorGUILayout.ObjectField("ターゲット", TargetObjects, typeof(Transform), true);
            EditorGUILayout.HelpBox("ターゲットのオブジェクトは１つの空きオブジェクトを親としてまとめ、指定できます。", MessageType.Info);

            EditorGUILayout.Space(20);


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("＋"))
            {
                MatArray.Add(null);
            }

            if (GUILayout.Button("－"))
            {
                MatArray.RemoveAt(MatArray.Count - 1);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            for (int i = 0;i < MatArray.Count; i++)
            {
                MatArray[i] = (Material)EditorGUILayout.ObjectField("Material"+i, MatArray[i], typeof(Material), false);
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("レッツランダム！"))
            {
                Setmaterial();
            }
        }
    }

    private void Setmaterial()
    {
        if (TargetObjects != null)
        {
            if (MatArray.Any(value => value != null))
            {
            Component[] Targetchilds = TargetObjects.GetComponentsInChildren(typeof(Renderer));

            Undo.RecordObjects(Targetchilds, "Changed Material");

            for (int i = 0; i < Targetchilds.Length; i++)
            {
                Renderer me = (Renderer)Targetchilds[i].GetComponent(typeof(Renderer));
                me.material = MatArray[Random.Range(0, MatArray.Count)];
            }

            Debug.Log("ランダム化を完了します");
            }
            else
            {
                Debug.LogError("マテリアルにnullがあります Says PYEditor/MaterialRandamizer");
            }

        }
        else
        {
            Debug.LogError("ターゲットのオブジェクトが未指定です Says PYEditor/MaterialRandamizer");
        }
    }
}

#endif