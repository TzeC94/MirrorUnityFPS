/*
 * Please keep this on top all the time and follw up with MapGenComponent
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenManager : MonoBehaviour
{
    public int seed = 5;

    private IMapGen[] mapGenComponents;

    // Start is called before the first frame update
    void Start()
    {
        MapGenData.seed = seed;

        Random.InitState(seed);

        InitializeGeneration();

        StartGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeGeneration() {

        //Find all
        mapGenComponents = GetComponents<IMapGen>();

    }

    private void StartGeneration() {

        if (mapGenComponents.Length == 0)
            return;

        StartCoroutine(GenerationProcess());

    }

    IEnumerator GenerationProcess() {

        for (int i = 0; i < mapGenComponents.Length; i++) {

            var cMapGenComponent = mapGenComponents[i];

            cMapGenComponent.Initialize();

            yield return cMapGenComponent.Process();

            yield return null;

        }

    }

}
