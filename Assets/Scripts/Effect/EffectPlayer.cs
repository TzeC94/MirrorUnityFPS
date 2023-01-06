using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectPlayer : MonoBehaviour
{
    [Header("Duration")]
    public bool useDuration = false;
    public float duration = 5f;

    [Header("Event")]
    public UnityEvent effectEvent;

    public virtual void PlayerEffect() {

        if (useDuration) {

            Invoke(nameof(Kill), duration);

        }

        effectEvent?.Invoke();

    }

    public virtual void Kill() {

        Destroy(gameObject);

    }
}
