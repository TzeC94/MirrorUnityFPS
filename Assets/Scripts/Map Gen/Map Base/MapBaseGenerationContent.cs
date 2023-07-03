using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Content", menuName = "Map Base/Base Content")]
public class MapBaseGenerationContent : ScriptableObject
{
    [System.Serializable]
    public struct BasePrefabs {
        public MapGenData.MapBaseType type;
        public GameObject[] buildingPrefab;
        public int buildingGenerationCount;
        public GameObject[] treePrefab;
        public int treeGenerationCount;
    }

    public BasePrefabs[] baseData;

}