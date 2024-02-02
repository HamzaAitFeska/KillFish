using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontExplodeBarrelsUICanvas : MonoBehaviour
{
    bool hasShownUp = false;
    [SerializeField] Animation myAnimation;    
    
    public void ShowUp()
    {
        if (hasShownUp) { return; }

        myAnimation.Play();
        hasShownUp = true;
    }
}
