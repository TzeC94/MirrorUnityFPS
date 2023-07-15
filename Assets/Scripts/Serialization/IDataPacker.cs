using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPacker
{
    void Pack(out byte[] data);
    void Unpack(ref byte[] data);
}
