using UnityEngine;

public class StarIconSingleUI : MonoBehaviour
{
    private static readonly int Fill = Animator.StringToHash("Fill");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayFillAnimation()
    {
        _animator.SetTrigger(Fill);
    }
}
