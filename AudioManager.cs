using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public AudioClip audioClip;

    [SerializeField]
    public AudioClip audioClipBang;

    public static AudioManager instance;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (instance == null) instance = this;

        DontDestroyOnLoad(gameObject); // объект не будет уничтожатся при загрузке новой сцены
    }


    public void playSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public static AudioClip GetClipClick()
    {
        return instance.audioClip;
    }

    public static AudioClip GetClipBang()
    {
        return instance.audioClipBang;
    }


    // это просто заглушка, чтобы в других скриптах НЕ писать AudioManager.insatance.playSound(clip);

    public static void PlaySound(AudioClip clip)
    {
        instance.playSound(clip);
    }
}
