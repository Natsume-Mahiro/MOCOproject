using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCOREandRESULT : SingletonMonoBehaviour<SCOREandRESULT>
{
    [Header("GREEN")]
    public int CreateGreenLevelFive = 0;
    public int CreateGreenLevelFour = 0;
    public int CreateGreenLevelthree = 0;
    [Header("BLUE")]
    public int CreateBlueLevelFive = 0;
    public int CreateBlueLevelFour = 0;
    public int CreateBlueLevelthree = 0;
    [Header("ENEMY")]
    public int TotalEnemies = 0;
    public int DefeatedEnemies = 0;
    [Header("SCORE")]
    public int SCORE = 0;

    int SceneNumber = 0;

    void Start()
    {
        // 現在のシーンの名前を取得
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // シーンの名前から数字部分を抽出し、intに変換
        string SceneNumberStr = System.Text.RegularExpressions.Regex.Replace(sceneName, @"\D", "");
        int.TryParse(SceneNumberStr, out SceneNumber);
    }

    public void ScoreUpdate()
    {
        if (SceneNumber == 0) return;

        if (PlayerData.Instance != null)
        {
            if (PlayerData.Instance.Scores[SceneNumber - 1] < SCORE) PlayerData.Instance.Scores[SceneNumber - 1] = SCORE;
            PlayerData.Instance.Money += SCORE / 10;
        }
    }
}
