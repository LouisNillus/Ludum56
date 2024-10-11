using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance = null;

    public List<AudioClip> _clips = new();
    public AudioSource _musicSource = null;
    public AudioSource _sfxSource = null;

    public bool _sfxEnabled = true;

    private void Start()
    {
        _musicSource.clip = _clips[Random.Range(0, _clips.Count)];
        _musicSource.Play();
    }

    public void PlaySFX(
        AudioClip clip
        )
    {
        if (!_sfxEnabled)
        {
            return;
        }

        _sfxSource.PlayOneShot(clip);
    }


    private void Awake()
    {
        Instance = this;
    }
}
