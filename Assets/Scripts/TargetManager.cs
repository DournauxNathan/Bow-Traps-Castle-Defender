using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetManager : Activator
{
    public List<Target> targets;

    public void SubscribeTarget(Target target)
    {
        targets.Add(target);

        if (LevelManager.Instance != null && LevelManager.Instance.GetCurrentScene() == 1) 
        {
            onActivate.AddListener(() => LevelManager.Instance.LoadSceneAsync("Tutorial_2"));
        }
    }

    public void IsTargetsActivated()
    {
        foreach (Target target in targets)
        {
            if (target.IsActivate)
            {
                onActivate?.Invoke();
                target.IsActivate = false;
            }            
        }
    }
}
