using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private readonly Dictionary<int, AudioSource> _sfxSources = new();
    private readonly Dictionary<int, AudioSource> _musicSources = new();

    private float _sfxVolume;
    private bool _sfxMute;

    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            foreach (var audioSource in _sfxSources.Values)
            {
                audioSource.volume = _sfxVolume;
            }
            PlayerPrefs.SetFloat("sfxVolume", _sfxVolume);
        }
    }

    public bool SFXMute
    {
        get => _sfxMute;
        set
        {
            _sfxMute = value;
            foreach (var audioSource in _sfxSources.Values)
            {
                audioSource.mute = _sfxMute;
            }
            PlayerPrefs.SetInt("sfxMute", _sfxMute ? 1 : 0);
        }
    }

    private float _musicVolume;
    private bool _musicMute;
    
    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            foreach (var audioSource in _musicSources.Values)
            {
                audioSource.volume = _musicVolume;
            }
            PlayerPrefs.SetFloat("musicVolume", _musicVolume);
        }
    }

    public bool MusicMute
    {
        get => _musicMute;
        set
        {
            _musicMute = value;
            foreach (var audioSource in _musicSources.Values)
            {
                audioSource.mute = _musicMute;
            }
            PlayerPrefs.SetInt("musicMute", _musicMute ? 1 : 0);
        }
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        var audioSources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip.name.StartsWith("M"))
            {
                _musicSources.Add(audioSource.GetHashCode(), audioSource);
            }
            else
            {
                _sfxSources.Add(audioSource.GetHashCode(), audioSource);
            }
        }


        _sfxVolume = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 1;
        _sfxMute = PlayerPrefs.HasKey("sfxMute") && (PlayerPrefs.GetInt("sfxMute") == 1 ? true : false );  
        
        _musicVolume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 1;
        _musicMute = PlayerPrefs.HasKey("musicMute") && (PlayerPrefs.GetInt("musicMute") == 1 ? true : false );
    }

    #region SFX

    public void AddSFXSource(AudioSource audioSource)
    {
        if (_sfxSources.ContainsKey(audioSource.GetHashCode())) return;
        
        _sfxSources.Add(audioSource.GetHashCode(), audioSource);
    }

    public void RemoveSFXSource(int hash)
    {
        if (!_sfxSources.ContainsKey(hash)) return;
        
        _sfxSources.Remove(hash);
    }

    public void PlaySFX(int hash)
    {
        if (!_sfxSources.ContainsKey(hash)) return;
        
        _sfxSources[hash].Play();
    }

    #endregion

    #region Music

    public void AddMusicSource(AudioSource audioSource)
    {
        if (_musicSources.ContainsKey(audioSource.GetHashCode())) return;
        
        _musicSources.Add(audioSource.GetHashCode(), audioSource);
    }

    public void RemoveMusicSource(int hash)
    {
        if (!_musicSources.ContainsKey(hash)) return;
        
        _musicSources.Remove(hash);
    }

    public void PlayMusic(int hash)
    {
        if (!_musicSources.ContainsKey(hash)) return;
        
        _musicSources[hash].Play();
    }

    #endregion
}