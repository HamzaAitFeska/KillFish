using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level Style Punctuations", menuName = "Data/LevelStyle"), System.Serializable]
public class LevelStyleData : ScriptableObject
{
    [Tooltip("Lenght of this vector should be 7--> Going from 0 to 6 equalising the 7 letters on style")]
     public Vector2[] timeLevelRanges; // For the time variable
    
    [Tooltip("Lenght of this vector should be 7--> Going from 0 to 6 equalising the 7 letters on style")]
    public Vector2[] pointsLevelRanges; //For the style variable
}

