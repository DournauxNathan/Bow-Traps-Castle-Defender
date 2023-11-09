// CritterFactory.cs

using UnityEngine;

[CreateAssetMenu(fileName = "NewCritterFactory", menuName = "Critter Factory")]
public class CritterFactory : ScriptableObject
{
    public CritterData critterData;

    public GameObject CreateCritter()
    {
        return Instantiate(critterData.prefab);
    }
}
