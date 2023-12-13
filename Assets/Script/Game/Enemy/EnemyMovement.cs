using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyData enemyData; // ScriptableObject

    private int currentDestinationIndex = 0;
    private Vector2 currentDestination; // x, y 座標のみ
    private bool isMoving = false;
    public GameObject targetUnit; // 検出したユニット
    private float angle = 0; // 敵の進行方向の角度
    private bool isLastDestination = false; // 最後の目的地に到達したかどうか
    public bool isUnit { get; set; }

    private void Start()
    {
        SetNextDestination();
        isUnit = false;
    }

    private void Update()
    {
        if (enemyData == null) return;

        if (isMoving)
        {
            MoveToDestination();
        }
        else if (!isMoving && targetUnit != null)
        {
            MoveToUnit();
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    private void MoveToDestination()
    {
        float step = enemyData.Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentDestination.x, currentDestination.y, transform.position.z), step);

        // 目的地に到達したら次の目的地を設定
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), currentDestination) < 0.001f)
        {
            SetNextDestination();
        }

        if (!isLastDestination)
        {
            UpdateMoveDirectionAngle();
        }
    }

    private void SetNextDestination()
    {
        if (currentDestinationIndex < AreaManager.Instance.destinationAreas.Count)
        {
            Rect currentDestinationArea = AreaManager.Instance.destinationAreas[currentDestinationIndex];
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            // もし敵が領域外にいる場合、ランダムな領域内の座標を設定
            if (!currentDestinationArea.Contains(currentPosition))
            {
                float randomX = Random.Range(currentDestinationArea.x, currentDestinationArea.x + currentDestinationArea.width);
                float randomY = Random.Range(currentDestinationArea.y, currentDestinationArea.y + currentDestinationArea.height);
                currentDestination = new Vector2(randomX, randomY);
            }
            else
            {
                // 敵が既に領域内にいる場合、最も近い領域内の座標を設定
                Vector2 closestPoint = new Vector2(
                    Mathf.Clamp(currentPosition.x, currentDestinationArea.x, currentDestinationArea.x + currentDestinationArea.width),
                    Mathf.Clamp(currentPosition.y, currentDestinationArea.y, currentDestinationArea.y + currentDestinationArea.height)
                );

                currentDestination = closestPoint;
            }

            currentDestinationIndex++;
            isMoving = true;
        }
        else
        {
            isMoving = false;
            isLastDestination = true; // 最後の目的地に到達
        }
    }

    public void SetTargetUnit(GameObject unit)
    {
        if (targetUnit == null)
        {
            targetUnit = unit;
            isMoving = false;
        }
    }

    private void MoveToUnit()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(targetUnit.transform.position.x, targetUnit.transform.position.y)) > 1.25f)
        {
            float step = enemyData.Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetUnit.transform.position, step);
            UpdateMoveDirectionAngle();
        }
        else
        {
            isUnit = true;
            CalculateAngleToTarget();
        }

        if (!targetUnit.activeSelf)
        {
            isMoving = true;
            targetUnit = null;
            isUnit = false;
        }
    }

    private void UpdateMoveDirectionAngle()
    {
        // 進行方向のベクトルを計算
        Vector2 moveDirection = currentDestination - new Vector2(transform.position.x, transform.position.y);

        // 進行方向の角度を計算
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        // 0度から360度に変換
        if (angle < 0)
        {
            angle += 360f;
        }
    }

    private float CalculateAngleToTarget()
    {
        if (targetUnit != null)
        {
            Vector2 targetDirection = new Vector2(targetUnit.transform.position.x, targetUnit.transform.position.y) - new Vector2(transform.position.x, transform.position.y);
            angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // 角度が負の場合、360度に変換
            if (angle < 0)
            {
                angle += 360f;
            }

            return angle;
        }

        // ユニットが存在しない場合、0度を返す
        return 0f;
    }

    public float GetAngle()
    {
        return angle;
    }
}
