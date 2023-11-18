using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : BreakableTrap
{
    public List<Boulder> boulders;

    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Subscribe(Boulder boulder)
    {
        boulders.Add(boulder);
    }


    public override void Activate()
    {
        base.Activate();
    }

    protected override IEnumerator ActivateTrapEffect()
    {
        // Placeholder logic for common trap effect
        yield return base.ActivateTrapEffect();

        m_Animator.SetTrigger("Down");

        boulders[0].Activate();
    }

    protected override IEnumerator DeactivateTrapEffectAfterDelay(float delay)
    {
        // Placeholder logic for common trap effect
        yield return base.DeactivateTrapEffectAfterDelay(trapEffectDuration);

        m_Animator.SetTrigger("Up");
    }
}
