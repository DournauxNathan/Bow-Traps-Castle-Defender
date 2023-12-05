using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    public ArrowType arrowWeakness;
    private int health = 0;
    public  bool isDown;
    private Boss bossData;

    internal bool isEnable;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<Boss>() != null)
        {
            bossData = GetComponentInParent<Boss>();
            health = bossData.maxHealth / bossData.weaknesses.Count;
        }
        else
        {
            Debug.LogWarning("No Boss Component found in Parent ! ");
        }        
    }

    void FixedUpdate()
    {
        // Check if the critter is defeated
        if (health <= 0 || isDown)
        {
            isDown = false;
            OnWeaknessDown();
        }
    }

    public void TakeDamage()
    {
        health -= bossData.damagePerHit;
        bossData.CancelCast();
    }

    public void OnWeaknessDown()
    {
        isDown = true;
        bossData.UpdateWeaknessCount();
        bossData.CancelCast();

    }
}
