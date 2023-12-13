using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visionArea : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = transform.root.GetComponent<EnemyMovement>();
    }

    void Update()
    {
        float enemyAngle = enemyMovement.GetAngle();
        transform.rotation = Quaternion.Euler(0f, 0f, enemyAngle);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Unit"))
        {
            // ユニットを検出した場合
            enemyMovement.SetTargetUnit(other.gameObject);
        }
    }
}
