using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyAttack : MonoBehaviour, IDamage
{
    [SerializeField] Slider slider;
    [SerializeField] EnemyData enemyData; // ScriptableObject
    [SerializeField] EnemyMovement enemyMovement; // 敵の移動スクリプト
    [SerializeField] EnemySpriteAnimation enemySpriteAnimation;
    [SerializeField] Sprite attackRightSprite; // Attackアニメーションに対応するスプライト
    [SerializeField] Sprite attackLeftSprite; // Attackアニメーションに対応するスプライト
    private SCOREandRESULT SCORE;

    private SpriteRenderer spriteRenderer;

    private int currentHP;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        enemyMovement = GetComponent<EnemyMovement>();

        currentHP = enemyData.HP;

        SCORE = SCOREandRESULT.Instance;
    }

    private void Update()
    {
        if (enemyData == null) return;
        
        AttackTargetUnit();

        if (enemyMovement.isUnit)
        {
            if (90.0f <= enemyMovement.GetAngle() && enemyMovement.GetAngle() < 270.0f)
            {
                enemySpriteAnimation.ChangeState("AttackLeft");
            }
            else
            {
                enemySpriteAnimation.ChangeState("AttackRight");
            }
        }
        else
        {
            enemySpriteAnimation.ChangeState("Move");
        }
    }

    // HPを増減
    public void Damage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, enemyData.HP);

        // DOTweenを使用してスライダーの値を滑らかに変化させる
        float targetValue = (float)currentHP / (float)enemyData.HP;
        slider.DOValue(targetValue, 0.5f)
                .OnComplete(() =>
                {
                    if (currentHP <= 0)
                    {
                        Destroy(this.gameObject);
                    }
                });
    }

    private void AttackTargetUnit()
    {
        if (enemyMovement.targetUnit != null)
        {
            Sprite a = enemySpriteAnimation.GetCurrentSprite();
            if (attackRightSprite == a || attackLeftSprite == a)
            {
                // ユニットにダメージを与える
                IDamage damage = enemyMovement.targetUnit.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.Damage(enemyData.ATK); // ダメージを与える値を設定
                }
            }
        }
    }

    void OnDestroy()
    {
        // DOTweenアニメーションをキャンセル
        DOTween.Kill(slider);

        if (SCORE != null)
        {
            SCORE.DefeatedEnemies++;
        }
    }
}
