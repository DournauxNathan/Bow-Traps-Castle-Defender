using UnityEngine;

public class QuiverController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform quiverTransform;
    public int maxArrowCount = 10;

    private int currentArrowCount;

    void Start()
    {
        currentArrowCount = maxArrowCount;
    }

    void ShootArrow()
    {
        if (currentArrowCount > 0)
        {
           // GameObject arrow = Instantiate(arrowPrefab, quiverTransform.position, quiverTransform.rotation);
            currentArrowCount--;
        }
        else
        {
            // Handle out of arrows scenario
        }
    }
}
