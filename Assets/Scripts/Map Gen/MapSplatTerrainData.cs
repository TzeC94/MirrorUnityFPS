using System;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class MapSplatTerrainData : MapTerrainData<Color> {

    protected float[,,] dataArray;

    protected override void Apply() {

        //MapGeneration.Instance.terrainData.SetAlphamaps(0, 0, dataArray);

        ApplySplat();

    }

    public override IEnumerator Generation() {

        //GenerateSplat();

        //yield return new WaitForEndOfFrame();
        //yield return new WaitForEndOfFrame();

        //jobHandle.Complete();

        //yield return new WaitForEndOfFrame();

        //ConvertToArray();

        yield return new WaitForEndOfFrame();

        Apply();
    }

    private void GenerateSplat() {

        if (nativeData.IsCreated) {

            nativeData.Dispose();

        } else {

            nativeData = new NativeArray<Color>(resolution * resolution, Allocator.Persistent);

        }

        var heightData = MapGeneration.Instance.mapHeightGeneration.nativeData;
        
        SplatGenerateJob job = new SplatGenerateJob() {
            mapResolution = resolution,
            heightData = heightData,
            splatData = nativeData
        };

        jobHandle = job.Schedule();

    }

    private struct SplatGenerateJob : IJob {

        public int mapResolution;
        public NativeArray<float> heightData;
        public NativeArray<Color> splatData;

        public void Execute() {
            
            for(int x = 0; x < mapResolution; x++) {

                for(int y = 0; y < mapResolution; y++) {

                    //var currentIndex = (x * mapResolution) + y;
                    var currentIndex = y * mapResolution + x;

                    var xIndex1 = currentIndex;
                    var xIndex2 = (y + 1) * mapResolution + x;

                    var yIndex1 = y * mapResolution + x + 1;
                    var yIndex2 = (y + 1) * mapResolution + x + 1;

                    var heightValue = (heightData[xIndex1] + heightData[xIndex2] + heightData[yIndex1] + heightData[yIndex2]) / 4;
                    //var heightValue = (heightData[xIndex1] + heightData[yIndex2]) / 2;

                    //var r = math.unlerp(1f, 0f, heightValue);
                    var b = heightValue > 0.4f ? 0 : math.unlerp(0f, 0.4f, heightValue);
                    var g = heightValue >= 0.4f && heightValue <= 0.8f ? math.unlerp(0.8f, 4f, heightValue) : 0f;
                    var r = heightValue > 0.8f ? math.unlerp(1f, 0.8f, heightValue) : 0f;

                    var sum = r + g + b;
                    r /= sum;
                    g /= sum;
                    b /= sum;

                    //var r = 1f - heightValue;
                    //var g = heightValue;
                    //var b = 0.0f;

                    //var r = (float)currentIndex / (mapResolution * mapResolution);
                    //var g = 1 - (float)currentIndex / (mapResolution * mapResolution);
                    //var b = 0.0f;

                    //var r = x < mapResolution / 2? 1f:0f;
                    //var g = y < mapResolution /2? 0f:1f;
                    //var g = 0f;
                    //var b = 0.0f;

                    //currentIndex = ((mapResolution - x) * mapResolution ) - mapResolution + y;
                    currentIndex = (x * mapResolution) + y;
                    splatData[currentIndex] = new Color(r, g, b);

                }

            }

        }
    }

    protected override void ConvertToArray() {

        dataArray = new float[resolution, resolution, 3];

        for (int x = 0; x < resolution; x++) {

            for (int y = 0; y < resolution; y++) {

                var index = (x * resolution) + y;
                var valueAtIndex = nativeData[index];

                dataArray[x, y, 0] = valueAtIndex.r;
                dataArray[x, y, 1] = valueAtIndex.g;
                dataArray[x, y, 2] = valueAtIndex.b;

                //Debug.Log(valueAtIndex.r);

            }

        }

    }

    private void ApplySplat() {

        var terrainData = MapGeneration.Instance.terrainData;

        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        dataArray = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++) {
            for (int x = 0; x < terrainData.alphamapWidth; x++) {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));

                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = terrainData.GetSteepness(y_01, x_01);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

                // Texture[0] has constant influence
                splatWeights[0] = 0.5f;

                // Texture[1] is stronger at lower altitudes
                splatWeights[1] = Mathf.Clamp01((terrainData.heightmapResolution - height));

                // Texture[2] stronger on flatter terrain
                // Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
                // Subtract result from 1.0 to give greater weighting to flat surfaces
                splatWeights[2] = 1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapResolution / 5.0f));

                // Texture[3] increases with height but only on surfaces facing positive Z axis 
                //splatWeights[3] = height * Mathf.Clamp01(normal.z);

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights[0] + splatWeights[1] + splatWeights[2];

                // Loop through each terrain texture
                for (int i = 0; i < terrainData.alphamapLayers; i++) {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    dataArray[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps(0, 0, dataArray);

    }

}