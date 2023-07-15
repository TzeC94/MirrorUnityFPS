using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseGeneration : MapBaseData
{
    public struct StructureData {

        public int prefabIndex;
        public Vector3 position;
        public Quaternion rotation;

    }

    [Header("Map Content")]
    public MapBaseGenerationContent generationContent;
    public MapGenData.MapBaseType baseType { get; private set; }
    private const float ovarlapCheckRange = 2f;
    private const float overlapCheckYOffset = 2.5f;

    public List<StructureData> buildingData = new List<StructureData>();
    public List<StructureData> treeData = new List<StructureData>();

    public IEnumerator StartGeneration() { 

        var length = generationContent.baseData.Length;
        var randomBaseType = generationContent.baseData[Random.Range(0, length)];

        //Lets random a base type
        baseType = randomBaseType.type;

        //building
        RandomSpawn(randomBaseType.buildingGenerationCount, randomBaseType.buildingPrefab, buildingData);

        //Tree
        RandomSpawn(randomBaseType.treeGenerationCount, randomBaseType.treePrefab, treeData);

        yield return null;
    
    }

    private void RandomSpawn(int generationCount, GameObject[] prefabList, List<StructureData> dataList) {

        //Start with building
        for (int i = 0; i < generationCount; i++) {

            var posX = width / 2f * Random.Range(-1f, 1f);
            var posZ = height / 2f * Random.Range(-1f, 1f);

            //Do an overlap check make sure it wont stack
            if (RayTracer.OverlapSphere(transform.position + new Vector3(posX, overlapCheckYOffset, posZ), ovarlapCheckRange) != null)
                continue;

            var index = Random.Range(0, prefabList.Length);

            //SPAWN
            var buildingPrefab = prefabList[index];

            var position = transform.position + new Vector3(posX, 0f, posZ);
            var rotation = Quaternion.identity;

            GameCore.Instantiate(buildingPrefab, position, rotation, transform);

            dataList.Add(new StructureData { prefabIndex = index, 
                position = position, rotation = rotation });
        }

    }

}