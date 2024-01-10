using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;
public class VignetteApplier : MonoBehaviour
{
    [Header("Properties")]
    public float intensity = 0.75f;
    public float duration = 0.5f;
    public Volume volume = null;

    // References
    [SerializeField] private LocomotionProvider snapProvider = null;
    [SerializeField] private LocomotionProvider moveProvider = null;
    [SerializeField] private LocomotionProvider snapContinuousProvider = null;
    private Vignette vignette = null;

    private void Awake()
    {
        // GetComponent the provider
        //locomotionProvider = GetComponent<LocomotionProvider>();

        // Get the vignette
        if (volume.profile.TryGet(out Vignette vignette))
        {
            this.vignette = vignette;
        }
    }


    private void OnEnable()
    {
        snapProvider.beginLocomotion += FadeIn;
        snapProvider.endLocomotion += FadeOut;

        moveProvider.beginLocomotion += FadeIn;
        moveProvider.endLocomotion += FadeOut;

        snapContinuousProvider.beginLocomotion += FadeIn;
        snapContinuousProvider.endLocomotion += FadeOut;
    }

    private void OnDisable()
    {
        snapProvider.beginLocomotion -= FadeIn;
        snapProvider.endLocomotion -= FadeOut;

        moveProvider.beginLocomotion -= FadeIn;
        moveProvider.endLocomotion -= FadeOut;

        snapContinuousProvider.beginLocomotion -= FadeIn;
        snapContinuousProvider.endLocomotion -= FadeOut;
    }

    public void FadeIn(LocomotionSystem locomotionSystem)
    {
        // Tween to intensity
        StartCoroutine(Fade(0, intensity));
    }

    public void FadeOut(LocomotionSystem locomotionSystem)
    {
        // Tween to zero
        StartCoroutine(Fade(intensity, 0));
    }

    private IEnumerator Fade(float startValue, float endValue)
    {
        float elapsideTme = 0.0f;

        while (elapsideTme <= duration)
        {
            // Figure out blend value
            float blend = elapsideTme / duration;
            elapsideTme += Time.deltaTime;

            // Appply intensity
            float intensity = Mathf.Lerp(startValue, endValue, blend);
            ApplyValue(intensity);

            if (moveProvider.locomotionPhase == LocomotionPhase.Moving || XRSettingsManager.Instance.isContinuousTurnActive())
            {
                ApplyValue(.60f);
            }
            else
            {
                intensity = Mathf.Lerp(.60f, 0, blend);
                ApplyValue(intensity);
            }

            yield return null;
        }
    }

    private void ApplyValue(float value)
    {
        vignette.intensity.Override(value);
    }
}
