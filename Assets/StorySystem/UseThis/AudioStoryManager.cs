using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioStoryManager : Singleton<AudioStoryManager>
{
    [SerializeField] private AudioClip menuBGM;
    private AudioSource audioSource;
    [HideInInspector] public AudioClip nowBGM;
    private bool needToChangeNewOne;

    private AudioSource buttonAudioSource;
    [SerializeField] private AudioClip buttonAudioClip;

    private void Start()
    {
        audioSource = this.AddComponent<AudioSource>();
        audioSource.loop = true;
        if (menuBGM != null)
        {
            StartMainBgm();
        }
        buttonAudioSource = this.AddComponent<AudioSource>();
        if (buttonAudioClip != null) buttonAudioSource.clip = buttonAudioClip;
    }

    public void StartMainBgm() => StartCoroutine(DelayPlay(.5f));

    public void PlayMusic()
    {
        if (audioSource.clip == null && nowBGM != null)
        {
            audioSource.clip = nowBGM;
        }
        if (needToChangeNewOne)
        {
            audioSource.Play();
            needToChangeNewOne = false;
            Debug.Log($"now Bgm is {nowBGM.name}, playing scucess");
        }
    }

    public void UpdateBGM(AudioClip audioClip)
    {
        nowBGM = audioClip;
        audioSource.clip = nowBGM;
        needToChangeNewOne = true;
    }

    IEnumerator DelayPlay(float time)
    {
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        InitMenuBGM();
        Debug.Log("Play Success");
    }

    public void PlayFirstBGM()
    {
        audioSource.clip = nowBGM;
        needToChangeNewOne = false;
        audioSource.Play();
    }

    private void InitMenuBGM()
    {
        audioSource.clip = menuBGM;
        audioSource.Play();
    }

    public void PlayButtonClickAudio()
    {
        buttonAudioSource.Play();
    }


}
