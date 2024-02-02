using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject LastCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Deadzone")
        {
            transform.position = LastCheckpoint.transform.position;
        }
    }
}
