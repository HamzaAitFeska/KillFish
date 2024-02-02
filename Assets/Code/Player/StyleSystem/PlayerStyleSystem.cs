using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStyleSystem : MonoBehaviour
{
    [Header("ScoreData")]
    [SerializeField] StyleScoreData scoreData;
    int shotgunKills = 0;

    public StyleScoreData GetScoreData() { return scoreData; }

    [Header("BasicKills")]
    [SerializeField] float resetTimeToKill; //Variable que usamos para el tiempo de pasar de kill a doublekill.
    float timeToKill;
    bool activeTimer;
    int enemiesKilled;

    [Header("HookKill")]
    [SerializeField] float resetHookTimeToKill;
    float hookTimeToKill;
    bool CanHookKill() { return hookTimeToKill > 0; }
    int hookKills = 0;

    [Header("DashKill")]
    [SerializeField] float resetDashTimeToKill;
    float dashTimeToKill;
    bool CanDashKill() { return dashTimeToKill > 0; }

    [Header("DistanceToEnemy")]
    [SerializeField] float overkillDistance;

    [Header("AirHookKill")]
    bool airkillHook;

    bool hasAddedPoints; //If another method is called, don't call Kill

    public LinkedList<DisplayText> textsToDisplay;

    [SerializeField] float maximunNumberOfActions;    

    PlayerVariableContainer playerVariableContainer;

    public StyleMeterCanvas styleCanvas;

    [SerializeField] GameObject healIconPrefab;

    [SerializeField] Animation textLetrasAnimation;
    
    private void Start()
    {
        activeTimer = false;
        airkillHook = false;
        playerVariableContainer = GetComponent<PlayerVariableContainer>();
        scoreData.SetCurrentPlayerPoints(0);
        textsToDisplay = new LinkedList<DisplayText>();
        scoreData.SetCurrentStyleState(GetCurrentStyle());
        scoreData.SetPlayerTotalPoints(0);

    }
    private void OnEnable()
    {
        IEnemyHealth.OnEnemyDeath += EnemyKill2;
        FPSMoveController.OnPlayerLanded += PlayerLanded;
        PlayerHealth.OnPlayerHit += LosingPoints;
        EnemySpawner.OnWaveFinished += ArchievementsRelated;
    }
    private void OnDisable()
    {
        IEnemyHealth.OnEnemyDeath -= EnemyKill2;
        FPSMoveController.OnPlayerLanded -= PlayerLanded;
        PlayerHealth.OnPlayerHit -= LosingPoints; 
        EnemySpawner.OnWaveFinished -= ArchievementsRelated;

    }

    void ArchievementsRelated(int wave)
    {
        ArchievementsNameConstantsData.RichArchievementsAccomplished(GetScoreData().PlayerTotalPoints());
    }
    private void Update()
    {
        if (scoreData.CurrentPlayerPoints() > 0) // Every frame less points
        {
            float newCurrentPlayerPoints = scoreData.CurrentPlayerPoints() - ((scoreData.GetPointSubstraction() * GetCurrentStyle().Multiplier) * Time.deltaTime);
            if (scoreData.CurrentPlayerPoints() <= 0)
            {
                scoreData.SetCurrentPlayerPoints(0);
            }
            else scoreData.SetCurrentPlayerPoints(newCurrentPlayerPoints);
            scoreData.SetCurrentStyleState(GetCurrentStyle());

           

        }
        else if (scoreData.CurrentPlayerPoints() <= 0)
        {
            scoreData.SetCurrentPlayerPoints(0);
        }
        if (activeTimer)
        {
            timeToKill -= Time.deltaTime;
            if (timeToKill <= 0) { activeTimer = false; enemiesKilled = 0; }
        }
        if (hookTimeToKill > 0)
        {
            hookTimeToKill -= Time.deltaTime;
            if (hookTimeToKill <= 0) hookTimeToKill = 0;
        }
        if (dashTimeToKill > 0)
        {
            dashTimeToKill -= Time.deltaTime;
            if (dashTimeToKill <= 0) dashTimeToKill = 0;
        }

        UpdateKillDisplayTimer();
    }
    public void UpdatePoints(int PointsToSum)
    {
        //Update total points
        float newTotalPoints = scoreData.PlayerTotalPoints() + PointsToSum * GetCurrentStyle().Multiplier;        
        scoreData.SetPlayerTotalPoints(newTotalPoints);

        //Update Current Points
        float newCurrentPoints = scoreData.CurrentPlayerPoints() + PointsToSum;
        var currentStyle = GetCurrentStyle();
        scoreData.SetCurrentPlayerPoints(newCurrentPoints);

        //Check if needs to change Style Multiplier enum
        var newCurrentStyle = GetCurrentStyle();       
        scoreData.SetCurrentStyleState(newCurrentStyle);

        if(currentStyle != newCurrentStyle)
        {
            styleCanvas.LetterPopChangeLetter();
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.styleMeterChange, transform.position);
            if(newCurrentStyle == GetScoreData().GetRangos()[GetScoreData().GetRangos().Length - 1]) ArchievementsNameConstantsData.TryhardAccomplisher(); //Maybe has reached SSS for the first time
        }
    }

    public float GetFillAmount()
    {
        var currentPlayerPoints = scoreData.CurrentPlayerPoints();
        var currentStyle = GetCurrentStyle();
        var refinedPlayerPoints = currentPlayerPoints - currentStyle.LevelRange.x;
        var maxRefinedPoints = currentStyle.LevelRange.y - currentStyle.LevelRange.x;
        var clampedFillAmount = refinedPlayerPoints / maxRefinedPoints;        
        return clampedFillAmount;
        // 350
        // 200 - 400
        // 350 - 200 = 150 -- 400 - 200 = 200
        // 150 / 200 = ?%
        // ?% = fill amount bar
    }

    public StyleMeterClass GetCurrentStyle()
    {
        var currentPlayerPoints = scoreData.CurrentPlayerPoints();
        var currentStyle = scoreData.ScoreStyleStates().FirstOrDefault(s => currentPlayerPoints <= s.LevelRange.y && currentPlayerPoints > s.LevelRange.x);
        return currentStyle;
    }

    public void EnableHookKills()
    {
        hookTimeToKill = resetHookTimeToKill;
    }

    public void EnableAirHookKills()
    {
        airkillHook = true;
    }
    void PlayerLanded()
    {
        airkillHook = false;
    }

    public void EnableDashKill()
    {
        dashTimeToKill = resetDashTimeToKill;
    }

    public void KillPoints()
    {
        UpdatePoints(scoreData.KillScorePoints());
        UpdateText("Kill", Color.white);       
        activeTimer = true;
        enemiesKilled = 1;
    }

    public void MultiKillPoints()
    {
        if (enemiesKilled == 2)
        {
            UpdateText("Double Kill", Color.white);
            UpdatePoints(scoreData.DoubleKillScorePoints() + scoreData.KillScorePoints());
            hasAddedPoints = true;           

        }
        else if (enemiesKilled == 3)
        {
            UpdateText("Triple Kill", Color.white);
            UpdatePoints(scoreData.TripleKillScorePoints() + scoreData.KillScorePoints());
            hasAddedPoints = true;           
        }
        else if (enemiesKilled >= 4)
        {
            UpdateText("MultiKill" + "x" + enemiesKilled, Color.white);
            UpdatePoints(scoreData.MultiKillScorePoints() + scoreData.KillScorePoints());
            hasAddedPoints = true;            
        }

    }

    public void AirKillPoints()
    {
        
        if (playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.RIFLE_SHOT)
        {
            UpdateText("Sniped Air", Color.white);
            UpdatePoints(scoreData.SnipedKillPoints());
        }
        else
        {
            UpdateText("Air kill", Color.white);
            UpdatePoints(scoreData.AirKillPoints());            
        }

    }

    public void HeadshotKill()
    {
        UpdateText("HeadShot Kill", Color.white);
        UpdatePoints(scoreData.HeadShotKillPoints());
    }

    public void OverkillKill()
    {
        UpdateText("Overkill", Color.white);        
        UpdatePoints(scoreData.OverkillKillPoints());
    }

    public void HookedEnemyKill()
    {
        UpdateText("Hook Kill", Color.white);        
        UpdatePoints(scoreData.HookedKillPoints());

    }
    public void HookAirKill()
    {
        UpdateText("Air-Hook Kill", Color.white);        
        UpdatePoints(scoreData.AirHookKillPoints());
        if(playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.RIFLE_SHOT)
        {
            UpdateText("Sniped", Color.white);
            UpdatePoints(scoreData.AirHookKillPoints() + scoreData.SniperKillPoints());
        }
    }
    public void DashKill()
    {
        UpdateText("Dash Kill", Color.white);       
        UpdatePoints(scoreData.DashKillPoints());
    }
    public void DuckHuntKill()
    {
        UpdateText("Duck Hunt", Color.white);
        UpdatePoints(scoreData.DuckHuntKillPoints());
    }
    public void DuckSnipeKill()
    {
        UpdateText("Duck Snipe", Color.white);
        UpdatePoints(scoreData.DuckSnipeKillPoints());
    }

    public bool IsFullChargeKill() { return playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.RIFLE_SHOT && playerVariableContainer.WeaponController.WasLastShotFullCharged(); } //if its rifle shot fully charged
    public void FullChargeKill()
    {
        UpdateText("Sniper Kill", Color.white);
        activeTimer = true;
        UpdatePoints(scoreData.SniperKillPoints());
        if (playerVariableContainer.IsInGround())
        {
            Debug.Log("Full charge kill on ground");
        }
        else
        {
            Debug.Log("Full charge kill on air");
        }
    }

    public void EnvironmentalKillPoints()
    {
        UpdateText("Enviroment", Color.white);
        UpdatePoints(scoreData.EnvironmentalKillPoints());
    }    

    void EnemyKill2(IEnemyHealth enemy)
    {
        enemiesKilled++;
        timeToKill = resetTimeToKill;
        Debug.Log(enemy.EnemyAI().EnemyState());
        if (enemy.EnemyAI().EnemyState() == EnemyStates.STOMP)        {
            
            if (playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.SHOTGUN_SHOT)
            {
                DuckHuntKill();
            }
            else if (playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.RIFLE_SHOT)
            {
                DuckSnipeKill();
            }
        }
        
        if (activeTimer && timeToKill > 0)
        {
            MultiKillPoints();
            hasAddedPoints = true; 
        }

        if (enemy.WasKilledByExplosion())
        {

            EnvironmentalKillPoints();
        }

        if (airkillHook)
        {
            HookAirKill();                    
            hasAddedPoints = true; 
            hookKills++;
            ArchievementsNameConstantsData.PirouetteChecker(hookKills);
        }
        if (!playerVariableContainer.IsInGround() && !airkillHook)
        {
            AirKillPoints();
            hasAddedPoints = true;
        }
        
        if (CanDashKill())
        {
            DashKill();
            hasAddedPoints = true;
        }
        
        if (CanHookKill())
        {
            HookedEnemyKill();
            hasAddedPoints = true;

            hookKills++;
            ArchievementsNameConstantsData.PirouetteChecker(hookKills);
        }
        
        if (playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.RIFLE_SHOT)
        {
            FullChargeKill();
            hasAddedPoints = true;
            if (playerVariableContainer.WeaponController.IsLastShotAHeadshot())
            {                                            
                HeadshotKill();                                               
            }
        
        }
        else if (playerVariableContainer.WeaponController.LastShot() == WeaponController.ShotType.SHOTGUN_SHOT)
        {
            shotgunKills++;
            if (Vector3.Distance(enemy.transform.position, playerVariableContainer.transform.position) < overkillDistance)
            {
                OverkillKill();                        
                hasAddedPoints = true;
            }
        }
         
        if (!hasAddedPoints)
        {
            KillPoints();
        }
        hasAddedPoints = false;


        textLetrasAnimation.Play();
        
    }

    public void UpdateText(string killType, Color color)
    {
        textsToDisplay.AddFirst(new DisplayText(killType,color));
        
        if (textsToDisplay.Count > maximunNumberOfActions)
        {
            textsToDisplay.RemoveLast();
        }

    }



    private void OnApplicationQuit()
    {
        ResetPoints();
    }

    public void ResetPoints()
    {
        scoreData.ResetPlayerCurrentPoints();
    }

    public void UpdateKillDisplayTimer()
    {
        //Foreach de tots els elements de la queue
        //Restar timer de tots els elements de la queue
        //Farem dequeue del elements que tinguin el timer en negatiu
        if (textsToDisplay.Count == 0)
            return;

        for (int i = 0; i < textsToDisplay.Count; i++)
        {
            textsToDisplay.ElementAt(i).TextLifetime -= Time.deltaTime;

            if (textsToDisplay.ElementAt(i).TextLifetime <= 0)
            {
                textsToDisplay.RemoveLast();
            }
        }                
    }

    public void LosingPoints()
    {
        if (scoreData.CurrentPlayerPoints() <= GetCurrentStyle().LosingPoints)
        {
            return;
        }
        float newpoints = scoreData.CurrentPlayerPoints() - GetCurrentStyle().LosingPoints;        
        scoreData.SetCurrentPlayerPoints(newpoints);

    }
        
    public class DisplayText
    {
        public string StringToDisplay { get; set; }
        public float TextLifetime { get; set; }

        public Color TextColor { get; set; }
        public DisplayText(string stringToDisplay, Color textcolor)
        {
            StringToDisplay = stringToDisplay;
            TextColor = textcolor;
            TextLifetime = 5.0f;            
        }
    }
}
