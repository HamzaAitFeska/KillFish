using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class FinalUIStylePunctuation : MonoBehaviour
{
    [SerializeField] GameObject UI;

    [SerializeField] TextMeshProUGUI timeNumber;
    [SerializeField] TextMeshProUGUI pointsNumber;
    [SerializeField] GameObject[] LetterTimeStyleGO;
    [SerializeField] GameObject[] LetterPointsStyleGO;
    [SerializeField] GameObject[] FinalLetterGO;

    [SerializeField] LevelStyleData levelStyleData;
    float startTimeWave;
    float timeWave;

    [SerializeField] Animation finalPunctuationAnimation;
    [SerializeField] AnimationClip enterAnim;
    [SerializeField] AnimationClip exitAnim;

    bool isOpened = false;
    private void OnEnable()
    {
        EnemySpawner.OnWaveStarted += WaveStart;
        EnemySpawner.OnWaveFinished += WaveEnd;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWaveStarted -= WaveStart;
        EnemySpawner.OnWaveFinished -= WaveEnd;
    }
    private void Update()
    {
        if (isOpened)
        {
            if (GameController.GetGameController().GetPlayer().ActionManager.Jump())
            {
                SetExitAnimation();
            }
        }
    }

    void WaveStart(int wave)
    {
        startTimeWave = Time.time;
    }
    void WaveEnd(int wave)
    {
        timeWave = Time.time - startTimeWave;

        timeNumber.text = ConvertTimeToString(timeWave);
        pointsNumber.text = GameController.GetGameController().GetPlayer().StyleSystem.GetScoreData().PlayerTotalPoints().ToString();
        LetterTimeStyleGO[GetFinalTimePunctuation()].SetActive(true);
        LetterPointsStyleGO[GetFinalPointsPunctuation()].SetActive(true);
        FinalLetterGO[GetFinalResult(GetFinalTimePunctuation(), GetFinalPointsPunctuation())].SetActive(true);

        if(GetFinalTimePunctuation() == levelStyleData.timeLevelRanges.Length-1 &&
            GetFinalPointsPunctuation() == levelStyleData.timeLevelRanges.Length - 1)
        {
            ArchievementsNameConstantsData.ProfessionalKillfisherChecker();
        }

        Debug.Log("Time: "+timeWave+" , Time punctuation: "+GetFinalTimePunctuation());
        Debug.Log("Points: " + GameController.GetGameController().GetPlayer().StyleSystem.GetScoreData().PlayerTotalPoints().ToString() + " , Point punctuation: " + GetFinalPointsPunctuation());
        Debug.Log("FINAL STYLE " + GetFinalResult(GetFinalTimePunctuation(), GetFinalPointsPunctuation()).ToString());


        SetEnterAnimation();
    }
    public void SetEnterAnimation()
    {
        UI.SetActive(true);
        finalPunctuationAnimation.Play(enterAnim.name);
        GameController.GetGameController().GetPlayer().SetCanMove(false);
        Invoke("CanExitAnimation", enterAnim.length);
    }
    public void SetExitAnimation()
    {
        finalPunctuationAnimation.Play(exitAnim.name);
        UI.SetActive(false);
        GameController.GetGameController().GetPlayer().SetCanMove(true);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIHover, transform.position);
        isOpened = false;
    }
    public void CanExitAnimation()
    {
        isOpened = true;
    }
    public int GetFinalTimePunctuation()
    {
        int punct = 0;
        for (int i = 0; i < levelStyleData.timeLevelRanges.Length; i++)
        {
            if(timeWave >= levelStyleData.timeLevelRanges[i].x && timeWave < levelStyleData.timeLevelRanges[i].y)
            {
                punct = i;
                return i;
            }
        }
        return punct;
    }
    public int GetFinalPointsPunctuation()
    {
        int punct = 0;
        float points = GameController.GetGameController().GetPlayer().StyleSystem.GetScoreData().PlayerTotalPoints();
        for (int i = 0; i < levelStyleData.pointsLevelRanges.Length; i++)
        {
            if(points >= levelStyleData.pointsLevelRanges[i].x && points < levelStyleData.pointsLevelRanges[i].y)
            {
                punct = i;
                return i;
            }
        }
        return punct;
    }
    public int GetFinalResult(int timeResult, int styleResult)
    {
        float result = (timeResult + styleResult)/2f;
        return Mathf.CeilToInt(result);
    }
    private string ConvertTimeToString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formattedTime;
    }

    public void StyleFinalAppear()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.StyleFinalAppear,transform.position);
    }
    public void StyleFinalFramesLetter()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.StyleFinalFramesLetters,transform.position);
    }
    public void StyleFinalTimeAndPoints()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.StyleFinalTimeAndPoints,transform.position);
    }
    public void StyleFinalStyleChange()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.StyleFinalStyleChange, transform.position);
    }
}
