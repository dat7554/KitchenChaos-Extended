using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private static readonly int Cut = Animator.StringToHash("Cut");
    
    private Animator _animator;
    private CuttingCounter _cuttingCounter;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cuttingCounter = GetComponentInParent<CuttingCounter>();
    }

    private void Start()
    {
        _cuttingCounter.OnCut += ContainerCounter_OnCut;
    }

    private void OnDestroy()
    {
        _cuttingCounter.OnProgressChanged -= ContainerCounter_OnCut;
    }

    private void ContainerCounter_OnCut(object sender, EventArgs e)
    {
        _animator.SetTrigger(Cut);
    }
}
