using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class StyleMeterCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Canvas hudCanvas;
    [SerializeField] TextMeshProUGUI letraText;
    [SerializeField] TextMeshProUGUI puntosText;
    [SerializeField] TextMeshProUGUI puntosTextNoRestados;
    [SerializeField] StyleScoreData scoreData;
    [SerializeField] PlayerStyleSystem playerStyleSystem;
    [SerializeField] TextMeshProUGUI multiplicadorVidaDaño;

    [SerializeField] GameObject CanvasControllerStyle;    

    [SerializeField] GameObject LetrasImage;
    [SerializeField] List<GameObject> StyleLettersUI;

    [SerializeField] List<TextMeshProUGUI> lastsKillsTextsList;

    [SerializeField] float fadeTimerMultiplier;

    [Header("Animations")]
    [SerializeField] Animation LetterRelatedAnimation;
    [SerializeField] AnimationClip LetterPop;
    [SerializeField] AnimationClip LetterPopReceiveDamage;
    [SerializeField] AnimationClip LetterPopKill;

    void Start()
    {        
        CanvasControllerStyle.SetActive(false);
    }
    void OnEnable()
    {
        PlayerHealth.OnPlayerHit += ReceiveDamageLetterPop;
        EnemySpawner.OnWaveStarted += SpawnStyleMeter;
        IEnemyHealth.OnEnemyDeath += KillLetterPop;
    }
    void OnDisable()
    {
        PlayerHealth.OnPlayerHit -= ReceiveDamageLetterPop;
        IEnemyHealth.OnEnemyDeath -= KillLetterPop;
        EnemySpawner.OnWaveStarted -= SpawnStyleMeter;
    }
    public void LetterPopChangeLetter()
    {
        LetterRelatedAnimation.Stop();
        LetterRelatedAnimation.Play(LetterPop.name);
    }
    public void KillLetterPop(IEnemyHealth enemy)
    {
        LetterRelatedAnimation.Stop();
        LetterRelatedAnimation.Play(LetterPopKill.name);
    }
    public void ReceiveDamageLetterPop()
    {
        LetterRelatedAnimation.Stop();
        LetterRelatedAnimation.Play(LetterPopReceiveDamage.name);
    }
    
    // Update is called once per frame
    void Update()
    {
        var currentStyle = scoreData.GetCurrentStyleState();
        int currentPlayerPoints = (int)scoreData.CurrentPlayerPoints();
        puntosText.text = currentPlayerPoints.ToString();
        puntosTextNoRestados.text = scoreData.PlayerTotalPoints().ToString();
        letraText.text = currentStyle.Texto;
        multiplicadorVidaDaño.text = "x " + currentStyle.Health_Damage.ToString();
        LetrasImage = currentStyle.Image;
        ActivateCurrentStyleLetter(currentStyle.Texto);

        LastKillTextUpdate();           

    }

    void ActivateCurrentStyleLetter(string currentLetter)
    {
        ResetStyleLettersUI();
        ActivateStyleLetterWithName(currentLetter);
    }

    void ResetStyleLettersUI()
    {
        foreach(var letter in StyleLettersUI)
        {
            letter.SetActive(false);
        }
    }

    void ActivateStyleLetterWithName(string gameObjectName)
    {
        GameObject letter = StyleLettersUI.FirstOrDefault(a => a.name == gameObjectName);
        if (letter == null)
            return;
        letter.SetActive(true);
        letter.transform.Find(gameObjectName).GetComponent<Image>().fillAmount = playerStyleSystem.GetFillAmount();
    }

    void LastKillTextUpdate()
    {
        for (int m = 0; m < lastsKillsTextsList.Count; m++)
        {
            lastsKillsTextsList[m].text = "";
        }

        int i = 0;
        foreach (var item in playerStyleSystem.textsToDisplay)
        {
            lastsKillsTextsList[i].text = item.StringToDisplay;
            //lastsKillsTextsList[i].color = item.TextColor;
            i++;
        }
    }    
    void SpawnStyleMeter(int wave)
    {
        CanvasControllerStyle.SetActive(true);
    }
   
}
