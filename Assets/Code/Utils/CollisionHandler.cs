using UnityEngine;
using UnityEditor;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] bool DestroyScriptAfterUse = false;
    [SerializeField] bool DestroyGameObjectAfterUse = false;
    [SerializeField] string[] TagsUseful;
    public UnityEngine.Events.UnityEvent onCollisionEnter;
    public UnityEngine.Events.UnityEvent onCollisionStay;
    public UnityEngine.Events.UnityEvent onCollisionExit;

    private void OnCollisionEnter(Collision other)
    {
        if (TagsUseful == null) return;
        bool OtherObjIsExpected = false;
        foreach (string collidingObjectTag in TagsUseful)
        {
            if (collidingObjectTag == other.collider.tag) OtherObjIsExpected = true;
        }
        if (!OtherObjIsExpected) return;
        onCollisionEnter.Invoke();
        DestroyAfterUse();
    }

    private void OnCollisionStay(Collision other)
    {
        if (TagsUseful == null) return;
        bool OtherObjIsExpected = false;
        foreach (string collidingObjectTag in TagsUseful)
        {
            if (collidingObjectTag == other.collider.tag) OtherObjIsExpected = true;
        }
        if (!OtherObjIsExpected) return;
        onCollisionStay.Invoke();
        DestroyAfterUse();
    }

    private void OnCollisionExit(Collision other)
    {
        if (TagsUseful == null) return;
        bool OtherObjIsExpected = false;
        foreach (string collidingObjectTag in TagsUseful)
        {
            if (collidingObjectTag == other.collider.tag) OtherObjIsExpected = true;
        }
        if (!OtherObjIsExpected) return;
        onCollisionExit.Invoke();
        DestroyAfterUse();
    }
    void DestroyAfterUse()
    {
        if (DestroyScriptAfterUse) Destroy(this);
        if (DestroyGameObjectAfterUse) Destroy(gameObject);
    }
}
