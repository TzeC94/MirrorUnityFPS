using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MapPoint : MonoBehaviour
{
    public static short Bridge = 1;

    [Flags]
    public enum PointType:short { None = 0, Bridge = 1 }
    
    public PointerType pointType;
}
