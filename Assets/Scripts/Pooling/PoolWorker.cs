using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        }

        pullObject.SetActive(true);
        return pullObject;

    }
}