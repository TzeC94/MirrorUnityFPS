using Mirror;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameCore
{
    public static GameObject Instantiate(GameObject objectToSpawn, Vector3 position, Quaternion quaternion, Transform parent) {

        if(objectToSpawn is GameObject gameObject) {

            var poolable = gameObject.GetComponent<Poolable>();

            if (poolable != null) {

                var item = PoolManager.Pull(gameObject);
                item.transform.position = position;
                item.transform.rotation = quaternion;
                item.transform.SetParent(parent, true);
                return item;

            }

        } 

        return GameObject.Instantiate(objectToSpawn, position, quaternion, parent);
    }

    public static GameObject Instantiate(GameObject objectToSpawn, Transform parent) {

        return Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity, parent);

    }

    public static GameObject Instantiate(GameObject objectTospawn, Vector3 position) {

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

    public static void Destroy(GameObject target) {

        if (!PoolManager.Push(target)) {

            Object.Destroy(target);

        }

    }

}
