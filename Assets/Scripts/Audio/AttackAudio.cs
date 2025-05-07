using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AttackAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioMixerGroup _mixerGroup;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.outputAudioMixerGroup = _mixerGroup;
    }

    public void AttackSound()
    {
        _audioSource.PlayOneShot(_attackSound);
    }
}
