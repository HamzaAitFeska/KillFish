using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleVanishHud : MonoBehaviour
{
    Animation m_animation;
    private void Start()
    {
        m_animation = GetComponent<Animation>();
    }
    private void OnEnable()
    {
        EnemySpawner.OnWaveFinished += StyleVanish;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWaveFinished -= StyleVanish;
    }

    void StyleVanish(int wave)
    {
        m_animation.Stop();
        m_animation.Play();
    }
}
