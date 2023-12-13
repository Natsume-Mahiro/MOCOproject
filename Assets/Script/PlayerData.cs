using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveManagement;

public class PlayerData : SingletonMonoBehaviour<PlayerData>
{
    override protected void Awake()
    {
        base.Awake();

        // シーン遷移で破棄されないようにする
        DontDestroyOnLoad(this);

        // Prefab から生成するときに (Clone) を外す
        this.gameObject.name = "PlayerData";

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
    }

    //---------- キャラクター ----------//
    private struct CharacterInfo
    {
        public int Level;
    }

    private CharacterInfo greenMoco = new CharacterInfo { Level = 1 };
    private CharacterInfo yellowMoco = new CharacterInfo { Level = 1 };

    // レベルの受け渡し
    public int GreenLevel
    {
        get { return greenMoco.Level; }
        set { greenMoco.Level = value; }
    }

    public int YellowLevel
    {
        get { return yellowMoco.Level; }
        set { yellowMoco.Level = value; }
    }

    // 体力の計算
    public int GreenHitPoint => CalculateHitPoint(greenMoco);
    public int YellowHitPoint => CalculateHitPointY(yellowMoco);

    // 攻撃力の計算
    public int GreenAttack => CalculateAttack(greenMoco);
    public int YellowAttack => CalculateAttackY(yellowMoco);

    // 緑の体力および攻撃力を計算
    private int CalculateHitPoint(CharacterInfo character)
    {
        return 10 + (character.Level - 1) * 2;
    }

    private int CalculateAttack(CharacterInfo character)
    {
        return 10 + (character.Level - 1) * 2;
    }

    // 黄の体力および攻撃力を計算
    private int CalculateHitPointY(CharacterInfo character)
    {
        return 11 + (character.Level - 1) * 3;
    }

    private int CalculateAttackY(CharacterInfo character)
    {
        return 9 + (character.Level - 1) * 1;
    }

    //---------- SCORE & MONEY ----------//
    private int[] scores = new int[5];
    private int money = 0;

    public int[] Scores
    {
        get { return scores; }
        set { scores = value; }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    void OnApplicationQuit()
    {
        Save.IntDataClear();
        Save.IntData("greenMoco", GreenLevel);
        Save.IntData("yellowMoco", YellowLevel);
        Save.IntData("Score", Scores);
        Save.IntData("Money", Money);
        Save.DataSave();
    }

    //---------- セーブ時に受け渡しする変数 ----------//
    // PlayerData.Instance.を付ければどこからでもアクセスできます
    // GreenLevel
    // YellowLevel
    // Scores[0] ~ Scores[4]
    // Money
}
