using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("WEATHER")]
    public GameObject clearSky;
    public GameObject stormSky;

    [Header("CRITTER VFX")]
    public MeshRenderer critterPortal;

    [Header("BOSS VFX")]
    public ParticleSystem rainParticles;
    public ParticleSystem cloudParticles;
    public MeshRenderer bossPortal;
    public MeshRenderer bossPortalBackground; 

    private float floatPropertyValue = 0f;

    private void Start()
    {
        //critterPortal.materials[0].SetFloat("_Dissolve", 0);
        bossPortal.materials[0].SetFloat("_Dissolve", 0);
        bossPortalBackground.materials[0].SetFloat("_Dissolve", 0);

    }

    public void ToggleStorm(bool enable)
    {
        stormSky.SetActive(enable);
        clearSky.SetActive(!enable);

        if (enable)
        {
            rainParticles.Play();
        }
        else
        {
            rainParticles.Stop();
        }
    }

    public void TogglePortal(bool enable)
    {
        if (enable)
        {
            StartCoroutine(IncreaseFloatOverTime(bossPortal, .85f));
            StartCoroutine(IncreaseFloatOverTime(bossPortalBackground, 1f));
            cloudParticles.Play();
        }
        else
        {
            StartCoroutine(DecreaseFloatOverTime(bossPortal));
            StartCoroutine(DecreaseFloatOverTime(bossPortalBackground));
            cloudParticles.Stop();
        }
    }


    private IEnumerator IncreaseFloatOverTime(MeshRenderer renderer, float max)
    {
        float duration = 5f; // Adjust the duration as needed
        float startTime = Time.time;

        while (renderer.materials[0].GetFloat("_Dissolve") <= max)
        {
            float progress = (Time.time - startTime) / duration;
            floatPropertyValue = Mathf.Lerp(0f, 1f, progress);

            // Pass the updated float value to the shader
            renderer.materials[0].SetFloat("_Dissolve", floatPropertyValue);

            yield return null;
        }
    }

    private IEnumerator DecreaseFloatOverTime(MeshRenderer renderer)
    {
        float duration = 5f; // Adjust the duration as needed
        float startTime = Time.time;

        while (renderer.materials[0].GetFloat("_Dissolve") >= 0f)
        {
            float progress = (Time.time - startTime) / duration;
            floatPropertyValue = Mathf.Lerp(floatPropertyValue, 0f, progress);

            // Pass the updated float value to the shader
            renderer.materials[0].SetFloat("_Dissolve", floatPropertyValue);

            yield return null;
        }
    }
}
