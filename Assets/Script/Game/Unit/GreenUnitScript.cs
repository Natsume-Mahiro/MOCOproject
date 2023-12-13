using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GreenUnitScript : MonoBehaviour, IDamage
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject bulletPrefab; // 弾丸のプレハブ
    [SerializeField] UnitSpriteAnimation spriteAnimation;
    [SerializeField] Sprite attackSprite; // Attackアニメーションに対応するスプライト

    private int EnemyInCollision = 1;
    private float lifeTime = 1.0f;

    private List<GameObject> detectedObjects = new List<GameObject>();

    private SpriteRenderer spriteRenderer;

    private int LV;
    private int ATK;
    private int maxHP;
    private int currentHP;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // オブジェクト名からレベル情報を抽出
        string objectName = this.gameObject.name;
        int level;
        // オブジェクト名から数字部分を抽出し、intに変換
        string levelStr = System.Text.RegularExpressions.Regex.Replace(objectName, @"\D", "");
        if (int.TryParse(levelStr, out level))
        {
            int Lv = 1;
            for (int i = 0; i < (level - 1); i++)
            {
                Lv *= 2;
            }

            LV = level;
            maxHP = PlayerData.Instance.GreenHitPoint * Lv;
            ATK = PlayerData.Instance.GreenAttack * Lv;
        }
        else
        {
            maxHP = PlayerData.Instance.GreenHitPoint;
            ATK = PlayerData.Instance.GreenAttack;
        }

        InitializeStats();

        if (LV == 5)
        {
            EnemyInCollision = 100;
            lifeTime = 20.0f;
        }
        else
        {
            EnemyInCollision = 1;
            lifeTime = 0.5f;
        }
    }

    private void InitializeStats()
    {
        currentHP = maxHP;
        slider.value = slider.maxValue;
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
                    // currentHP が0以下になった場合、HideObjectByPosition を呼び出す
                    if (currentHP <= 0)
                    {
                        // オブジェクトの座標からタイル座標を取得して非表示にする
                        Vector3 objectPosition = transform.position;
                        TileObjectManager.Instance.HideObjectByPosition(objectPosition);
                    }
                });
    }

    void OnEnable()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlaySE(1);
        InitializeStats();
    }

    void Update()
    {
        // リスト内のオブジェクトを処理
        for (int i = 0; i < detectedObjects.Count; i++)
        {
            GameObject obj = detectedObjects[i];
            if (obj == null)
            {
                // Destroyされたオブジェクトを検出し、リストから削除
                detectedObjects.RemoveAt(i);
            }
        }

        if (detectedObjects.Count == 0)
        {
            spriteAnimation.ChangeState("Stay");
        }
        else
        {
            spriteAnimation.ChangeState("Attack");
            
            Vector3 enemyPosition = detectedObjects[0].transform.position;
            Vector2 fireDirection = (enemyPosition - transform.position).normalized;

            // スプライトを反転させる条件を確認
            if (fireDirection.x < 0)
            {
                // スプライトを反転させる
                spriteRenderer.flipX = true;
            }
            else
            {
                // スプライトの反転を元に戻す
                spriteRenderer.flipX = false;
            }

            FireAtEnemy(detectedObjects[0]);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!detectedObjects.Contains(other.gameObject))
        {
            detectedObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (detectedObjects.Contains(other.gameObject))
        {
            detectedObjects.Remove(other.gameObject);
        }
    }
    
    void FireAtEnemy(GameObject enemy)
    {
        if (enemy != null && attackSprite == spriteAnimation.GetCurrentSprite())
        {
            // 弾丸を生成
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // 弾丸の初期化
            Bullet Bullet = bullet.GetComponent<Bullet>();
            Bullet.Initialize(ATK, EnemyInCollision, lifeTime);

            // 敵の方向に発射
            Vector2 fireDirection = (enemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            rb.rotation = angle;

            // 弾丸の生成位置を調整
            Vector2 offset = fireDirection * 0.5f;
            bullet.transform.position += new Vector3(offset.x, offset.y, 0);

            // 弾丸の速度を設定
            rb.velocity = fireDirection * Bullet.ReturnSpeed();
        }
    }

    void OnDestroy()
    {
        // DOTweenアニメーションをキャンセル
        DOTween.Kill(transform);
        DOTween.Kill(slider);
    }
}
