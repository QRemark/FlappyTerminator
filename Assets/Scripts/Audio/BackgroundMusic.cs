using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private AudioMixerGroup _musicGroup;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
       

        if (_musicGroup != null)
        {
            _audioSource.outputAudioMixerGroup = _musicGroup;
        }

        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

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

    public void PauseMusic()
    {
        _audioSource.Pause();
    }

    public void ContinueMusic()
    {
        _audioSource.UnPause(); 
    }
}
