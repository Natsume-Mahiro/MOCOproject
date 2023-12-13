using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [SerializeField] Vector3Int[] ScoreLine = new Vector3Int[5];

    void Start()
    {
        popStart();
        stageStart();
    }

    //---------- STAGE ----------//
    [Header("STAGE")]
    [SerializeField] Image STAGE1;
    [SerializeField] Image STAGE2;
    [SerializeField] Image STAGE3;
    [SerializeField] Image STAGE4;
    [SerializeField] Image STAGE5;

    [SerializeField] Sprite S0;
    [SerializeField] Sprite S1;
    [SerializeField] Sprite S2;
    [SerializeField] Sprite S3;

    void stageStart()
    {
        if (PlayerData.Instance.Scores[0] >= ScoreLine[0].z) STAGE1.sprite = S3;
        else if (PlayerData.Instance.Scores[0] >= ScoreLine[0].y) STAGE1.sprite = S2;
        else if (PlayerData.Instance.Scores[0] >= ScoreLine[0].x) STAGE1.sprite = S1;
        else STAGE1.sprite = S0;

        if (PlayerData.Instance.Scores[1] >= ScoreLine[1].z) STAGE2.sprite = S3;
        else if (PlayerData.Instance.Scores[1] >= ScoreLine[1].y) STAGE2.sprite = S2;
        else if (PlayerData.Instance.Scores[1] >= ScoreLine[1].x) STAGE2.sprite = S1;
        else STAGE2.sprite = S0;

        if (PlayerData.Instance.Scores[2] >= ScoreLine[2].z) STAGE3.sprite = S3;
        else if (PlayerData.Instance.Scores[2] >= ScoreLine[2].y) STAGE3.sprite = S2;
        else if (PlayerData.Instance.Scores[2] >= ScoreLine[2].x) STAGE3.sprite = S1;
        else STAGE3.sprite = S0;

        if (PlayerData.Instance.Scores[3] >= ScoreLine[3].z) STAGE4.sprite = S3;
        else if (PlayerData.Instance.Scores[3] >= ScoreLine[3].y) STAGE4.sprite = S2;
        else if (PlayerData.Instance.Scores[3] >= ScoreLine[3].x) STAGE4.sprite = S1;
        else STAGE4.sprite = S0;

        if (PlayerData.Instance.Scores[4] >= ScoreLine[4].z) STAGE5.sprite = S3;
        else if (PlayerData.Instance.Scores[4] >= ScoreLine[4].y) STAGE5.sprite = S2;
        else if (PlayerData.Instance.Scores[4] >= ScoreLine[4].x) STAGE5.sprite = S1;
        else STAGE5.sprite = S0;
    }

    //---------- POPUP ----------//
    [Header("POPUP")]
    [SerializeField] Image Stage_Black;
    [SerializeField] Image Stage;
    [SerializeField] Image STAR1;
    [SerializeField] Image STAR2;
    [SerializeField] Image STAR3;

    [SerializeField] List<Sprite> StageSprite;
    [SerializeField] Sprite no_star;
    [SerializeField] Sprite star;

    [SerializeField] TextMeshProUGUI StageText;
    [SerializeField] TextMeshProUGUI BestScore;

    [SerializeField] Image BlackOut;

    int SelectStage;

    void popStart()
    {
        CloseStagePop();
        SelectStage = 0;

        BlackOut.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        BlackOut.gameObject.SetActive(true);
        BlackOut.DOFade(0.0f, 1.0f).OnComplete(() => { BlackOut.gameObject.SetActive(false); });
    }

    public void Stage1()
    {
        SelectStage = 1;
        OpenStagePop();
    }

    public void Stage2()
    {
        SelectStage = 2;
        OpenStagePop();
    }

    public void Stage3()
    {
        SelectStage = 3;
        OpenStagePop();
    }

    public void Stage4()
    {
        SelectStage = 4;
        OpenStagePop();
    }

    public void Stage5()
    {
        SelectStage = 5;
        OpenStagePop();
    }
    
    public void GameStart()
    {
        if (SelectStage != 0)
        {
            BlackOut.gameObject.SetActive(true);
            BlackOut.DOFade(1.0f, 1.0f).OnComplete(() =>
                                            {
                                                if (SelectStage == 1) SceneManager.LoadScene("Stage1");
                                                if (SelectStage == 2) SceneManager.LoadScene("Stage2");
                                                if (SelectStage == 3) SceneManager.LoadScene("Stage3");
                                                if (SelectStage == 4) SceneManager.LoadScene("Stage4");
                                                if (SelectStage == 5) SceneManager.LoadScene("Stage5");
                                            });
        }
    }

    private void OpenStagePop()
    {
        if (SelectStage != 0)
        {
            if (StageSprite.Count >= SelectStage)
            {
                Stage.sprite = StageSprite[SelectStage - 1];
            }
            else Stage.sprite = null;

            if (PlayerData.Instance.Scores[SelectStage - 1] >= ScoreLine[SelectStage - 1].x) STAR1.sprite = star;
            if (PlayerData.Instance.Scores[SelectStage - 1] >= ScoreLine[SelectStage - 1].y) STAR2.sprite = star;
            if (PlayerData.Instance.Scores[SelectStage - 1] >= ScoreLine[SelectStage - 1].z) STAR3.sprite = star;

            BestScore.text = string.Format("Best Score : {0}", PlayerData.Instance.Scores[SelectStage - 1]);
        }
        StageText.text = string.Format("STAGE {0}", SelectStage);

        Stage_Black.gameObject.SetActive(true);
    }

    public void CloseStagePop()
    {
        Stage_Black.gameObject.SetActive(false);
        STAR1.sprite = no_star;
        STAR2.sprite = no_star;
        STAR3.sprite = no_star;
        SelectStage = 0;
    }
}
