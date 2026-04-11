using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private float _footstepTimer;
    private float _footstepTimerMax = 0.1f;

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer < 0f)
        {
            _footstepTimer = _footstepTimerMax;

            if (Player.Instance.IsWalking)
            {
                SoundManager.Instance.PlayFootstepsSound(Player.Instance.transform.position);
            }
        }
    }
}
