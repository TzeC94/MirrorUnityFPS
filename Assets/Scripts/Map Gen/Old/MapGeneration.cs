using MyBox;
using System.Collections;
using UnityEngine;

public class MapGeneration : MonoBehaviour {

    //Singleton
    public static MapGeneration Instance;

    [Header("Seed")]
    [SerializeField] private uint seed = 5;
    public uint Seed {  get { return seed; } }

    [Header("Map Size")]
    [SerializeField] private int width = 512;
    [SerializeField] private int lengthPerSector = 256;
    [SerializeField] private int sectorCount = 4;
    [SerializeField] private float maxHeight = 5f;
    [SerializeField] private GameObject defaultTerrain;
    private GameObject createdTerrainGameObject;
    private Terrain createdTerrainComponent;
    public Terrain terrain { get { return createdTerrainComponent; } }
    private TerrainData createdTerrainData;
    public TerrainData terrainData { get { return createdTerrainData; } }

    //Height Map
    public MapHeightTerrainData mapHeightGeneration;

    //Splat
    public MapSplatTerrainData mapSplatGeneration;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance !=  null) {

            Destroy(Instance.gameObject);

        } else {

            Instance = this;

        }

        mapHeightGeneration = new MapHeightTerrainData();
        mapSplatGeneration = new MapSplatTerrainData();

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

        //Height Map
        yield return mapHeightGeneration.Generation();

        yield return new WaitForEndOfFrame();

        //Splat Map
        yield return mapSplatGeneration.Generation();

        yield return new WaitForEndOfFrame();

        EndOfTerrainGeneration();
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

    private int GetFinalSectorLength() {

        return Mathf.NextPowerOfTwo(lengthPerSector * sectorCount);

    }

    private void EndOfTerrainGeneration() {

        mapHeightGeneration.Dispose();
        mapSplatGeneration.Dispose();

    }

#if UNITY_EDITOR

    #region Context Menu

    [ButtonMethod()]
    public void CreateDefaultTerrain() {

        if(createdTerrainGameObject != null) {
            DestroyImmediate(createdTerrainGameObject);
        }

        StartCoroutine(MapGenCoroutine());

    }

    [ButtonMethod()]
    public void RegenerateHeight() {

        StartCoroutine(Editor_RegenHeight());

    }

    IEnumerator Editor_RegenHeight() {

        yield return new WaitForEndOfFrame();
        yield return mapHeightGeneration.Generation();

    }

    #endregion

#endif

}