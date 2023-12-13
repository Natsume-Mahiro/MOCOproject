using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UnitUI : MonoBehaviour
{
    // 次に生成される Unit
    [SerializeField] List<Sprite> UnitSprites;
    [SerializeField] Image GreenBack;
    [SerializeField] Image BlueBack;
    [SerializeField] Image OrangeBack;
    [SerializeField] Image WAKU;

    // アニメーション
    [SerializeField] private Image GreenImageToAnimate;
    [SerializeField] private Image BlueImageToAnimate;
    [SerializeField] private Image OrangeImageToAnimate;
    private float animationDuration = 2.0f; // アニメーションの時間（秒）
    private int GreenAnimationCount;
    private int BlueAnimationCount;
    private int OrangeAnimationCount;

    // 数字
    [SerializeField] TextMeshProUGUI GreenNumber;
    [SerializeField] TextMeshProUGUI BlueNumber;

    // ゲッター
    public int GetAnimationCount(string Color)
    {
        if (Color == "GREEN") return GreenAnimationCount;
        else if (Color == "BLUE") return BlueAnimationCount;
        else if (Color == "ORANGE") return OrangeAnimationCount;
        return -1;
    }

    // セッター
    public void SetAnimationCount(string Color)
    {
        if (Color == "GREEN")
        {
            GreenAnimationCount -= 1;
            GreenNumber.text = string.Format("{0}", GreenAnimationCount);
        }
        else if (Color == "BLUE")
        {
            BlueAnimationCount -= 1;
            BlueNumber.text = string.Format("{0}", BlueAnimationCount);
        }
        else if (Color == "ORANGE") OrangeAnimationCount -= 1;
    }

    void Start()
    {
        // FillAmountを最初に設定
        GreenImageToAnimate.fillAmount = 0.0f; // 初めは完全に非表示
        BlueImageToAnimate.fillAmount = 0.0f; // 初めは完全に非表示
        OrangeImageToAnimate.fillAmount = 0.0f; // 初めは完全に非表示

        GreenAnimationCount = 5;
        BlueAnimationCount = 5;
        OrangeAnimationCount = 1;

        GreenNumber.text = string.Format("{0}", GreenAnimationCount);
        BlueNumber.text = string.Format("{0}", BlueAnimationCount);
    }

    public void _Update()
    {
        if (GreenImageToAnimate.fillAmount == 0.0f && GreenAnimationCount < 5)
        {
            GreenImageToAnimate.fillAmount = 1.0f;

            // DOTweenを使用してFillAmountを変更
            GreenImageToAnimate.DOFillAmount(0.0f, animationDuration)
                               .SetEase(Ease.Linear)
                               .OnComplete(() =>
                               {
                                   GreenAnimationCount += 1;
                                   GreenNumber.text = string.Format("{0}", GreenAnimationCount);
                               });
        }
        if (BlueImageToAnimate.fillAmount == 0.0f && BlueAnimationCount < 5)
        {
            BlueImageToAnimate.fillAmount = 1.0f;

            // DOTweenを使用してFillAmountを変更
            BlueImageToAnimate.DOFillAmount(0.0f, 1.8f)
                            .SetEase(Ease.Linear)
                            .OnComplete(() =>
                            {
                                BlueAnimationCount += 1;
                                BlueNumber.text = string.Format("{0}", BlueAnimationCount);
                            });
        }
        if (OrangeImageToAnimate.fillAmount == 0.0f && OrangeAnimationCount < 1)
        {
            OrangeImageToAnimate.fillAmount = 1.0f;

            // DOTweenを使用してFillAmountを変更
            OrangeImageToAnimate.DOFillAmount(0.0f, animationDuration).SetEase(Ease.Linear).OnComplete(() => OrangeAnimationCount += 1);
        }
    }

    public void SetBack(GameObject Unit, string Color)
    {
        foreach (Sprite UnitSprite in UnitSprites)
        {
            if (Color == "GREEN" && Unit.name == UnitSprite.name)
            {
                GreenBack.sprite = UnitSprite;
                break;
            }
            else if (Color == "BLUE" && Unit.name == UnitSprite.name)
            {
                BlueBack.sprite = UnitSprite;
                break;
            }
            else if (Color == "ORANGE")
            {
                if (Unit == null)
                {
                    OrangeBack.sprite = UnitSprites[UnitSprites.Count - 1];
                    break;
                }
                else if (Unit != null && Unit.name == UnitSprite.name)
                {
                    OrangeBack.sprite = UnitSprite;
                    break;
                }
            }
        }
    }

    public void ChangeWAKU(string Color)
    {
        if (Color == "GREEN") WAKU.transform.position = GreenBack.transform.position;
        else if (Color == "BLUE") WAKU.transform.position = BlueBack.transform.position;
        else WAKU.transform.position = OrangeBack.transform.position;
    }

    void OnDestroy()
    {
        // DOTweenアニメーションをキャンセル
        DOTween.Kill(GreenImageToAnimate);
        DOTween.Kill(BlueImageToAnimate);
        DOTween.Kill(OrangeImageToAnimate);
    }
}
