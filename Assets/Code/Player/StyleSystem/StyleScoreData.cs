using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StyleMeterClass
{
    public Vector2 LevelRange = Vector2.zero;
    public float Multiplier = 1;
    public float Health_Damage = 1;
    public float LosingPoints;
    public string Texto = "";
    public GameObject Image;    
}


[CreateAssetMenu(fileName = "Style Score Data", menuName = "Data/StyleScore"), System.Serializable]
public class StyleScoreData : ScriptableObject
{
    //The Style States as enum strings
    StyleMeterClass currentStyleState;
    public StyleMeterClass GetCurrentStyleState() { return currentStyleState; }

    public void SetCurrentStyleState(StyleMeterClass style) { currentStyleState = style; }

    //The punctuation to change between states

    [SerializeField] StyleMeterClass[] rangos;  
    public StyleMeterClass[] GetRangos() { return rangos; }

    public StyleMeterClass[] ScoreStyleStates() { return rangos; }

    //TOTAL Points the player has in this game
    [SerializeField] float playerTotalPoints;
    public float PlayerTotalPoints() { return playerTotalPoints; }
    public void SetPlayerTotalPoints(float newTotalPoints) { playerTotalPoints = newTotalPoints; }

    public void SetMinusPlayerTotalPoints(float minusTotal) { playerTotalPoints -= minusTotal; }

    //CURRENT Points the player has
    float currentPlayerPoints;
    public float CurrentPlayerPoints() { return currentPlayerPoints; }
    public void SetCurrentPlayerPoints(float newCurrentPlayerPoints) { currentPlayerPoints = newCurrentPlayerPoints;
        currentPlayerPoints = Mathf.Clamp(currentPlayerPoints, 0, rangos[rangos.Length-1].LevelRange.y);
    }
    public void ResetPlayerCurrentPoints() { currentPlayerPoints = 0; }
        
    //The amount of points that are substracted per frame
    [SerializeField] int pointSubstraction;
    public int GetPointSubstraction() { return pointSubstraction; }

    public int KillScorePoints() { return KillScore; }

    public int DoubleKillScorePoints() { return DoubleKillScore; }

    public int TripleKillScorePoints() { return TripleKillScore; }

    public int MultiKillScorePoints() { return MultiKillScore; }

    public int AirKillPoints() { return AirKill; }

    public int HeadShotKillPoints() { return HeadShot; }

    public int OverkillKillPoints() { return Overkill; }

    public int HookedKillPoints() { return Hooked; }

    public int DashKillPoints() { return DashKill; }

    public int AirHookKillPoints() { return AirHookKill; }

    public int SniperKillPoints() { return SniperKill; }

    public int EnvironmentalKillPoints() { return EnvironmentalKill; }

    public int SnipedKillPoints() { return SnipedKill; }

    public int DuckHuntKillPoints() { return DuckHuntKill; }

    public int DuckSnipeKillPoints() { return DuckSnipeKill; }




    //How much does every action give?
    [SerializeField] int KillScore;
    [SerializeField] int DoubleKillScore;
    [SerializeField] int TripleKillScore;
    [SerializeField] int MultiKillScore;
    [SerializeField] int AirKill;
    [SerializeField] int HeadShot;
    [SerializeField] int Overkill;
    [SerializeField] int Hooked;
    [SerializeField] int DashKill;
    [SerializeField] int AirHookKill;
    [SerializeField] int SniperKill;
    [SerializeField] int EnvironmentalKill;
    [SerializeField] int SnipedKill;
    [SerializeField] int DuckHuntKill;
    [SerializeField] int DuckSnipeKill;




}

