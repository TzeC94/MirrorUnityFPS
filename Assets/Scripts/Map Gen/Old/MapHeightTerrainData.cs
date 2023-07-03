using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using System;

[Serializable]
public class MapHeightTerrainData : MapTerrainData<float> {

    public float[,] dataArray;

    protected override int resolution => 513;

    private float valueIsFlat = 0.3f;

    public override IEnumerator Generation() {

        GenerateHeightMap();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        jobHandle.Complete();

        ConvertToArray();

        yield return new WaitForEndOfFrame();

        Apply();

    }

    private void GenerateHeightMap() {

        if (nativeData.IsCreated) {

            nativeData.Dispose();

        } else {

            nativeData = new NativeArray<float>(resolution * resolution, Allocator.Persistent);

        }

        nativeData = new NativeArray<float>(resolution * resolution, Allocator.Persistent);

        HeightGenerateJob heightFillJob = new HeightGenerateJob() {
            seed = MapGeneration.Instance.Seed,
            width = resolution,
            height = resolution,
            fillData = nativeData,
            valueAsFlat = valueIsFlat
        };

        Debug.Log($"Seed {MapGeneration.Instance.Seed}, width {resolution}, height {resolution}");

        jobHandle = heightFillJob.Schedule();

    }

    private struct HeightGenerateJob : IJob {

        public uint seed;
        public int width;
        public int height;
        public float valueAsFlat;
        public NativeArray<float> fillData;

        public void Execute() {

            var random = new Unity.Mathematics.Random(seed);
            var offset = random.NextFloat(math.floor(random.NextFloat(0f, 10000f)));

            for (int x = 0; x < width; x++) {

                for (int y = 0; y < height; y++) {

                    var index = (x * width) + y;
                    //This cnoise return value between -1 to 1
                    var noiseValue = noise.cnoise(new float2 { x = (x + offset) / 100f, y = (y + offset) / 100f });
                    var clampedValue = math.max(math.unlerp(-1f, 1f, noiseValue), valueAsFlat);

                    fillData[index] = clampedValue;

                }

            }

        }
    }

    protected override void Apply() {

        MapGeneration.Instance.terrainData.SetHeights(0, 0, dataArray);

    }

    protected override void ConvertToArray() {

        dataArray = new float[resolution, resolution];

        for (int x = 0; x < resolution; x++) {

            for (int y = 0; y < resolution; y++) {

                var index = (x * resolution) + y;
                var valueAtIndex = nativeData[index];
                dataArray[x, y] = valueAtIndex;

            }

        }

    }

}