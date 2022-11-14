using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "propplacerDB", menuName = "ScriptableObjects/PropPlacerDB")]
public class PropPlacerDB : ScriptableObject
{
    [SerializeField]
    string testString = "PropPlacerエディタ用のデータです";

    //Prefabselector
    public List<GameObject> SetPrefabs = new List<GameObject>(0);
    public List<Texture> PrefabTex = new List<Texture>(0);
    public int SelectPrefabNum = 0;

    //Spownsetting
    public List<Transform> PrefabPlacepoints = new List<Transform>(0);
    public Transform DefaultPoint;
}