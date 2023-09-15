using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapPoint;

public class MapPointCollection : MonoBehaviour
{
    public MapPoint[] mapPoints;

    public bool ContainType(PointType pointType) {

        foreach (var point in mapPoints) {
            
            if(point.ContainType(pointType)) {
                return true;
            }

        }

        return false;

    }

#if UNITY_EDITOR

    private void OnValidate() {
        
        mapPoints = gameObject.GetComponentsInChildren<MapPoint>();

    }

#endif
}
