using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI GreenState;
    [SerializeField] TextMeshProUGUI YellowState;

    public void _Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateCharacterUI(PlayerData.Instance.GreenLevel, PlayerData.Instance.GreenHitPoint, PlayerData.Instance.GreenAttack, GreenState);
        UpdateCharacterUI(PlayerData.Instance.YellowLevel, PlayerData.Instance.YellowHitPoint, PlayerData.Instance.YellowAttack, YellowState);
    }

    private void UpdateCharacterUI(int level, int hitPoint, int attack, TextMeshProUGUI stateText)
    {
        if (level < 1)
        {
            stateText.text = "N/A";
        }
        else if (level >= 20)
        {
            stateText.text = string.Format("MAX\n{0}\n{1}", hitPoint, attack);
        }
        else
        {
            stateText.text = string.Format("{0}\n{1}\n{2}", level, hitPoint, attack);
        }
    }
}
