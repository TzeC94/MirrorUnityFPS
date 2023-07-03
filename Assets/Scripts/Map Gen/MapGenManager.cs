/*
 * Please keep this on top all the time and follw up with MapGenComponent
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenManager : MonoBehaviour
{
    private MapGenComponent[] mapGenComponents;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGeneration();

        StartGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeGeneration() {

        //Find all
        mapGenComponents = GetComponents<MapGenComponent>();

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
