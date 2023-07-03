using UnityEngine;
using System.Collections;

public class MapBasesGeneration : MapGenComponent
{
    public GameObject basePrefab;
    public int baseCountToGenerate = 4;
    public float baseGap = 20f;

    public override IEnumerator Process() {

        Vector3 startPos = Vector3.zero;

        for(int i = 0; i < baseCountToGenerate; i++) {

            var spawnedBase = GameCore.Instantiate(basePrefab, null);
            spawnedBase.transform.position = startPos;

            //Add it
            MapGenData.bases.Add(spawnedBase);

            //Increase it
            startPos += Vector3.forward * baseGap;

            yield return null;

        }

    }

}