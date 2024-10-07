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
        //TODO: Here add a function to stop other music from playing, as well as this one in loop
        SoundManager.Instance.PlayMusic(_asNormalMusic.GetHashCode());
    }
    
    private void PlayEndOfDayMusic(params object[] parameters)
    {
        //TODO: Here add a function to stop other music from playing, as well as this one in loop
        SoundManager.Instance.PlayMusic(_asEndOfDayMusic.GetHashCode());
    }

}
