using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IkuseiUI : MonoBehaviour
{
    [SerializeField] StateText StateText;

    [SerializeField] GameObject LevelUp_Black;
    [SerializeField] GameObject TARINAI;
    [SerializeField] TextMeshProUGUI currentMoney;
    [SerializeField] TextMeshProUGUI NextMoney;
    private string Unit;

    void Start()
    {
        StateText._Start();
        CloseLevelUpText();
        CloseTARINAI();
    }

    void MoneyText()
    {
        currentMoney.text = string.Format("{0}", PlayerData.Instance.Money);
    }

    void NextMoneyText(int UnitLevel)
    {
        NextMoney.text = string.Format("{0}", PlayerData.Instance.Money - (UnitLevel * 15));
    }

    public void LevelUp()
    {
        if (Unit == "GREEN" && PlayerData.Instance.Money - (PlayerData.Instance.GreenLevel * 15) >= 0) GreenLevelUp();
        else if (Unit == "YELLOW" && PlayerData.Instance.Money - (PlayerData.Instance.YellowLevel * 15) >= 0) YellowLevelUp();
        else TARINAI.SetActive(true);
    }

    void GreenLevelUp()
    {
        PlayerData.Instance.Money -= (PlayerData.Instance.GreenLevel * 15);

        PlayerData.Instance.GreenLevel += 1;
        StateText.UpdateUI(); // ステータスを更新
    }

    void YellowLevelUp()
    {
        PlayerData.Instance.Money -= (PlayerData.Instance.YellowLevel * 15);
        
        PlayerData.Instance.YellowLevel += 1;
        StateText.UpdateUI(); // ステータスを更新
    }

    public void GreenOpenLevelUpText()
    {
        if (PlayerData.Instance.GreenLevel < 20)
        {
            Unit = "GREEN";
            MoneyText();
            NextMoneyText(PlayerData.Instance.GreenLevel);
            LevelUp_Black.SetActive(true);
        }
    }

    public void YellowOpenLevelUpText()
    {
        if (PlayerData.Instance.YellowLevel < 20)
        {
            Unit = "YELLOW";
            MoneyText();
            NextMoneyText(PlayerData.Instance.YellowLevel);
            LevelUp_Black.SetActive(true);
        }
    }

    public void CloseLevelUpText()
    {
        LevelUp_Black.SetActive(false);
    }

    public void CloseTARINAI()
    {
        TARINAI.SetActive(false);
    }
}
