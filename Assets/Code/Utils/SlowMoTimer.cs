using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoTimer : MonoBehaviour
{
    [SerializeField] float smoothnessFadeIn;       //AS HIGHER FASTER IT GOES
    [SerializeField] float smoothnessFadeOut;      //AS HIGHER FASTER IT GOES
    [SerializeField] float waitTimeInLowestTimeScale; // The time at the lowest timescale
    [SerializeField] float lowestTimeScale; //The lowest timescale that reaches
    public void StartSlowMo()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (!Mathf.Approximately(Time.timeScale, lowestTimeScale))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, lowestTimeScale, smoothnessFadeIn * Time.deltaTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(waitTimeInLowestTimeScale*Time.timeScale);
        while (!Mathf.Approximately(Time.timeScale, 1.0f))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, smoothnessFadeOut * Time.deltaTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
