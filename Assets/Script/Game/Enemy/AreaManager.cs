using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : SingletonMonoBehaviour<AreaManager>
{
    [SerializeField] List<GameObject> destinationGameObjects; // 目的地エリアのリスト
    [SerializeField] List<GameObject> enemySpawnGameObjects; // 敵生成エリアのリスト

    [HideInInspector] public List<Rect> destinationAreas = new List<Rect>(); // 目的地エリアの Rect 配列
    [HideInInspector] public List<Rect> enemySpawnAreas = new List<Rect>(); // 敵生成エリアの Rect 配列

    override protected void Awake()
    {
        base.Awake();

        CalculateRect(destinationGameObjects, destinationAreas);
        CalculateRect(enemySpawnGameObjects, enemySpawnAreas);
    }

    private void CalculateRect(List<GameObject> Areas, List<Rect> Rects)
    {
        foreach (GameObject Area in Areas)
        {
            // 各目的地エリアの座標とスケールから Rect を計算
            Vector3 position = Area.transform.position;
            Vector3 scale = Area.transform.localScale;
            Rect areaRect = new Rect(
                position.x - scale.x / 2,
                position.y - scale.y / 2,
                scale.x,
                scale.y
            );
            Rects.Add(areaRect);
        }
    }
}
