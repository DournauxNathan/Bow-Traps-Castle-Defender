using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetManager : Activator
{
    public List<Target> targets;
    private List<Target> activatedTargets = new List<Target>();

    public void SubscribeTarget(Target target)
    {
        targets.Add(target);

        if (LevelManager.Instance != null && LevelManager.Instance.GetCurrentScene() == 1) 
        {
            onActivate.AddListener(() => LevelManager.Instance.LoadSceneAsync("Tutorial_2"));
        }
    }

    public void IsTargetsActivated(Target _target)
    {
        _target.enabled = false;
        activatedTargets.Add(_target);

        if (activatedTargets.Count == targets.Count)
        {
            onActivate?.Invoke();
            activatedTargets.Clear();

            _target.enabled = true;
            _target.IsActivate = false;
        }
    }
}
