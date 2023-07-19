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

    private const int contentGenerationCount = 2;
    public List<StructureData> buildingData = new List<StructureData>();
    public List<StructureData> treeData = new List<StructureData>();

    public IEnumerator StartGeneration(int baseNo) {

        //Generate content
        if (GameManagerBase.instance.isServer) {

            var length = generationContent.baseData.Length;
            var randomBaseType = generationContent.baseData[Random.Range(0, length)];

            //Lets random a base type
            baseType = randomBaseType.type;
            MapGenData.mapBasesType.Add((int)baseType);

            List<List<StructureData>> baseContent = null;

            if(!MapGenData.baseContents.ContainsKey(baseNo)) {

                MapGenData.baseContents.Add(baseNo, new List<List<StructureData>>(contentGenerationCount));
                baseContent = MapGenData.baseContents[baseNo];

                for(int i = 0; i < contentGenerationCount; i++) {

                    baseContent.Add(new List<StructureData>());

                }

            } else {

                baseContent = MapGenData.baseContents[baseNo];

            }

            //building
            RandomSpawn(randomBaseType.buildingGenerationCount, randomBaseType.buildingPrefab, baseContent[0]);

            //Tree
            RandomSpawn(randomBaseType.treeGenerationCount, randomBaseType.treePrefab, baseContent[1]);

        }

        //Restore the spawned content
        if (GameManagerBase.instance.isClient) {

            baseType = (MapGenData.MapBaseType)MapGenData.mapBasesType[baseNo];

            //Get my base data
            var spawnList = MapGenData.baseContents[baseNo];

            //Base related prefab list
            var mapGenBaseContents = generationContent.baseData[(int)baseType];

            //Building
            var buildingList = spawnList[0];
            ClientSpawn(buildingList, mapGenBaseContents.buildingPrefab);

            //Tree
            var treeList = spawnList[1];
            ClientSpawn(treeList, mapGenBaseContents.treePrefab);

        }

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

    private void ClientSpawn(List<StructureData> spawningList, GameObject[] prefabList) {

        var count = spawningList.Count;

        if (count == 0) return;

        for(int i = 0; i < count; i++) {

            var spawningData = spawningList[i];
            var prefab = prefabList[spawningData.prefabIndex];
            var pos = spawningData.position;
            var rotation = spawningData.rotation;

            GameCore.Instantiate(prefab, pos, rotation, transform);

        }

    }

}