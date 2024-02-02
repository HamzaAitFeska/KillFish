using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUtils : MonoBehaviour
{
    public static T GetRandomVar<T>(T[] array)
    {
        Shuffle(array);
        return array[0];
    }
    public static T GetRandomVar<T>(List<T> array)
    {
        Shuffle(array);
        return array[0];
    }
    public static T Shuffle<T>(T[] Array)
    {
        T tempGO;
        for (int i = 0; i < Array.Length; i++)
        {
            int rnd = Random.Range(0, Array.Length);
            tempGO = Array[rnd];
            Array[rnd] = Array[i];
            Array[i] = tempGO;
        }
        return (T)Array[0];
    }
    public static T Shuffle<T>(List<T> Array)
    {
        T tempGO;
        for (int i = 0; i < Array.Count; i++)
        {
            int rnd = Random.Range(0, Array.Count);
            tempGO = Array[rnd];
            Array[rnd] = Array[i];
            Array[i] = tempGO;
        }
        return (T)Array[0];
    }
}
