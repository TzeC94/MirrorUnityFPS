using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectPlayer : MonoBehaviour
{
    public UnityEvent effectEvent;

    public void PlayerEffect() {

        effectEvent?.Invoke();

    }
}
