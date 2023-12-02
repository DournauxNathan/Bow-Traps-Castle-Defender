// CritterFactory.cs

using UnityEngine;

[CreateAssetMenu(fileName = "NewCritterFactory", menuName = "Critter Factory")]
public class CritterFactory : ScriptableObject
{
    public CritterData critterData;
    public BossData bossData;

    public GameObject CreateCritter(Transform parent)
    {
        return Instantiate(critterData.prefab, parent.position, Quaternion.identity);
    }

    public GameObject SpawnBoss(Transform parent)
    {
        return Instantiate(bossData.prefab, parent.position, Quaternion.identity);
    }
}
