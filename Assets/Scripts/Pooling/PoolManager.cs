using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static List<PoolWorker> poolWorkers = new List<PoolWorker>();

    public static GameObject Pull(GameObject prefab) {

        if (poolWorkers.Count == 0) {

            //Create a new list for this
            return CreateNewList(prefab);

        } else {

            //Loop through the list to find
            for (int i = 0; i < poolWorkers.Count; i++) {

                PoolWorker currentWorker;

                //Match Prefab Type
                if (poolWorkers[i].poolObject == prefab) {

                    currentWorker = poolWorkers[i];

                    var pulledObject = currentWorker.Pull();

                    return pulledObject;

                }

            }

            //Create a new list for this
            return CreateNewList(prefab);

        }

    }

    private static GameObject CreateNewList(GameObject targetPrefab) {

        //Create a new list for this
        var newWorker = new PoolWorker(targetPrefab);
        newWorker.workerID = poolWorkers.Count + 1;
        poolWorkers.Add(newWorker);
        var pulledObject = newWorker.Pull();

        return pulledObject;

    }

    public static bool Push(GameObject targetObject) {

        var poolable = targetObject.GetComponent<Poolable>();

        if(poolable != null) {

            var myPoolWorker = poolWorkers[poolable.poolWorkerID - 1];
            myPoolWorker.Push(targetObject);

            return true;

        } else {

            return false;

        }
        
    }

}
