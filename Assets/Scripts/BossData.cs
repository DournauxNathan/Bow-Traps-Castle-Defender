using UnityEngine;

[CreateAssetMenu(fileName = "NewBossData", menuName = "Boss Data")]
public class BossData : ScriptableObject
{
    public string BossName;
    public int health;

    public int maxCritterWaves;
    public int critterToSpawn;

    public GameObject prefab;

    public int phase;
}
