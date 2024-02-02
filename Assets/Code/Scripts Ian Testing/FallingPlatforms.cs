using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    [SerializeField]
    private GameObject thePlatform;
    [SerializeField]
    private GameObject theOtherPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            thePlatform.transform.Translate(0.0f, -12.0f, 0.0f);
            theOtherPlatform.transform.Translate(0.0f, -12.0f, 0.0f);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
