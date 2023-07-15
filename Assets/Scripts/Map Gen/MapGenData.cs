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

    public static void Pack(out byte[] data) {

        data = null;    //Temporary set null

        //Prepare binary profile
        NetworkWriter writer = new NetworkWriter();
        writer.Write(seed);

        //Base count
        var baseCount = bases.Count;
        writer.Write(baseCount);

        //Base Position
        for(int i = 0; i < baseCount; i++) {

            writer.Write(bases[i].transform.position);

        }

        //Base Data
        for (int i = 0; i < baseCount; i++) {

            var baseLevel = bases[i].GetComponent<MapBaseGeneration>();

            writer.Write(baseLevel.baseType);

            //Building
            WriteSpawnedObjectInBase(writer, baseLevel.buildingData);

            //Tree
            WriteSpawnedObjectInBase(writer, baseLevel.treeData);

        }

    }

    private static void WriteSpawnedObjectInBase(NetworkWriter networkWritter, List<StructureData> objectList) {

        var buildingCount = objectList.Count;
        networkWritter.Write(buildingCount);

        for (int j = 0; j < buildingCount; j++) {

            StructureData structureData = objectList[j];

            networkWritter.Write(structureData.prefabIndex);
            networkWritter.Write(structureData.position);
            networkWritter.Write(structureData.rotation);

        }

    }

    public static void Unpack(ref byte[] data) {



    }
}
