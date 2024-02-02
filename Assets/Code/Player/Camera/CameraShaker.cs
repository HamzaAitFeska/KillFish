using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;
    static Dictionary<string, CameraShaker> instanceList = new Dictionary<string, CameraShaker>();

    public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);
    public Vector3 DefaultRotInfluence = new Vector3(1, 1, 1);
    public Vector3 RestPositionOffset = new Vector3(0, 0, 0);
    public Vector3 RestRotationOffset = new Vector3(0, 0, 0);

    Vector3 posAddShake, rotAddShake;

    List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();

    void Awake()
    {
        Instance = this;
        instanceList.Add(gameObject.name, this);
    }

    void Update()
    {
        posAddShake = Vector3.zero;
        rotAddShake = Vector3.zero;

        if (!GameController.GetGameController().GetSettingsData().ScreenShake) return;

        for (int i = 0; i < cameraShakeInstances.Count; i++)
        {
            if (i >= cameraShakeInstances.Count)
                break;

            CameraShakeInstance c = cameraShakeInstances[i];

            if (c.CurrentState == CameraShakeState.Inactive && c.DeleteOnInactive)
            {
                cameraShakeInstances.RemoveAt(i);
                i--;
            }
            else if (c.CurrentState != CameraShakeState.Inactive)
            {
                posAddShake += CameraUtilities.MultiplyVectors(c.UpdateShake(), c.PositionInfluence);
                rotAddShake += CameraUtilities.MultiplyVectors(c.UpdateShake(), c.RotationInfluence);
            }
        }

        transform.localPosition = posAddShake + RestPositionOffset;
        transform.localEulerAngles = rotAddShake + RestRotationOffset; 
    }
        
    public static CameraShaker GetInstance(string name)
    {
        CameraShaker c;

        if (instanceList.TryGetValue(name, out c))
            return c;

        Debug.LogError("CameraShake " + name + " not found!");

        return null;
    }
    public CameraShakeInstance Shake(CameraShakeInstance shake)
    {
        cameraShakeInstances.Add(shake);
        return shake;
    }
    public CameraShakeInstance Shake(CameraShakeInstanceVariables shake)
    {
        CameraShakeInstance newInstance = new CameraShakeInstance(shake.magnitude, shake.roughness, shake.fadeInTime, shake.fadeOutTime);
        newInstance.PositionInfluence = DefaultPosInfluence;
        newInstance.RotationInfluence = DefaultRotInfluence;
        cameraShakeInstances.Add(newInstance);
        return newInstance;
    }
    public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
        shake.PositionInfluence = DefaultPosInfluence;
        shake.RotationInfluence = DefaultRotInfluence;
        cameraShakeInstances.Add(shake);

        return shake;
    }
    public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence)
    {
        CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
        shake.PositionInfluence = posInfluence;
        shake.RotationInfluence = rotInfluence;
        cameraShakeInstances.Add(shake);

        return shake;
    }
    public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
    {
        CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness);
        shake.PositionInfluence = DefaultPosInfluence;
        shake.RotationInfluence = DefaultRotInfluence;
        shake.StartFadeIn(fadeInTime);
        cameraShakeInstances.Add(shake);
        return shake;
    }
    public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence)
    {
        CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness);
        shake.PositionInfluence = posInfluence;
        shake.RotationInfluence = rotInfluence;
        shake.StartFadeIn(fadeInTime);
        cameraShakeInstances.Add(shake);
        return shake;
    }

    public List<CameraShakeInstance> ShakeInstances
    { get { return new List<CameraShakeInstance>(cameraShakeInstances); } }

    void OnDestroy()
    {
        instanceList.Remove(gameObject.name);
    }

    public CameraShakeInstance Bump
    {
        get
        {
            CameraShakeInstance c = new CameraShakeInstance(2.5f, 4, 0.1f, 0.75f);
            c.PositionInfluence = Vector3.one * 0.15f;
            c.RotationInfluence = Vector3.one;
            return c;
        }
    }
    public CameraShakeInstance Explosion
    {
        get
        {
            CameraShakeInstance c = new CameraShakeInstance(5f, 10, 0, 1.5f);
            c.PositionInfluence = Vector3.one * 0.25f;
            c.RotationInfluence = new Vector3(4, 1, 1);
            return c;
        }
    }
    public CameraShakeInstance Vibration
    {
        get
        {
            CameraShakeInstance c = new CameraShakeInstance(0.4f, 20f, 2f, 2f);
            c.PositionInfluence = new Vector3(0, 0.15f, 0);
            c.RotationInfluence = new Vector3(1.25f, 0, 4);
            return c;
        }
    }
    public CameraShakeInstance RoughDriving
    {
        get
        {
            CameraShakeInstance c = new CameraShakeInstance(1, 2f, 1f, 1f);
            c.PositionInfluence = Vector3.zero;
            c.RotationInfluence = Vector3.one;
            return c;
        }
    }

    //You can add more types



}
