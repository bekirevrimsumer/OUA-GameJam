using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource generalAudioSource;
    public AudioSource backgroundAudioSource;
    public AudioSource footstepsAudioSource;
    public AudioSource UIAudioSource;
    
    [Header("Audio Clips")]
    public AudioClip[] generalAudioClips;
    public AudioClip[] backgroundAudioClips;
    public AudioClip[] grassFootstepsAudioClips;
    public AudioClip[] rockFootstepsAudioClips;
    public AudioClip[] combatAudioClips;
    private int terrainIndex;
    
    private PlayerManager playerManager;
    
    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }
    
    public void SetFootstepsTarget(AudioSource target)
    {
        footstepsAudioSource = target;
    }

    public int GetRandomFootstepsIndex()
    { 
        terrainIndex = playerManager.GetTerrainIndex();
        if (terrainIndex == 0)
            footstepsAudioSource.volume = 0.03f;
        else
            footstepsAudioSource.volume = 0.3f;
        return terrainIndex switch
        {
            0 => Random.Range(0, grassFootstepsAudioClips.Length),
            1 => Random.Range(0, rockFootstepsAudioClips.Length),
            _ => Random.Range(0, grassFootstepsAudioClips.Length)
        };
    }

    public void PlayGeneralSound(int index)
    {
        generalAudioSource.clip = generalAudioClips[index];
        generalAudioSource.Play();
    }

    public void PlayBackgroundSound(int index)
    {
        backgroundAudioSource.clip = backgroundAudioClips[index];
        backgroundAudioSource.Play();
    }

    public void PlayFootstepsSound()
    {
        var index = GetRandomFootstepsIndex();
        footstepsAudioSource.clip = terrainIndex switch
        {
            0 => grassFootstepsAudioClips[index],
            1 => rockFootstepsAudioClips[index],
            _ => grassFootstepsAudioClips[index]
        };
        footstepsAudioSource.PlayOneShot(footstepsAudioSource.clip);
    }

    public void PlayCombatSound(int index)
    {
        footstepsAudioSource.clip = combatAudioClips[index];
        footstepsAudioSource.PlayOneShot(footstepsAudioSource.clip, 1f);
    }

    public void StopBackgroundSound()
    {
        backgroundAudioSource.Stop();
    }


    public void StopFootstepsSound()
    {
        footstepsAudioSource.Stop();
    }

    public void StopAllSounds()
    {
        generalAudioSource.Stop();
        backgroundAudioSource.Stop();
        footstepsAudioSource.Stop();
    }

    public void PauseAllSounds()
    {
        generalAudioSource.Pause();
        backgroundAudioSource.Pause();
        footstepsAudioSource.Pause();
    }

    public void UnPauseAllSounds()
    {
        generalAudioSource.UnPause();
        backgroundAudioSource.UnPause();
        footstepsAudioSource.UnPause();
    }

    public void SetGeneralVolume(float volume)
    {
        generalAudioSource.volume = volume;
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundAudioSource.volume = volume;
    }

    public void SetFootstepsVolume(float volume)
    {
        footstepsAudioSource.volume = volume;
    }

    public void SetAllVolume(float volume)
    {
        generalAudioSource.volume = volume;
        backgroundAudioSource.volume = volume;
        footstepsAudioSource.volume = volume;
    }

    public void SetGeneralMute(bool mute)
    {
        generalAudioSource.mute = mute;
    }

    public void SetBackgroundMute(bool mute)
    {
        backgroundAudioSource.mute = mute;
    }

    public void SetFootstepsMute(bool mute)
    {
        footstepsAudioSource.mute = mute;
    }

    public void SetAllMute(bool mute)
    {
        generalAudioSource.mute = mute;
        backgroundAudioSource.mute = mute;
        footstepsAudioSource.mute = mute;
    }
    
    private int GetRandomAudioClip(AudioClip[] audioClips)
    {
        return Random.Range(0, audioClips.Length);
    }
}
