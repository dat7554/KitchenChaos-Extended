using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    private const string PlayerPrefs_SoundEffectsVolume = "SoundEffectsVolume";
    
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    
    private float _volume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of SoundManager in your scene.");
        }
        Instance = this;
        
        _volume = PlayerPrefs.GetFloat(PlayerPrefs_SoundEffectsVolume, 1f);
    }
    
    private void Start()
    {
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManagerOnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailed += DeliveryManagerOnOrderFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickupSomething += Player_OnPickupSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnOrderSuccess -= DeliveryManagerOnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailed -= DeliveryManagerOnOrderFailed;
        CuttingCounter.OnAnyCut -= CuttingCounter_OnAnyCut;
        Player.Instance.OnPickupSomething -= Player_OnPickupSomething;
        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    private void DeliveryManagerOnOrderSuccess(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }
    
    private void DeliveryManagerOnOrderFailed(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }
    
    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }
    
    private void Player_OnPickupSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }
    
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }
    
    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }
    
    public void PlayFootstepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
    
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    
    public void PlayWarninigSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }
    
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * _volume);
    }

    public void ChangeVolume()
    {
        _volume += 0.1f;
        if (_volume > 1f)
        {
            _volume = 0f;
        }

        PlayerPrefs.SetFloat(PlayerPrefs_SoundEffectsVolume, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _volume;
    }
}
