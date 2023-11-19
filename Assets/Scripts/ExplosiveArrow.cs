using UnityEngine;

public class ExplosiveArrow : Arrow
{
    public float explosionRadius = 5f;
    public int explosionDamage = 15;

    protected override void DealDamage(Collision collision)
    {
        base.DealDamage(collision);

        // Explosive arrow-specific logic
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hitCollider in colliders)
        {
            if (hitCollider.CompareTag("Critter"))
            {
                if (hitCollider.TryGetComponent<Critter>(out Critter critter))
                {
                    // Apply damage to critters within the explosion radius
                    critter.health -= explosionDamage;
                }
            }
        }

        // Optionally: Visual effects, sound, or any other explosion-related logic
        PlayExplosionEffects();
    }

    private void PlayExplosionEffects()
    {
        m_ParticleSystem.Play();
        m_AudioSource.PlayOneShot(onHitSound);
    }
}
