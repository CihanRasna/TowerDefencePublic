using System.Collections.Generic;
using UnityEngine;
using Vanta.Core;
using Vanta.Levels;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip[] menuSounds;
    [SerializeField] private AudioSource audioSource;
    private bool _levelStarted;

    private void Start()
    {
        LevelManager.Instance.levelDidLoad += LevelDidLoad;
        LevelManager.Instance.levelDidStart += LevelDidStart;
        LevelManager.Instance.levelDidSuccees += LevelDidSuccess;
        LevelManager.Instance.levelDidFail += LevelDidFail;
    }

    private void PlayNextSong(IReadOnlyCollection<AudioClip> clipList)
    {
        if (Application.isPlaying) audioSource.Stop();
        var rnd = Random.Range(0, clipList.Count);
        audioSource.clip = menuSounds[rnd];
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && _levelStarted)
        {
            PlayNextSong(menuSounds);
        }
    }

    public void ChangeVolume(float v)
    {
        audioSource.volume = v;
        MuteSound(v <= 0);
    }

    public void ChangeEffectVolume(float v)
    {
        Debug.LogError("NO SOUND EFFECT YET");
    }

    private void MuteSound(bool mute)
    {
        audioSource.mute = mute;
    }
    

    private void LevelDidLoad(BaseLevel baseLevel)
    {
        PlayNextSong(menuSounds);
    }

    private void LevelDidStart(BaseLevel baseLevel)
    {
        _levelStarted = true;
    }

    private void LevelDidSuccess(BaseLevel baseLevel)
    {
       
    }

    private void LevelDidFail(BaseLevel baseLevel)
    {
        
    }
}
