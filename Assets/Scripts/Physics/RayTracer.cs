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
    private static Collider[] collision = new Collider[50];

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

    public static GameObject ObjectInFOV(Transform myTransform, float distance, float withinAngle, Collider[] colliderArray, int layerMask = Physics.DefaultRaycastLayers) {

        GameObject targetObject = null;

        var myPos = myTransform.position;
        var myForward = myTransform.forward;
        int inSphereCount = Physics.OverlapSphereNonAlloc(myPos, distance, colliderArray, layerMask);

        if(inSphereCount > 0) {

            float cachedDistance = 0f;

            for(int i = 0; i < inSphereCount; i++) {

                var sphereTarget = colliderArray[i];

                var targetPos = sphereTarget.transform.position;
                var targetDir = (targetPos - myPos).normalized;
                var angleValue = Vector3.Angle(targetDir, myForward);

                if(angleValue < withinAngle) {

                    if(targetObject == null) {

                        targetObject = sphereTarget.gameObject;
                        cachedDistance = Vector3.Distance(targetPos, myPos);

                    } else {

                        //Check distance
                        var betweenDistance = Vector3.Distance(targetPos, myPos);
                        if(betweenDistance < cachedDistance) {

                            cachedDistance = betweenDistance;
                            targetObject = sphereTarget.gameObject;

                        }

                    }

                }

            }

            return targetObject;

        }

        return null;

    }

    /// <summary>
    /// Check overlap and return the nearest
    /// </summary>
    /// <param name="myTransform"></param>
    /// <param name="size"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static Collider OverlapBox(Transform myTransform, Vector3 size, int layerMask = Physics.DefaultRaycastLayers) {

        var myPos = myTransform.position;
        var detectedSize = Physics.OverlapBoxNonAlloc(myPos, size, collision, myTransform.rotation, layerMask);

        if(detectedSize <= 0)
            return null;

        //Cache the first
        Collider targetObject = collision[0];
        float distance = Vector3.Distance(myPos, targetObject.transform.position);

        for(int i = 1; i < detectedSize; i++) {

            var currentObject = collision[i];
            var currentDistance = Vector3.Distance(myPos, currentObject.transform.position);

            if(distance > currentDistance) {

                distance = currentDistance;
                targetObject = currentObject;

            }

        }

        return targetObject;

    }

}
