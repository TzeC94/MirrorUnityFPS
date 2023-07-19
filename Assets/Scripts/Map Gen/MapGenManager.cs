/*
 * Please keep this on top all the time and follw up with MapGenComponent
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenManager : MonoBehaviour
{
    public static MapGenManager Instance { get { return instance; } }
    private static MapGenManager instance;

    public int seed = 5;

    public IMapGen[] mapGenComponents;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            return;

        MapGenData.seed = seed;

        Random.InitState(seed);

        InitializeGeneration();

        //StartGeneration();
    }

    private void OnDisable() {

        instance = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeGeneration() {

        //Find all
        mapGenComponents = GetComponents<IMapGen>();

    }

    public void StartGeneration() {

        if (mapGenComponents.Length == 0)
            return;

        StartCoroutine(GenerationProcess());

    }

    public IEnumerator GenerationProcess() {

        for (int i = 0; i < mapGenComponents.Length; i++) {

            var cMapGenComponent = mapGenComponents[i];

            cMapGenComponent.Initialize();

            yield return cMapGenComponent.Process();

            yield return null;

        }

    }

}
