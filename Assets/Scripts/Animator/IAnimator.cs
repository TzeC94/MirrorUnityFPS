using Mirror;
using UnityEngine;

public interface IAnimator
{
    public NetworkBehaviour MyNetworkBehaviour();

    public Vector3 Velocity();

    public bool OnGround();
}