using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSpawner : NetworkBehaviour
{
    public enum MaterialType { General, Brick}

    //Singleton
    public static DecalSpawner instance;

    public GameObject decalEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) {

            instance = this;
            return;
        }

        Destroy(gameObject);
    }

    private void OnDisable() {

        instance = null;

    }

    [Server]
    public void SpawnDecal(Collider hitCollider, Vector3 position, Quaternion rotation) {

        //Spawn
        var matType = GetMaterialType(hitCollider);
        RPC_SpawnDecal(matType, position, rotation);

    }

    [Server]
    private MaterialType GetMaterialType(Collider hitCollider) {

        var physMat = hitCollider.material;

        switch (physMat.name) {
            case "Floor":
                return MaterialType.Brick;
            default:
                return MaterialType.General;
        }

    }

    [ClientRpc]
    private void RPC_SpawnDecal(MaterialType materialType, Vector3 position, Quaternion rotation) {

        //Spawn
        var decal = GameCore.Instantiate(decalEffectPrefab, position, rotation, null);
        var decalEffect = decal.GetComponent<DecalEffectPlayer>();
        decalEffect.PlayDecal(materialType);
        decalEffect.PlayerEffect();

    }

    [Client]
    public void DespawnDecal(GameObject decalObject) {

        //Pooling in future???
        Destroy(decalObject);

    }
}
