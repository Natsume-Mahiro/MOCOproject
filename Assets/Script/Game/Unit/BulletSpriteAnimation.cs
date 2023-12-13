using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpriteAnimation : MonoBehaviour
{
    public Sprite sprite1; // 切り替えるスプライト1
    public Sprite sprite2; // 切り替えるスプライト2
    public float swapInterval; // スプライトを切り替える間隔（秒）

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool isUsingSprite1; // 現在のスプライトがsprite1かどうか

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0;
        isUsingSprite1 = true;
        
        // 最初はsprite1を表示
        spriteRenderer.sprite = sprite1;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= swapInterval)
        {
            // swapInterval秒ごとにスプライトを切り替え
            if (isUsingSprite1)
            {
                spriteRenderer.sprite = sprite2;
            }
            else
            {
                spriteRenderer.sprite = sprite1;
            }

            isUsingSprite1 = !isUsingSprite1;
            timer = 0;
        }
    }
}
