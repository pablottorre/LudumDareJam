using UnityEngine;

public class MusicScripts : MonoBehaviour
{
    [SerializeField] private AudioSource _asNormalMusic;
    [SerializeField] private AudioSource _asEndOfDayMusic;

    void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnStartNewDay, PlayNormalMusic);
        EventManager.SubscribeToEvent(EventNames._OnEndNewDay, PlayEndOfDayMusic);
    }

    private void PlayNormalMusic(params object[] parameters)
    {
        SoundManager.Instance.PauseMusic();
        SoundManager.Instance.PlayMusic(_asNormalMusic.GetHashCode());
    }
    
    private void PlayEndOfDayMusic(params object[] parameters)
    {
        SoundManager.Instance.PauseMusic();
        SoundManager.Instance.PlayMusic(_asEndOfDayMusic.GetHashCode());
    }

}
