using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerDataPrefab;
    [SerializeField] private CreateUnit createUnit;
    [SerializeField] private UnitUI unitUI;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private Core core;

    PlayerData PlayerData;
    private MODE mode;
    private UNIT unit;

    private bool isRESULT;

    // ゲームの状態
    enum MODE
    {
        GAME,
        STOP,
        SETTING,
        RESULT
    }

    // 選択しているユニット
    enum UNIT
    {
        GREEN,
        BLUE,
        ORANGE
    }

    void Awake()
    {
        // 万が一 PlayerData がないときのため
        Instantiate(PlayerDataPrefab);

        mode = MODE.GAME;
        unit = UNIT.GREEN;
        isRESULT = false;
        unitUI.ChangeWAKU("GREEN");

        gameUI.SetGameUI();

        if (SoundManager.Instance != null) SoundManager.Instance.PlayBGM(1);
    }

    void Update()
    {
        switch (mode)
        {
            case MODE.GAME:
                if (Time.timeScale != 1.0f) Time.timeScale = 1.0f;
                if (SCOREandRESULT.Instance.TotalEnemies == SCOREandRESULT.Instance.DefeatedEnemies)
                {
                    gameUI.GameVictory(true);
                    mode = MODE.RESULT;
                }
                else if (core.Defeat())
                {
                    gameUI.GameVictory(false);
                    mode = MODE.RESULT;
                }
                
                HandleGameMode();
                break;

            case MODE.STOP:
                if (Time.timeScale != 0.0f) Time.timeScale = 0.0f;
                break;

            case MODE.SETTING:
                if (Time.timeScale != 0.0f) Time.timeScale = 0.0f;
                break;

            case MODE.RESULT:
                if (!isRESULT)
                {
                    GameClear();
                    SCOREandRESULT.Instance.ScoreUpdate();
                    isRESULT = true;
                }
                break;
        }
    }

    void GameClear()
    {
        gameUI.StartResultAnimation();
    }

    void HandleGameMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeGREEN();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeBLUE();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeORANGE();

        if (unit == UNIT.GREEN)
        {
            createUnit.ClickCreateUnit("GREEN");
        }
        else if (unit == UNIT.BLUE)
        {
            createUnit.ClickCreateUnit("BLUE");
        }
        else
        {
            createUnit.ClickSpuitUnit();
        }
        unitUI._Update();
    }

    // ゲームモードの変更
    public void STOPkaraGAME()
    {
        mode = MODE.GAME;
        gameUI.ChangeStopUI();
    }
    public void SETTINGkaraGAME()
    {
        mode = MODE.GAME;
        gameUI.CloseSETTING();
    }
    public void GAMEkaraSTOP()
    {
        mode = MODE.STOP;
        gameUI.ChangeMoveUI();
    }
    public void GAMEkaraSETTING()
    {
        mode = MODE.SETTING;
        gameUI.OpenSETTING();
    }
    
    // ユニットの変更
    public void ChangeGREEN()
    {
        unit = UNIT.GREEN;
        unitUI.ChangeWAKU("GREEN");
    }

    public void ChangeBLUE()
    {
        unit = UNIT.BLUE;
        unitUI.ChangeWAKU("BLUE");
    }

    public void ChangeORANGE()
    {
        unit = UNIT.ORANGE;
        unitUI.ChangeWAKU("ORANGE");
    }
}
