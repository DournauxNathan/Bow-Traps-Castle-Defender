using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public static ArrowSpawner instance;

    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable _bow;
    [SerializeField] private bool isArrowNotched = false;
    private GameObject currentArrow;

    // Start is called before the first frame update

    void Start()
    {
        instance = this;
        _bow = GetComponentInParent<XRGrabInteractable>();
        PullInteraction.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bow.isSelected && !isArrowNotched)
        {
            StartCoroutine(DelayedSpawn());
        }
        if (!_bow.isSelected)
        {
            Destroy(currentArrow);
        }
    }

    private void NotchEmpty(float value)
    {
        isArrowNotched = false;
    }

    IEnumerator DelayedSpawn()
    {
        isArrowNotched = true;
        yield return new WaitForSeconds(1f);
        currentArrow = Instantiate(arrow, notch.transform);
    }

    public void UpdateArrowPrefab(GameObject newArrow)
    {
        arrow = newArrow;
    }

    public void UpdateArrowPrefab(string arrowName)
    {
        arrow = Resources.Load<GameObject>(arrowName);
    }
}
