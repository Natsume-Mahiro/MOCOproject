using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    public Image Title;
    public float blinkDuration = 1.0f;
    private Tweener blinkTween;

    private void Start()
    {
        if (Title != null)
        {
            blinkTween = Title.DOFade(0.0f, blinkDuration)
                                .SetEase(Ease.InCubic)
                                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnDestroy()
    {
        if (blinkTween != null)
        {
            blinkTween.Kill();
        }
    }
}
