using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField] bool DestroyScriptAfterUse = false;
    [SerializeField] bool DestroyGameObjectAfterUse = false;
    [SerializeField] bool HideGameObjectAfterUse = false;

    [SerializeField] string[] TagsUseful;
    public UnityEngine.Events.UnityEvent onTriggerEnter;
    public UnityEngine.Events.UnityEvent onTriggerStay;
    public UnityEngine.Events.UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (TagsUseful == null) return;
        bool OtherObjIsExpected = false;
        foreach (string collidingObjectTag in TagsUseful)
        {
            if (collidingObjectTag == other.tag) OtherObjIsExpected = true;
        }
        if (!OtherObjIsExpected) return;
        onTriggerEnter.Invoke();
        DestroyAfterUse();
    }

    private void OnTriggerStay(Collider other)
    {
        if (TagsUseful == null) return;
        bool OtherObjIsExpected = false;
        foreach (string collidingObjectTag in TagsUseful)
        {
            if (collidingObjectTag == other.tag) OtherObjIsExpected = true;
        }
        if (!OtherObjIsExpected) return;
        onTriggerStay.Invoke();
        DestroyAfterUse();
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagsUseful == null) return;
        bool OtherObjIsExpected = false;
        foreach (string collidingObjectTag in TagsUseful)
        {
            if (collidingObjectTag == other.tag) OtherObjIsExpected = true;
        }
        if (!OtherObjIsExpected) return;
        onTriggerExit.Invoke();
        DestroyAfterUse();
    }

    void DestroyAfterUse()
    {
        if (DestroyScriptAfterUse) Destroy(this);
        if (DestroyGameObjectAfterUse) Destroy(gameObject);
        if(HideGameObjectAfterUse) gameObject.SetActive(false);
    }
}
