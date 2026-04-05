using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private static readonly int OpenClose = Animator.StringToHash("OpenClose");
    
    private Animator _animator;
    private ContainerCounter _containerCounter;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _containerCounter = GetComponentInParent<ContainerCounter>();
    }

    private void Start()
    {
        _containerCounter.OnPlayGrabbedObject += ContainerCounter_OnPlayGrabbedObject;
    }

    private void OnDisable()
    {
        _containerCounter.OnPlayGrabbedObject -= ContainerCounter_OnPlayGrabbedObject;
    }

    private void ContainerCounter_OnPlayGrabbedObject(object sender, EventArgs e)
    {
        _animator.SetTrigger(OpenClose);
    }
}
