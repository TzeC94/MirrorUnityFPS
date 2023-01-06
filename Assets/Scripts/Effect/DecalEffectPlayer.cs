using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalEffectPlayer : EffectPlayer
{
    //Const
    private const string brickPhysc = "Floor";

    [Header("Decal")]
    public DecalProjector decalProjector;
    public Material decalMat_General;
    public Material decalMat_Brick;

    public void PlayDecal(DecalSpawner.MaterialType hitMaterial) {

        switch (hitMaterial) {
            case DecalSpawner.MaterialType.Brick:
                decalProjector.material = decalMat_Brick;
                break;
            default:
                decalProjector.material = decalMat_General;
                break;

        }

    }

    public override void Kill() {

        DecalSpawner.instance.DespawnDecal(gameObject);

    }
}
