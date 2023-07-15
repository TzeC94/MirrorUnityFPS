using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseContentGeneration : MapGenComponent
{
    public override IEnumerator Process() {
        
        //Loop every of it and generate the content
        foreach(var baseLevel in MapGenData.bases) {

            var baseGeneration = baseLevel.GetComponent<MapBaseGeneration>();

            yield return baseGeneration.StartGeneration();

            yield return null;

        }

    }

    public override void Load() {
        throw new System.NotImplementedException();
    }

    public override void Save() {
        throw new System.NotImplementedException();
    }
}