using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    public ArrowType arrowWeakness;
    internal Boss bossData;

    public void OnWeaknessDown()
    {
        bossData.CancelCast();
        bossData.TakeDamage();
        bossData.UpdateWeaknessCount();
    }
}
