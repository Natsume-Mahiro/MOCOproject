using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] private List<AudioClip> bgmList;
    [SerializeField] private List<AudioClip> seList;

    private int currentBGMIndex = -1; // 今再生中のBGMのインデックス

    private float crossfadeDuration = 0.5f; // クロスフェードの時間

    public bool BGMplaying;
    public bool SEplaying;

    override protected void Awake()
    {
        base.Awake();

        // シーン遷移で破棄されないようにする
        DontDestroyOnLoad(this);

        // Prefab から生成するときに (Clone) を外す
        this.gameObject.name = "SoundManager";

        OnBGM();
        OnSE();
    }

    public void PlayBGM(int bgmIndex)
    {
        if (bgmIndex >= 0 && bgmIndex < bgmList.Count && currentBGMIndex != bgmIndex)
        {
            float Duration = currentBGMIndex == -1 ? 0.0f : crossfadeDuration;
            // DOTweenを使用してクロスフェード
            float startVolume = bgmAudioSource.volume;
            bgmAudioSource.DOFade(0, Duration).OnComplete(() =>
            {
                bgmAudioSource.clip = bgmList[bgmIndex];
                bgmAudioSource.Play();
                bgmAudioSource.DOFade(startVolume, Duration);
                currentBGMIndex = bgmIndex; // 再生中のBGMを更新
            });
        }
    }

    public void PlaySE(int seIndex)
    {
        if (seIndex >= 0 && seIndex < seList.Count)
        {
            seAudioSource.PlayOneShot(seList[seIndex]);
        }
        else
        {
            Debug.LogError("Invalid SE index: " + seIndex);
        }
    }

    public void OnBGM() { bgmAudioSource.volume = 0.5f; BGMplaying = true; }
    public void OffBGM() { bgmAudioSource.volume = 0.0f; BGMplaying = false; }
    public void OnSE() { seAudioSource.volume = 1.0f; SEplaying = true; }
    public void OffSE() { seAudioSource.volume = 0.0f; SEplaying = false; }
}
