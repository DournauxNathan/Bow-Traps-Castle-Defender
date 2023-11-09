// CritterFactory.cs

using UnityEngine;

[CreateAssetMenu(fileName = "NewCritterFactory", menuName = "Critter Factory")]
public class CritterFactory : ScriptableObject
{
    public CritterData critterData;

    public GameObject CreateCritter(Transform parent)
    {
        return Instantiate(critterData.prefab, parent.position, Quaternion.identity);
    }
}
