using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapGen
{
    public void Initialize();
    public IEnumerator Process();
}
