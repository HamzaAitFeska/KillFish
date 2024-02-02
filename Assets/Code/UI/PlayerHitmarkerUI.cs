using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHitmarkerUI : MonoBehaviour
{
    //Action for when wants to Hitmark
    public static Action OnHitmark;

    // Timers to not spam hitmark
    float elapsedTimeFromLastHitmark;
    [SerializeField] float minTimeBetweenHitMarkers = 0.2f;

    //Resources for hitmarking
    [SerializeField] Animation hitmarkAnimation;


    private void Start()
    {
        elapsedTimeFromLastHitmark = minTimeBetweenHitMarkers;
    }
    private void OnEnable()
    {
        OnHitmark += Hitmark;
    }
    private void OnDisable()
    {
        OnHitmark -= Hitmark;
    }
    private void Update()
    {
        if(!CanHitmarkByTime())
        {
            elapsedTimeFromLastHitmark += Time.deltaTime;
        }
    }
    void Hitmark()
    {
        if (CanHitmarkByTime())
        {
            elapsedTimeFromLastHitmark = 0.0f;
            hitmarkAnimation.Stop();
            hitmarkAnimation.Play();
            //Play 2D sound event of hitmark
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.HitmarkerAudio, transform.position);
        }
    }
    bool CanHitmarkByTime()
    {
        return elapsedTimeFromLastHitmark >= minTimeBetweenHitMarkers;
    }
}
