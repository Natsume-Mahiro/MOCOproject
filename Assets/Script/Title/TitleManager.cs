using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using SaveManagement;

public class TitleManager : MonoBehaviour
{
    // プレイヤーデータ
    [SerializeField] GameObject PlayerDataPrefab;
    [SerializeField] Image BlackOut;

    bool click;

    void Awake()
    {
        // プレイヤーデータの生成
        Instantiate(PlayerDataPrefab);

        BlackOut.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        click = false;
    }

    void Start()
    {
        Data.CreateSaveFolder();
        Load.DataLoad();

        PlayerData.Instance.GreenLevel = Load.IntData(PlayerData.Instance.GreenLevel, "greenMoco");
        PlayerData.Instance.YellowLevel = Load.IntData(PlayerData.Instance.YellowLevel, "yellowMoco");
        PlayerData.Instance.Scores = Load.IntData(PlayerData.Instance.Scores, "Score");
        PlayerData.Instance.Money = Load.IntData(PlayerData.Instance.Money, "Money");

        if (SoundManager.Instance != null) SoundManager.Instance.PlayBGM(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!click)
            {
                click = true;
                BlackOut.DOFade(1.0f, 1.0f).OnComplete(() =>
                                            {
                                                SceneManager.LoadScene("Main");
                                            });
            }
        }
    }
}
