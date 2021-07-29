using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirverVent : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _animator = null;
    public float _driveValue = 0.0f;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Talk", _driveValue);
    }
}
