using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MIZUniUKU : MonoBehaviour
{
    private float minMoveDistance = 0.2f;
    private float maxMoveDistance = 0.7f;
    float randomDistance = 0.5f;
    float moveTime = 6.0f;
    Tween moveTween;

    void Start()
    {
        // 移動距離と時間をランダムに決定
        randomDistance = Random.Range(minMoveDistance, maxMoveDistance);
        moveTime = 2.0f + 8.0f * randomDistance;

        // randomDistance で決定した距離を動くアニメーション
        moveTween = transform.DOMoveY(randomDistance, moveTime)
            .SetEase(Ease.InOutQuad)
            .SetRelative();

        // 移動アニメーションが完了したら元の位置に戻るアニメーションを開始
        moveTween.OnComplete(() => ReturnPosition());
        
        // ループ設定（無限ループ）
        moveTween.SetLoops(-1, LoopType.Yoyo);
    }

    private void ReturnPosition()
    {
        // 元の位置への戻るアニメーション
        transform.DOMoveY(randomDistance, moveTime)
            .SetEase(Ease.InOutQuad)
            .SetRelative();
    }

    // シーン遷移前にアニメーションを中止
    void OnDestroy()
    {
        if (moveTween != null)
        {
            moveTween.Kill(); // アニメーションを中止
        }
    }
}
