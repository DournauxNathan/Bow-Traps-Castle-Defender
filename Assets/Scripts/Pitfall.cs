using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : BaseTrap
{
    private Animator m_Animator;
    public GameObject pathModifier;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        base.Activate();
    }

    protected override IEnumerator ActivateTrapEffect()
    {
        // Placeholder logic for common trap effect
        yield return base.ActivateTrapEffect();

        m_Animator.SetTrigger("Open");
        pathModifier.SetActive(true);
    }

    protected override IEnumerator DeactivateTrapEffectAfterDelay(float delay)
    {
        // Placeholder logic for common trap effect
        yield return base.DeactivateTrapEffectAfterDelay(trapEffectDuration);
        
        pathModifier.SetActive(false);
        m_Animator.SetTrigger("Close");
    }
}