/*
 * This the Abstract component that will get process by Map Gen Manager to generate the level
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenComponent : MonoBehaviour
{
    public virtual void Initialize() {

    }

    public abstract IEnumerator Process();

}
