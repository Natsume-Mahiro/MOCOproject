using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs; // 生成する敵のプレハブ

    [Tooltip("Xにゲーム開始からの秒数(float)、Yに生成するエリア(int, 0スタート)、Zに生成する敵の数(int)、Wに生成する敵の種類を入力してください(int)。\n敵の種類はenemyPrefabsの順番です")]
    [SerializeField] List<Vector4> SpawnEnemy = new List<Vector4>();

    void Start()
    {
        for (int i = 0; i < SpawnEnemy.Count; i++)
        {
            SCOREandRESULT.Instance.TotalEnemies += (int)SpawnEnemy[i].z;
            StartCoroutine(SpawnEnemiesDelayed(SpawnEnemy[i].x, (int)SpawnEnemy[i].y, (int)SpawnEnemy[i].z, enemyPrefabs[(int)SpawnEnemy[i].w]));
        }
    }

    IEnumerator SpawnEnemiesDelayed(float delay, int Area, int numberOfEnemies, GameObject enemy)
    {
        yield return new WaitForSeconds(delay);

        SpawnEnemies(numberOfEnemies, Area, enemy); // 引数を指定してメソッドを呼び出す
    }

    void SpawnEnemies(int numberOfEnemies, int Area, GameObject enemy)
    {
        if (AreaManager.Instance.enemySpawnAreas.Count == 0) { return; }
        if (enemyPrefabs.Count == 0) { return; }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Rect spawnArea = AreaManager.Instance.enemySpawnAreas[Area];

            Vector3 randomSpawnPosition = new Vector3(
                Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax),
                0.0f
            );

            // ランダムな座標に敵を生成
            Instantiate(enemy, randomSpawnPosition, Quaternion.identity);
        }
    }
}
