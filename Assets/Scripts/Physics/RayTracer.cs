using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * It is better to extend from Unity function, incase we want to modify or extend the default behaviour
 */

public static class RayTracer
{
    public static int RaycastNonAlloc(ref Ray ray, ref RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers) {

        return Physics.RaycastNonAlloc(ray, results, maxDistance, layerMask);

    }
}
