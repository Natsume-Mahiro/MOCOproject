using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerDataPrefab;
    [SerializeField] GameObject StageObj;
    [SerializeField] GameObject IkuseiObj;
    [SerializeField] GameObject ZukanObj;

    [SerializeField] SpriteRenderer BlackOut;

    private enum Mode
    {
        Stage,
        Ikusei,
        Zukan
    }

    private Mode mode;

    private void Awake()
    {
        // 万が一 PlayerData がないときのため
        Instantiate(PlayerDataPrefab);
        
        SetInitialScreen(Mode.Stage);

        Time.timeScale = 1.0f;

        BlackOut.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        if (SoundManager.Instance != null) SoundManager.Instance.PlayBGM(0);
    }

    private void SetInitialScreen(Mode initialMode)
    {
        ShowScreen(initialMode);
        mode = initialMode;
    }

    private void ShowScreen(Mode screenMode)
    {
        StageObj.SetActive(screenMode == Mode.Stage);
        IkuseiObj.SetActive(screenMode == Mode.Ikusei);
        ZukanObj.SetActive(screenMode == Mode.Zukan);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }
        
        // 画面ごとの処理をここに追加
        switch (mode)
        {
            case Mode.Stage:
                // ステージ選択の処理
                break;

            case Mode.Ikusei:
                // 育成画面の処理
                break;

            case Mode.Zukan:
                // 図鑑画面の処理
                break;
        }
    }

    public void OnStage()
    {
        BlackOut.DOFade(1.0f, 0.1f).OnComplete(() =>
        { 
            ShowScreen(Mode.Stage);
            BlackOut.DOFade(0.0f, 0.1f).SetDelay(0.1f);
        });
    }

    public void OnIkusei()
    {
        BlackOut.DOFade(1.0f, 0.1f).OnComplete(() =>
        { 
            ShowScreen(Mode.Ikusei);
            BlackOut.DOFade(0.0f, 0.1f).SetDelay(0.1f);
        });
    }

    public void OnZukan()
    {
        BlackOut.DOFade(1.0f, 0.1f).OnComplete(() =>
        { 
            ShowScreen(Mode.Zukan);
            BlackOut.DOFade(0.0f, 0.1f).SetDelay(0.1f);
        });
    }
}
