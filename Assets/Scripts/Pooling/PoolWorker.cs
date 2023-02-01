using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PoolWorker {

    public int workerID;
    public GameObject poolObject;
    private IList<GameObject> poolList = new List<GameObject>();

    public PoolWorker(GameObject originalItem){

        poolObject = originalItem;

    }

    public void Push(GameObject item) {

        //Reset position
        item.transform.position = Vector3.zero;
        item.SetActive(false);

        var iPoolable = item.GetComponents<IPoolable>();
        foreach(var ipoolable in iPoolable) {
            ipoolable.Push();
        }

        poolList.Add(item);

    }   

    public GameObject Pull() {

        GameObject pullObject;

        if (poolList.Count <= 0) {

            pullObject = GameObject.Instantiate(poolObject) as GameObject;

        } else {

            pullObject = poolList[0];
            //Remove
            poolList.RemoveAt(0);

            var iPoolable = pullObject.GetComponents<IPoolable>();
            foreach (var ipoolable in iPoolable) {
                ipoolable.Pull();
            }
        }

        pullObject.SetActive(true);
        return pullObject;

    }
}