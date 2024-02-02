using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHookPoint : MonoBehaviour
{
    [SerializeField] bool applyJumpForce;
    public bool ApplyJumpForce() { return applyJumpForce; }
}
