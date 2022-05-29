using System.Collections.Generic;
using UnityEngine;
using Vanta.Core;
using Vanta.Levels;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip[] menuSounds;
    [SerializeField] private AudioSource audioSourceMusic;
    private bool _levelStarted;

    private float _fXSound = 0;
    public float FXSound => _fXSound;

    private void Start()
    {
        LevelManager.Instance.levelDidLoad += LevelDidLoad;
        LevelManager.Instance.levelDidStart += LevelDidStart;
        LevelManager.Instance.levelDidSuccees += LevelDidSuccess;
        LevelManager.Instance.levelDidFail += LevelDidFail;
    }

    private void PlayNextSong(IReadOnlyCollection<AudioClip> clipList)
    {
        if (Application.isPlaying) audioSourceMusic.Stop();
        var rnd = Random.Range(0, clipList.Count);
        audioSourceMusic.clip = menuSounds[rnd];
        audioSourceMusic.Play();
    }

    private void Update()
    {
        if (!audioSourceMusic.isPlaying && _levelStarted)
        {
            PlayNextSong(menuSounds);
        }
    }

    public void ChangeVolume(float v)
    {
        audioSourceMusic.volume = v;
        MuteSound(v <= 0);
    }

    public void ChangeEffectVolume(float v)
    {
        _fXSound = v;
    }

    private void MuteSound(bool mute)
    {
        audioSourceMusic.mute = mute;
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
