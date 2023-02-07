using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
    [SerializeField]
    private Animation anim;
    [SerializeField]
    private float duration = 1.5f;

    [Header("Position")]
    [SerializeField]
    private bool randomPositionOffset = true;
    [SerializeField]
    private float randomOffSetMin = -0.5f;
    [SerializeField]
    private float randomOffSetMax = -0.5f;

    public void SetText(string text) {

        textMesh.text = text;
        anim.Play();

        //Offset Position
        if (randomPositionOffset) {

            Vector3 randPos;
            randPos.x = Random.Range(randomOffSetMin, randomOffSetMax);
            randPos.y = Random.Range(randomOffSetMin, randomOffSetMax);
            randPos.z = 0.0f;

            //Apply
            transform.localPosition += randPos;

        }

        Invoke(nameof(DestroyItem), duration);

    }

    void DestroyItem() {

        GameCore.Destroy(gameObject);

    }

}
