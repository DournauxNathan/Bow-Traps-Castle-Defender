// CritterData.cs

using UnityEngine;

[CreateAssetMenu(fileName = "NewCritterData", menuName = "Critter Data")]
public class CritterData : ScriptableObject
{
    public string critterName;
    public int health;
    public float speed;
    public GameObject prefab;
}
