using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    
    private const string PlayerPrefs_MusicsVolume = "MusicVolume";
    
    private AudioSource _audioSource;
    private float _volume = 0.3f;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of MusicManager in your scene.");
        }
        Instance = this;
        
        _audioSource = GetComponent<AudioSource>();
        
        _volume = PlayerPrefs.GetFloat(PlayerPrefs_MusicsVolume, 0.3f);
        _audioSource.volume = _volume;
    }
    
    public void SetVolume(float value)
    {
        _volume = value;
        PlayerPrefs.SetFloat(PlayerPrefs_MusicsVolume, _volume);
        PlayerPrefs.Save();
        
        _audioSource.volume = _volume;
    }

    public float GetVolume()
    {
        return _volume;
    }
}
