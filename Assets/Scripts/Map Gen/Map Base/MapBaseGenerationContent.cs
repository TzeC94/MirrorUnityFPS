using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Content", menuName = "Map Base/Base Content")]
public class MapBaseGenerationContent : ScriptableObject
{
    [System.Serializable]
    public struct BasePrefabs {
        public GameObject[] buldingPrefab;
        public GameObject[] treePrefab;
    }

    [Header("City")]
    public BasePrefabs cityPrefabData;

    [Header("Forest")]
    public BasePrefabs forestPrefabData;

    [Header("Future")]
    public BasePrefabs futurePrefabData;

}