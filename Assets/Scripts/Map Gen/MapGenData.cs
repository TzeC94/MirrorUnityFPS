using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenData
{
    public enum MapBaseType { city, forest, future }

    public static List<GameObject> bases = new List<GameObject>();

    public static int seed;
}
