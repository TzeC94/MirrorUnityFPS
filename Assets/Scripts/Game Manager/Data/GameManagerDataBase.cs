using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManagerDataBase: IDataPacker
{
    public virtual void Pack(out byte[] data) {

        data = null;

    }

    public virtual void Unpack(ref byte[] data) {

    }
}
