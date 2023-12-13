using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameUI : MonoBehaviour
{
    public void SetGameUI()
    {
        ChangeStopUI();
        CloseSETTING();
        DisableResultBlack();
        ChangeZoomIn();

        BlackOutStart();

        if (SoundManager.Instance != null)
        {
            if (SoundManager.Instance.BGMplaying) OnBGM();
            else OffBGM();
            if (SoundManager.Instance.SEplaying) OnFX();
            else OffFX();
        }
    }

    //---------- TIMESTOP ----------//
    [Header("TIMESTOP")]
    [SerializeField] Image Transparent_Back;
    [SerializeField] Image STOP;
    [SerializeField] Image MOVE;

    public void ChangeStopUI()
    {
        STOP.gameObject.SetActive(true);
        MOVE.gameObject.SetActive(false);
        Transparent_Back.gameObject.SetActive(false);
    }

    public void ChangeMoveUI()
    {
        STOP.gameObject.SetActive(false);
        MOVE.gameObject.SetActive(true);
        Transparent_Back.gameObject.SetActive(true);
    }

    //---------- SETTING ----------//
    [Header("SETTING")]
    [SerializeField] Image Black;
    [SerializeField] Image BACK;
    [SerializeField] Image FX_ON;
    [SerializeField] Image FX_OFF;
    [SerializeField] Image BGM_ON;
    [SerializeField] Image BGM_OFF;
    [SerializeField] Image EXIT;

    [SerializeField] Sprite SelectON;
    [SerializeField] Sprite SelectOFF;
    [SerializeField] Sprite not_SelectON;
    [SerializeField] Sprite not_SelectOFF;

    [SerializeField] Image BlackOut;

    public void OpenSETTING()
    {
        Black.gameObject.SetActive(true);
    }

    public void CloseSETTING()
    {
        Black.gameObject.SetActive(false);
    }

    public void OnFX()
    {
        FX_ON.sprite = SelectON;
        FX_OFF.sprite = not_SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OnSE();
    }

    public void OffFX()
    {
        FX_ON.sprite = not_SelectON;
        FX_OFF.sprite = SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OffSE();
    }

    public void OnBGM()
    {
        BGM_ON.sprite = SelectON;
        BGM_OFF.sprite = not_SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OnBGM();
    }

    public void OffBGM()
    {
        BGM_ON.sprite = not_SelectON;
        BGM_OFF.sprite = SelectOFF;

        if (SoundManager.Instance != null) SoundManager.Instance.OffBGM();
    }

    void BlackOutStart()
    {
        BlackOut.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        BlackOut.gameObject.SetActive(true);
        BlackOut.DOFade(0.0f, 1.0f).OnComplete(() => { BlackOut.gameObject.SetActive(false); });
    }

    public void ExitStage()
    {
        BlackOut.gameObject.SetActive(true);
        BlackOut.DOFade(1.0f, 1.0f).OnComplete(() =>
                                    {
                                        SceneManager.LoadScene("Main");
                                    });
    }

    //---------- ZOOM ----------//
    [Header("ZOOM")]
    [SerializeField] Image ZOOMIN;
    [SerializeField] Image ZOOMOUT;

    public void ChangeZoomIn()
    {
        ZOOMIN.gameObject.SetActive(true);
        ZOOMOUT.gameObject.SetActive(false);
    }

    public void ChangeZoomOut()
    {
        ZOOMIN.gameObject.SetActive(false);
        ZOOMOUT.gameObject.SetActive(true);
    }

    //---------- RESULT ----------//
    [Header("RESULT")]
    [SerializeField] Image Result_Black;
    [SerializeField] Image RESULT_BACK;
    [SerializeField] Image RESULT;
    [SerializeField] Image RESULT_EXIT;

    [SerializeField] Image GreenLv5;
    [SerializeField] Image GreenLv4;
    [SerializeField] Image GreenLv3;
    [SerializeField] TextMeshProUGUI Green_KAKERU;
    [SerializeField] TextMeshProUGUI Green_TEN;
    [SerializeField] TextMeshProUGUI Green_SCORE;
    [SerializeField] Image BlueLv5;
    [SerializeField] Image BlueLv4;
    [SerializeField] Image BlueLv3;
    [SerializeField] TextMeshProUGUI Blue_KAKERU;
    [SerializeField] TextMeshProUGUI Blue_TEN;
    [SerializeField] TextMeshProUGUI Blue_SCORE;

    [SerializeField] TextMeshProUGUI PROGRESS;
    [SerializeField] TextMeshProUGUI SCORE;
    [SerializeField] TextMeshProUGUI REWARD;
    [SerializeField] Image MONEY;

    [SerializeField] Sprite Victory;
    [SerializeField] Sprite Lose;

    private Sequence resultSeq; // シーケンスを格納する変数

    public void DisableResultBlack()
    {
        Result_Black.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        RESULT_BACK.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        RESULT.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        RESULT_EXIT.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        GreenLv5.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        GreenLv4.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        GreenLv3.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Green_KAKERU.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Green_TEN.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Green_SCORE.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        BlueLv5.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        BlueLv4.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        BlueLv3.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Blue_KAKERU.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Blue_TEN.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Blue_SCORE.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        PROGRESS.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        SCORE.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        REWARD.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        MONEY.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        Result_Black.gameObject.SetActive(false);
    }

    public void GameVictory(bool Vic)
    {
        if (Vic) RESULT.sprite = Victory;
        else RESULT.sprite = Lose;
    }

    public void StartResultAnimation()
    {
        Result_Black.gameObject.SetActive(true);

        UpdateScore();

        // 同時に実行されるアニメーション
        resultSeq = DOTween.Sequence();
        resultSeq.Append(Result_Black.DOFade(0.9f, 1.0f));
        resultSeq.Append(RESULT.DOFade(1.0f, 1.0f));
        resultSeq.Append(RESULT.rectTransform.DOAnchorPosY(400.0f, 1.0f));
        resultSeq.Append(RESULT_BACK.DOFade(1.0f, 1.0f));
        resultSeq.Join(SCORE.DOFade(1.0f, 1.0f));

        resultSeq.Join(GreenLv5.DOFade(1.0f, 1.0f));
        resultSeq.Join(GreenLv4.DOFade(1.0f, 1.0f));
        resultSeq.Join(GreenLv3.DOFade(1.0f, 1.0f));
        resultSeq.Join(Green_KAKERU.DOFade(1.0f, 1.0f));
        resultSeq.Join(Green_TEN.DOFade(1.0f, 1.0f));
        resultSeq.Join(Green_SCORE.DOFade(1.0f, 1.0f));
        resultSeq.Join(BlueLv5.DOFade(1.0f, 1.0f));
        resultSeq.Join(BlueLv4.DOFade(1.0f, 1.0f));
        resultSeq.Join(BlueLv3.DOFade(1.0f, 1.0f));
        resultSeq.Join(Blue_KAKERU.DOFade(1.0f, 1.0f));
        resultSeq.Join(Blue_TEN.DOFade(1.0f, 1.0f));
        resultSeq.Join(Blue_SCORE.DOFade(1.0f, 1.0f));

        resultSeq.Join(PROGRESS.DOFade(1.0f, 1.0f));
        resultSeq.Join(SCORE.DOFade(1.0f, 1.0f));
        resultSeq.Join(REWARD.DOFade(1.0f, 1.0f));
        resultSeq.Join(MONEY.DOFade(1.0f, 1.0f));

        resultSeq.Append(RESULT_EXIT.DOFade(1.0f, 1.0f));
        resultSeq.OnComplete(() =>
        {
            resultSeq.Kill();
        });
    }

    private int LevelFive = 1000;
    private int LevelFour = 200;
    private int LevelThree = 40;

    void UpdateScore()
    {
        SCOREandRESULT.Instance.SCORE = (SCOREandRESULT.Instance.CreateGreenLevelFive * LevelFive +
                                        SCOREandRESULT.Instance.CreateGreenLevelFour * LevelFour +
                                        SCOREandRESULT.Instance.CreateGreenLevelthree * LevelThree +
                                        SCOREandRESULT.Instance.CreateBlueLevelFive * LevelFive +
                                        SCOREandRESULT.Instance.CreateBlueLevelFour * LevelFour +
                                        SCOREandRESULT.Instance.CreateBlueLevelthree * LevelThree) *
                                        (SCOREandRESULT.Instance.DefeatedEnemies / SCOREandRESULT.Instance.TotalEnemies);

        Green_KAKERU.text = string.Format("× {0}\n\n× {1}\n\n× {2}" , SCOREandRESULT.Instance.CreateGreenLevelFive
                                                                    , SCOREandRESULT.Instance.CreateGreenLevelFour
                                                                    , SCOREandRESULT.Instance.CreateGreenLevelthree);
        Green_SCORE.text = string.Format("× {0}\n\n× {1}\n\n× {2}"  , SCOREandRESULT.Instance.CreateGreenLevelFive * LevelFive
                                                                    , SCOREandRESULT.Instance.CreateGreenLevelFour * LevelFour
                                                                    , SCOREandRESULT.Instance.CreateGreenLevelthree * LevelThree);

        Blue_KAKERU.text = string.Format("× {0}\n\n× {1}\n\n× {2}"  , SCOREandRESULT.Instance.CreateBlueLevelFive
                                                                    , SCOREandRESULT.Instance.CreateBlueLevelFour
                                                                    , SCOREandRESULT.Instance.CreateBlueLevelthree);
        Blue_SCORE.text = string.Format("× {0}\n\n× {1}\n\n× {2}"   , SCOREandRESULT.Instance.CreateBlueLevelFive * LevelFive
                                                                    , SCOREandRESULT.Instance.CreateBlueLevelFour * LevelFour
                                                                    , SCOREandRESULT.Instance.CreateBlueLevelthree * LevelThree);

        PROGRESS.text = string.Format("PROGRESS : {0} %", (int)((float)SCOREandRESULT.Instance.DefeatedEnemies / (float)SCOREandRESULT.Instance.TotalEnemies * 100));
        SCORE.text = string.Format("SCORE : {0}", SCOREandRESULT.Instance.SCORE);
        REWARD.text = string.Format("REWARD :    {0}", SCOREandRESULT.Instance.SCORE / 10);
    }

    void OnDestroy()
    {
        if (resultSeq != null)
        {
            resultSeq.Kill();
        }
    }
}
