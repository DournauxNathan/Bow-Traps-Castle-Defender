using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossData", menuName = "Boss Data")]
public class BossData : ScriptableObject
{
    public string BossName;
    public int maxHealth;
    public int damagePerHit;
    public float timeBeforeReleaseCast;

    public GameObject prefab;

    public int phase;
    public List<Wave> waves;
}

[System.Serializable]
public class Wave
{
    public int waveId;
    public int critterToSpawn;
}