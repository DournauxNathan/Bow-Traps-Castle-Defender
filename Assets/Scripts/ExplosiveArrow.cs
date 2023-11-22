using UnityEngine;

public class ExplosiveArrow : Arrow
{
    public AudioClip onExplosionSound;
    [Header("EXPLOSION PARAMS")]
    public float explosionRadius = 5f;
    public int explosionDamage = 15;


    protected override void DealDamage(Collision collision)
    {
        base.DealDamage(collision);

        // Explosive arrow-specific logic
        Collider[] colliders = Physics.OverlapSphere(tip.position, explosionRadius, layerMask);

        foreach (Collider hitCollider in colliders)
        {
            if (hitCollider.CompareTag("Critter"))
            {
                if (hitCollider.TryGetComponent<Critter>(out Critter critter))
                {
                    // Apply damage to critters within the explosion radius
                    critter.TakeDamage(explosionDamage);
                }
            }
        }

        // Optionally: Visual effects, sound, or any other explosion-related logic
        PlayExplosionEffects();
    }

    private void PlayExplosionEffects()
    {
        m_ParticleSystem.Play();
        m_AudioSource.PlayOneShot(onExplosionSound);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.35f);
        Gizmos.DrawSphere(tip.position, explosionRadius);
    }
}
