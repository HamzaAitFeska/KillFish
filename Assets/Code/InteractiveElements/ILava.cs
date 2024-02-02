using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ILava : MonoBehaviour
{
    [SerializeField] float damagePlayer = 10;
    [SerializeField] float timeBetweenDamageSpikes = 1;

    float elapsedTime;
    public static Action OnDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        elapsedTime = timeBetweenDamageSpikes;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= timeBetweenDamageSpikes) {
            GameController.GetGameController().GetPlayer().HealthSystem.TakeDamage(damagePlayer);
            elapsedTime = 0;
            OnDamage?.Invoke();
        }
    }
}
