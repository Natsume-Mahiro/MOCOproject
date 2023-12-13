using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpriteAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] attackFrames; // "Attack"アニメーションのスプライトフレーム
    [SerializeField] Sprite[] stayFrames;   // "Stay"アニメーションのスプライトフレーム
    [SerializeField] Sprite[] deadFrames;   // "Dead"アニメーションのスプライトフレーム
    [Header("アニメーションが一回分終わる時間")]
    [SerializeField] float attackFrameRate; // Attackアニメーションのフレームの更新レート
    [SerializeField] float stayFrameRate;   // Stayアニメーションのフレームの更新レート
    [SerializeField] float deadFrameRate;   // Deadアニメーションのフレームの更新レート

    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex;
    private float timer;
    private string currentState;
    private Sprite previousSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackFrameRate /= attackFrames.Length;
        stayFrameRate /= stayFrames.Length;
        deadFrameRate /= deadFrames.Length;
        currentFrameIndex = 0;
        timer = 0;
        previousSprite = null;
        currentState = "Stay"; // 初期状態を"Stay"に設定
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
            case "Attack":
                return attackFrames;
            case "Stay":
                return stayFrames;
            case "Dead":
                return deadFrames;
            default:
                return null;
        }
    }

    // 状態に応じたフレームレートを返す関数
    private float GetFrameRateForState(string state)
    {
        switch (state)
        {
            case "Attack":
                return attackFrameRate;
            case "Stay":
                return stayFrameRate;
            case "Dead":
                return deadFrameRate;
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
