using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseGeneration : MapBaseData
{
    [Header("Map Content")]
    public MapBaseGenerationContent generationContent;
    private MapGenData.MapBaseType baseType; 

    public IEnumerator StartGeneration() { 

        var length = generationContent.baseData.Length;
        var randomBaseType = generationContent.baseData[Random.Range(0, length)];

        //Lets random a base type
        baseType = randomBaseType.type;

        //building
        RandomSpawn(randomBaseType.buildingGenerationCount, randomBaseType.buildingPrefab);

        //Tree
        RandomSpawn(randomBaseType.treeGenerationCount, randomBaseType.treePrefab);

        yield return null;
    
    }

    private void RandomSpawn(int generationCount, GameObject[] prefabList) {

        //Start with building
        for (int i = 0; i < generationCount; i++) {

            var posX = width / 2f * Random.Range(-1f, 1f);
            var posZ = height / 2f * Random.Range(-1f, 1f);

            //Do an overlap check make sure it wont stack
            if (RayTracer.OverlapSphere(transform.position + new Vector3(posX, 2.5f, posZ), 2f) != null)
                continue;

            //SPAWN
            var buildingPrefab = prefabList[Random.Range(0, prefabList.Length)];

            GameCore.Instantiate(buildingPrefab, transform.position + new Vector3(posX, 0f, posZ), Quaternion.identity, transform);

        }

    }

}