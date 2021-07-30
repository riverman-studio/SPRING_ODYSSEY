using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timelineTrigger : MonoBehaviour
{
    public Animator childAnimator;
    public string triggerName;
    public bool triggerValue;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerValue)
            childAnimator.SetTrigger(triggerName);
    }
}
