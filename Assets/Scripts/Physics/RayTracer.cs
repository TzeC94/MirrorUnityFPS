using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * It is better to extend from Unity function, incase we want to modify or extend the default behaviour
 */

public static class RayTracer
{
    public static int RaycastNonAlloc(Ray ray, ref RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers) {

        return Physics.RaycastNonAlloc(ray, results, maxDistance, layerMask, QueryTriggerInteraction.Ignore);

    }

    private static RaycastHit[] hit = new RaycastHit[50];

    public static GameObject RaycastNonAllocNearest(Ray ray, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers) {

        int hitCount = RaycastNonAlloc(ray, ref hit, maxDistance, layerMask);

        Transform nearest = null;

        if(hitCount > 0) {

            nearest = hit[0].transform;

            if(hitCount > 1) {

                var distance = Vector3.Distance(ray.origin, nearest.position);

                for (int i = 1; i < hitCount; i++) {

                    var cHitObject = hit[i].transform;
                    var cDistance = Vector3.Distance(ray.origin, cHitObject.position);

                    if (distance > cDistance) {

                        distance = cDistance;
                        nearest = cHitObject;

                    }

                }

            } 

            return nearest.gameObject;

        }

        return null;

    }

    public static GameObject RaycastNonAllocNearest(Ray ray, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers) {

        int hitCount = RaycastNonAlloc(ray, ref hit, maxDistance, layerMask);

        Transform nearest = null;

        if (hitCount > 0) {

            var currentHit = hitInfo = hit[0];
            nearest = hitInfo.transform;

            if (hitCount > 1) {

                var distance = currentHit.distance;

                for (int i = 1; i < hitCount; i++) {

                    var cHit = hit[i];
                    var cHitObject = cHit.transform;
                    var cDistance = cHit.distance;

                    if (distance > cDistance) {

                        distance = cDistance;
                        nearest = cHitObject;
                        hitInfo = hit[i];

                    }

                }

            }

            return nearest.gameObject;

        }

        hitInfo = new RaycastHit();
        return null;

    }

}
