using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameCore
{
    public static GameObject Instantiate(GameObject objectToSpawn, Vector3 position, Quaternion quaternion, Transform parent) {

        return GameObject.Instantiate(objectToSpawn, position, quaternion, parent);

    }

    public static GameObject Instantiate(GameObject objectToSpawn, Transform parent) {

        return GameObject.Instantiate(objectToSpawn, parent);

    }

}
