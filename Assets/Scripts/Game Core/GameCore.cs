using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameCore
{
    public static T Instantiate<T>(T objectToSpawn, Vector3 position, Quaternion quaternion, Transform parent) where T : Object {

        return (T)Object.Instantiate(objectToSpawn, position, quaternion, parent);

    }

    public static T Instantiate<T>(T objectToSpawn, Transform parent) where T : Object {

        return (T)Object.Instantiate(objectToSpawn, parent);

    }

}
