using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashOrb : MonoBehaviour
{
    [SerializeField] AnimDoor targetDoor;
    [SerializeField] GameObject orbRender;
    [SerializeField] GameObject dashOrbParticles;
    private void OnEnable()
    {
        EnemySpawner.OnWaveFinished += SpawnOrb;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWaveFinished -= SpawnOrb;
    }

    public void ActiveAnimDoor()
    {
        targetDoor.OpenDoor();
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIHover, transform.position);
        Instantiate(dashOrbParticles, transform.position, Quaternion.identity);

    }

    void SpawnOrb(int wave)
    {
        GetComponent<SphereCollider>().enabled = true;
        orbRender.SetActive(true);
    }


}
