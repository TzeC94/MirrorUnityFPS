using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapBaseGeneration;

public static class MapGenData
{
    public enum MapBaseType { city, forest, future }

    public static List<GameObject> bases = new List<GameObject>();

    public static int seed;
    public static int baseCount;

    public static SortedDictionary<int, List<List<StructureData>>> baseContents = new SortedDictionary<int, List<List<StructureData>>>();

    #region Pack

    public static void Pack(out byte[] data) {

        data = null;    //Temporary set null

        //Prepare binary profile
        NetworkWriter writer = new NetworkWriter();
        writer.Write(seed);

        //Base count
        var baseCount = bases.Count;
        writer.Write(baseCount);

        //Base Data
        for (int i = 0; i < baseCount; i++) {   //Base count

            //How many spaned building/tree/etc in this base?
            var spawnedObjectInBaseCount = baseContents[i].Count;
            writer.Write(spawnedObjectInBaseCount);

            //Loop through the spawned object List and store it
            for(int j = 0; j < spawnedObjectInBaseCount; j++) {

                WriteSpawnedObjectInBase(writer, baseContents[i][j]);

            }

        }

    }

    private static void WriteSpawnedObjectInBase(NetworkWriter networkWritter, List<StructureData> objectList) {

        var buildingCount = objectList.Count;
        networkWritter.Write(buildingCount);

        //Store each data
        for (int j = 0; j < buildingCount; j++) {

            StructureData structureData = objectList[j];

            networkWritter.Write(structureData.prefabIndex);
            networkWritter.Write(structureData.position);
            networkWritter.Write(structureData.rotation);

        }

    }

    #endregion

    #region Unpack

    public static void Unpack(ref byte[] data) {

        NetworkReader reader = new NetworkReader(data);

        seed = reader.ReadInt();

        //Bases
        baseCount = reader.ReadInt();
        bases.Capacity = baseCount;

        for(int i = 0; i < baseCount; i++) {

            //How many list in this base
            var baseContentListCount = reader.ReadInt();

            baseContents.Add(i, new List<List<StructureData>>(baseContentListCount));

            //Load the spawned contest list
            for(int j = 0; j < baseContentListCount; j++) {

                ReadSpawnedObjectInBase(reader, baseContents[i][j]);

            }

        }

    }

    private static void ReadSpawnedObjectInBase(NetworkReader networkReader, List<StructureData> targetList) {

        var buildingCount = networkReader.ReadInt();
        targetList.Capacity = buildingCount;


        for (int i = 0; i < buildingCount; i++) {

            StructureData structureData;

            structureData.prefabIndex = networkReader.ReadInt();
            structureData.position = networkReader.ReadVector3();
            structureData.rotation = networkReader.ReadQuaternion();

            targetList[i] = structureData;
        }

    }

    #endregion
}
