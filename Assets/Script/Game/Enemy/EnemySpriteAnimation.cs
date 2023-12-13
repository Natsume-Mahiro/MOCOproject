using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] attackRightFrames; // "Attack"アニメーションの右向きスプライトフレーム
    [SerializeField] Sprite[] attackLeftFrames;  // "Attack"アニメーションの左向きスプライトフレーム
    [SerializeField] Sprite[] moveFrames;        // "Move"アニメーションのスプライトフレーム
    [SerializeField] float attackRightFrameRate; // Attackアニメーションの右向きフレームの更新レート
    [SerializeField] float attackLeftFrameRate;  // Attackアニメーションの左向きフレームの更新レート
    [SerializeField] float moveFrameRate;        // Moveアニメーションのフレームの更新レート

    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex;
    private float timer;
    private string currentState;
    private Sprite previousSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackRightFrameRate /= attackRightFrames.Length;
        attackLeftFrameRate /= attackLeftFrames.Length;
        moveFrameRate /= moveFrames.Length;
        currentFrameIndex = 0;
        timer = 0;
        previousSprite = null;
        currentState = "Move"; // 初期状態を"Move"に設定
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 現在の状態に応じてスプライトを更新
        Sprite[] currentFrames = GetFramesForState(currentState);
        float currentFrameRate = GetFrameRateForState(currentState);

        if (currentFrames != null)
        {
            if (timer >= currentFrameRate)
            {
                // 次のフレームを表示
                spriteRenderer.sprite = currentFrames[currentFrameIndex];
                currentFrameIndex = (currentFrameIndex + 1) % currentFrames.Length;
                timer = 0;
            }
        }
    }

    // 状態に応じてスプライトフレームを返す関数
    private Sprite[] GetFramesForState(string state)
    {
        switch (state)
        {
            case "AttackRight":
                return attackRightFrames;
            case "AttackLeft":
                return attackLeftFrames;
            case "Move":
                return moveFrames;
            default:
                return null;
        }
    }

    // 状態に応じたフレームレートを返す関数
    private float GetFrameRateForState(string state)
    {
        switch (state)
        {
            case "AttackRight":
                return attackRightFrameRate;
            case "AttackLeft":
                return attackLeftFrameRate;
            case "Move":
                return moveFrameRate;
            default:
                return 0.0f;
        }
    }

    // 状態を切り替える関数
    public void ChangeState(string newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            currentFrameIndex = 0;
        }
    }

    // 現在のスプライトフレームを返す関数
    public Sprite GetCurrentSprite()
    {
        Sprite[] currentFrames = GetFramesForState(currentState);
        if (currentFrames != null && currentFrames.Length > 0 && previousSprite != currentFrames[currentFrameIndex])
        {
            previousSprite = currentFrames[currentFrameIndex];
            return currentFrames[currentFrameIndex];
        }
        return null;
    }
}
