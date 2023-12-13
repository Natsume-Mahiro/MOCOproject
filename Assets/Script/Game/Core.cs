using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Core : MonoBehaviour, IDamage
{
    [SerializeField] Slider slider;

    private int maxHP;
    private int currentHP;
    private bool isDefeat;

    void Start()
    {
        maxHP = 1000;
        currentHP = maxHP;

        isDefeat = false;
    }

    // HPを増減
    public void Damage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // DOTweenを使用してスライダーの値を滑らかに変化させる
        float targetValue = (float)currentHP / (float)maxHP;
        slider.DOValue(targetValue, 0.5f)
                .OnComplete(() =>
                {
                    if (currentHP <= 0)
                    {
                        isDefeat = true;
                    }
                });
    }

    public bool Defeat()
    {
        return isDefeat;
    }

    void OnDestroy()
    {
        DOTween.Kill(slider);
    }
}
