using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseData : MonoBehaviour
{
    public int width = 100;
    public int height = 20;

    public virtual void OnDrawGizmosSelected() {

        Gizmos.DrawWireCube(transform.position + Vector3.up * 2.5f, new Vector3(width, 5f, height));

    }

}
