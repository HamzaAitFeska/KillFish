using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private static ParticleController instance;

    public static ParticleController GetInstance()
    {
        if (instance == null)
        {
            instance = new GameObject("ParticleController").AddComponent<ParticleController>();
        }
        return instance;
    }

    public static void DestroySingleton()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance.gameObject);
        }
        instance = null;
    }

    public void SpawnParticle(GameObject particlePrefab, Vector3 position)
    {
        Instantiate(particlePrefab, position, Quaternion.identity);
    }

    public void SpawnParticle(GameObject particlePrefab, Vector3 position, Transform parent)
    {
        Instantiate(particlePrefab, position, Quaternion.identity, parent);
    }

    public void SpawnParticle(GameObject particlePrefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(particlePrefab, position, rotation);
    }

    public void SpawnParticle(GameObject particlePrefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        Instantiate(particlePrefab, position, rotation, parent);
    }
}
