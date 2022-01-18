using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для управления звуковыми эффектами
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public AudioClip audioClip;

    [SerializeField]
    public AudioClip audioClipBang;

    public static AudioManager instance;

    private AudioSource source;

    private bool IsMute;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (instance == null) instance = this;
        // объект не будет уничтожатся при загрузке новой сцены
        DontDestroyOnLoad(gameObject); 

        IsMute = PlayerPrefs.GetInt("Mute") == 1;
        AudioListener.pause = IsMute;
    }

    public void playSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    // Звук клика
    public static AudioClip GetClipClick()
    {
        return instance.audioClip;
    }

    //Звук уничтожения линии
    public static AudioClip GetClipBang()
    {
        return instance.audioClipBang;
    }


    // Заглушка, чтобы в других скриптах НЕ писать AudioManager.insatance.playSound(clip);
    public static void PlaySound(AudioClip clip)
    {
        instance.playSound(clip);
    }

    // Метод для отключения звука в игре
    public void MuteButton()
    {
        IsMute = !IsMute;
        AudioListener.pause = IsMute;
        PlayerPrefs.SetInt("Mute", IsMute ? 1 : 0);
    }
}
