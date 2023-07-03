using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseGeneration : MapBaseData
{
    [Header("Map Content")]
    public MapBaseContentGeneration generationContent;

    public IEnumerator StartGeneration() { 
        
        yield return null;
    
    }

}
