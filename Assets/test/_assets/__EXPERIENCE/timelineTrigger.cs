using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;

public class timelineTrigger : MonoBehaviour
{
    /*
    [Serializable]
    public class TriggerEvent : UnityEvent<BaseEventData>
    { }
    [Serializable]
    public class Entry
    {
        /// <summary>
        /// The desired TriggerEvent to be Invoked.
        /// </summary>
        public TriggerEvent callback = new TriggerEvent();
    }
    [SerializeField]
    private List<Entry> m_Delegates;

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [Obsolete("Please use triggers instead (UnityUpgradable) -> triggers", true)]
    public List<Entry> delegates { get { return triggers; } set { triggers = value; } }


    public List<Entry> triggers
    {
        get
        {
            if (m_Delegates == null)
                m_Delegates = new List<Entry>();
            return m_Delegates;
        }
        set { m_Delegates = value; }
    }
    */

    public Animator childAnimator;
    public string triggerName;
    public bool triggerValue;

    bool lastTriggerValue = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lastTriggerValue != triggerValue)
            childAnimator.SetTrigger(triggerName);
        lastTriggerValue = triggerValue;
    }
}
