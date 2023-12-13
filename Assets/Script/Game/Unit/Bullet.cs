using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 1; // 弾のダメージ
    private float bulletSpeed = 1.5f; // 弾丸の速度
    private float lifeTime = 1.0f; // 弾丸の生存時間（秒）

    private int EnemyCount = 0;
    private int EnemyInCollision = 1;

    public void Initialize(int unitATK, int enemyInCollision, float Time)
    {
        damage = unitATK;
        lifeTime = Time;
        EnemyInCollision = enemyInCollision;
    }

    private void Start()
    {
        // lifeTime秒後に自動的にBulletをDestroy
        Destroy(gameObject, lifeTime);
    }

    public float ReturnSpeed()
    {
        return bulletSpeed;
    }

    // 衝突したときに呼び出されるメソッド
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (EnemyCount >= EnemyInCollision)
        {
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            // 衝突したオブジェクトが"Enemy"タグを持つ場合、ダメージを与える
            IDamage damageable = other.gameObject.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.Damage(damage);
            }

            EnemyCount++;
        }
    }
}
