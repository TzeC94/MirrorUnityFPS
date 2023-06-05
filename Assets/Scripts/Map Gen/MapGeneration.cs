using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;

public class MapGeneration : MonoBehaviour {

    [Header("Map Size")]
    [SerializeField] private int width = 512;
    [SerializeField] private int lengthPerSector = 256;
    [SerializeField] private int sectorCount = 4;
    [SerializeField] private float maxHeight = 5f;
    [SerializeField] private GameObject defaultTerrain;
    private GameObject createdTerrainGameObject;
    private Terrain createdTerrainComponent;
    private TerrainData createdTerrainData;

    //[Header("Height Map")]
    private NativeArray<float> nativeHeightMapData;
    private JobHandle heightMapJob;
    private float[,] heightArray;
    private const int heightMapResolution = 513;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MapGenCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MapGenCoroutine() {

        bool result;

        result = GenerateTerrain();

        if (result == false) yield break;

        yield return new WaitForEndOfFrame();

        GenerateHeightMap();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        heightMapJob.Complete();

        FillHeightMap();

        yield return new WaitForEndOfFrame();

        ApplyHeight();
    }

    #region Terrain

    private bool GenerateTerrain() {

        //Craete Terrain First
        createdTerrainGameObject = Instantiate(defaultTerrain);

        if (createdTerrainGameObject == null) return false;

        createdTerrainComponent = defaultTerrain.GetComponent<Terrain> ();
        
        if(createdTerrainComponent == null) return false;

        createdTerrainData = createdTerrainComponent.terrainData;

        var finalSectorLength = GetFinalSectorLength();

        createdTerrainData.size = new Vector3(width, maxHeight, finalSectorLength);

        return true;
    }

    #endregion

    #region Height Map

    private bool GenerateHeightMap() {

        var finalSectorLength = GetFinalSectorLength();

        nativeHeightMapData = new NativeArray<float>(heightMapResolution * heightMapResolution, Allocator.TempJob);

        HeightGenerateJob heightFillJob = new HeightGenerateJob() {
            width = heightMapResolution,
            height = heightMapResolution,
            fillData = nativeHeightMapData
        };

        heightMapJob = heightFillJob.Schedule();

        return true;

    }

    private struct HeightGenerateJob : IJob {

        public int width;
        public int height;
        public NativeArray<float> fillData;

        public void Execute() {

            for(int x = 0; x < width; x++) {

                for(int y = 0; y < height; y++) {

                    var index = x * width + y;
                    var noiseValue = noise.cnoise(new float2 { x = x / 100f, y = y / 100f });
                    var clampedValue = math.unlerp(-1f, 1f, noiseValue);

                    fillData[index] = clampedValue;

                }

            }

        }
    }

    private void FillHeightMap() {

        var finalSectorLength = GetFinalSectorLength();

        heightArray = new float[heightMapResolution, heightMapResolution];

        for(int x = 0; x < heightMapResolution; x++) {

            for (int y = 0; y < heightMapResolution; y++) {

                var index = x * heightMapResolution + y;
                var valueAtIndex = nativeHeightMapData[index];
                heightArray[x, y] = valueAtIndex;

            }

        }

        //Dispose the native array
        nativeHeightMapData.Dispose();
    }

    private void ApplyHeight() {

        createdTerrainData.SetHeights(0, 0, heightArray);

    }

    #endregion

    private int GetFinalSectorLength() {

        return Mathf.NextPowerOfTwo(lengthPerSector * sectorCount);

    }

    #region Context Menu

    [ContextMenu("Create Default Terrain")]
    public void CreateDefaultTerrain() {

        GenerateTerrain();

    }

    #endregion

}