using System.Collections;

public class MapBaseContentGeneration : MapGenComponent
{
    public override IEnumerator Process() {

        int loopCount = 0;

        //Loop every of it and generate the content
        foreach(var baseLevel in MapGenData.bases) {

            var baseGeneration = baseLevel.GetComponent<MapBaseGeneration>();

            yield return baseGeneration.StartGeneration(loopCount);

            loopCount++;

            yield return null;

        }

    }
}