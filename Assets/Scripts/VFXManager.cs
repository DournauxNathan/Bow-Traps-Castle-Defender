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
    public MeshRenderer critterPortalBackground;

    [Header("BOSS VFX")]
    public ParticleSystem rainParticles;
    public ParticleSystem cloudParticles;
    public MeshRenderer bossPortal;
    public MeshRenderer bossPortalBackground; 

    private float floatPropertyValue = 0f;

    private void Start()
    {
        critterPortal.materials[0].SetFloat("_Dissolve", 0);
        critterPortalBackground.materials[0].SetFloat("_Dissolve", 0);
        bossPortal.materials[0].SetFloat("_Dissolve", 0);
        bossPortalBackground.materials[0].SetFloat("_Dissolve", 0);

        ToggleStorm(false);
        TogglePortal(false);
    }

    public void ToggleStorm(bool enable)
    {
        stormSky.SetActive(enable);
        clearSky.SetActive(!enable);

        if (enable)
        {
            StartCoroutine(IncreaseAlphaOverTime(stormSky.GetComponent<MeshRenderer>(), 0.85f));
            rainParticles.Play();
        }
        else
        {
            rainParticles.Stop();
        }
    }

    public void ToggleCritterPortal(bool enable)
    {
        if (enable)
        {
            StartCoroutine(IncreaseFloatOverTime(critterPortal, 0.9f));
            StartCoroutine(IncreaseFloatOverTime(critterPortalBackground, 1f));
        }
        else
        {
            
            StartCoroutine(DecreaseFloatOverTime(critterPortal));
            StartCoroutine(DecreaseFloatOverTime(critterPortalBackground));
        }
    }

    public void TogglePortal(bool enable)
    {
        if (enable)
        {
            bossPortal.gameObject.SetActive(true);
            bossPortalBackground.gameObject.SetActive(true);

            StartCoroutine(IncreaseFloatOverTime(bossPortal, 0.85f));
            StartCoroutine(IncreaseFloatOverTime(bossPortalBackground, 1f));
            cloudParticles.Play();
        }
        else
        {
            bossPortal.gameObject.SetActive(false);
            bossPortalBackground.gameObject.SetActive(false);

            StartCoroutine(DecreaseFloatOverTime(bossPortal));
            StartCoroutine(DecreaseFloatOverTime(bossPortalBackground));
            cloudParticles.Stop();
        }
    }

    private IEnumerator IncreaseAlphaOverTime(MeshRenderer renderer, float maxAlpha)
    {
        float duration = 10f; // Adjust the duration as needed
        float startTime = Time.time;

        while (renderer.materials[0].color.a <= maxAlpha)
        {
            float progress = (Time.time - startTime) / duration;
            Color currentColor = renderer.materials[0].color;

            // Interpolate alpha value
            float newAlpha = Mathf.Lerp(currentColor.a, maxAlpha, progress);

            // Update the color with the new alpha value
            renderer.materials[0].color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

            yield return null;
        }
    }

    private IEnumerator IncreaseFloatOverTime(MeshRenderer renderer, float max)
    {
        float duration = 2.5f; // Adjust the duration as needed
        float startTime = Time.time;

        while (renderer.materials[0].GetFloat("_Dissolve") <= max)
        {
            float progress = (Time.time - startTime) / duration;
            floatPropertyValue = Mathf.Lerp(0f, max, progress);

            // Pass the updated float value to the shader
            renderer.materials[0].SetFloat("_Dissolve", floatPropertyValue);

            yield return null;
        }
    }

    private IEnumerator DecreaseFloatOverTime(MeshRenderer renderer)
    {
        float duration = 5; // Adjust the duration as needed
        float startTime = Time.time;

        while (renderer.materials[0].GetFloat("_Dissolve") >= 0f)
        {
            float progress = (Time.time - startTime) / duration;
            floatPropertyValue = Mathf.Lerp(renderer.materials[0].GetFloat("_Dissolve"), 0f, progress);

            // Pass the updated float value to the shader
            renderer.materials[0].SetFloat("_Dissolve", floatPropertyValue);

            yield return null;
        }
    }
}
