using System;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[Serializable]
public abstract class MapTerrainData<T> where T : struct
{
    public NativeArray<T> nativeData;
    protected JobHandle jobHandle;
    protected virtual int resolution => 512;
    public int Resolution {
        get { return resolution; }
    }

    public abstract IEnumerator Generation();

    protected abstract void ConvertToArray();

    protected abstract void Apply();

    public virtual void Dispose() {

        if(nativeData.IsCreated) {

            nativeData.Dispose();

        }

    }

}