using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    public GameObject EpicEffect;
    public GameObject EpicComboEffect;

    public ParticleSystem[] gasParticles;

    public ParticleSystem[] boostParticles;
    public void PlayGasParticles()
    {

        foreach (ParticleSystem gasParticle in gasParticles)
        {
            gasParticle.Play();
            ParticleSystem.EmissionModule em = gasParticle.emission;
            em.rateOverTime = Mathf.Lerp(em.rateOverTime.constant, Mathf.Clamp(150.0f, 30.0f, 100.0f), 0.1f);
        }

    }
    public void StopEpicComboEffect()
    {
        EpicComboEffect.SetActive(false);
    }
    public void PlayEpicComboEffect()
    {
        EpicComboEffect.SetActive(true);
    }
    public IEnumerator PlayEpicEffect()
    {
        EpicEffect.SetActive(true);
        yield return new WaitForSeconds(1);
        EpicEffect.SetActive(false);

    }
    public void StopEpicEffect()
    {
        EpicEffect.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayGasParticles();
    }
}
