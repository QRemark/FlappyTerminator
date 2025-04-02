using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private AudioMixerGroup _musicGroup;
    //[SerializeField] private Player _player;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
       // _player = GetComponent<Player>(); 

        if (_musicGroup != null)
        {
            _audioSource.outputAudioMixerGroup = _musicGroup;
        }

        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

    //private void OnEnable()
    //{
    //    if (_player != null)
    //    {
    //        _player.GameOver += StopMusic;
    //    }

    //    PlayMusic();
    //}

    //private void OnDisable()
    //{
    //    if (_player != null)
    //    {
    //        _player.GameOver -= StopMusic;
    //    }
    //}

    public void PlayMusic()
    {
        if (_musicClip != null && !_audioSource.isPlaying)
        {
            _audioSource.clip = _musicClip;
            _audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
