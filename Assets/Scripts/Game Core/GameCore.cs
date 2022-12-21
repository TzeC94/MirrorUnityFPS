using Mirror;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameCore
{
    public static T Instantiate<T>(T objectToSpawn, Vector3 position, Quaternion quaternion, Transform parent) where T : Object {

        return (T)Object.Instantiate(objectToSpawn, position, quaternion, parent);

    }

    public static T Instantiate<T>(T objectToSpawn, Transform parent) where T : Object {

        return Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity, parent);

    }

    public static T Instantiate<T>(T objectTospawn, Vector3 position) where T : Object {

        return Instantiate(objectTospawn, position, Quaternion.identity, null);

    }

    public static GameObject NetworkInstantiate(GameObject objectToSpawn) {

        return NetworkInstantiate(objectToSpawn, Vector3.zero, Quaternion.identity);

    }

    public static GameObject NetworkInstantiate(GameObject objectToSpawn, NetworkConnectionToClient ownerConnection) {

        return NetworkInstantiate(objectToSpawn, Vector3.zero, Quaternion.identity, ownerConnection);

    }

    public static GameObject NetworkInstantiate(GameObject objectToSpawn, Vector3 position) {

        return NetworkInstantiate(objectToSpawn, position, Quaternion.identity);

    }

    public static GameObject NetworkInstantiate(GameObject objectToSpawn, Vector3 position, Quaternion rotation) {

        return NetworkInstantiate(objectToSpawn, position, rotation, null);

    }

    public static GameObject NetworkInstantiate(GameObject objectToSpawn, Vector3 position, Quaternion rotation, NetworkConnectionToClient ownerConnection) {

        var spawnedObject = GameObject.Instantiate(objectToSpawn, position, rotation);
        NetworkServer.Spawn(spawnedObject, ownerConnection);

        return spawnedObject;

    }

    public static GameObject DropItemNetworkInstantiate(GameObject objectToSpawn, Vector3 position, Item item) {

        var droppedItem = Instantiate(objectToSpawn, position);
        droppedItem.GetComponent<PickupBase>().itemData = item;

        NetworkServer.Spawn(droppedItem);

        return droppedItem;

    }

}
