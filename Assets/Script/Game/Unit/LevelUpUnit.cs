using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelUpUnit : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; // タイルマップ
    [SerializeField] private UnitUI unitUI; // スクリプト
    [SerializeField] private List<GameObject> GreenBackPrefabs; // GreenBack のPrefabを格納
    [SerializeField] private List<GameObject> BlueBackPrefabs; // BlueBack のPrefabを格納
    private Vector3Int[] E_hexOffsets; // タイル座標用
    private Vector3Int[] O_hexOffsets; // タイル座標用
    private Dictionary<Vector3Int, bool> exploredTiles = new Dictionary<Vector3Int, bool>(); // 探索済みのタイルを記録
    private List<Vector3Int> TilePositionList = new List<Vector3Int>(); // 同じオブジェクトのタイル座標を保存

    private Dictionary<string, List<GameObject>> unitPrefabs = new Dictionary<string, List<GameObject>>(); // GreenBack と BlueBack をまとめて管理
    private Dictionary<string, GameObject> currentUnit = new Dictionary<string, GameObject>(); // 現在 Unit

    private int composite = 4; // レベルアップに必要なUnit数

    // currentUnit["ORANGE"] のセッター、ゲッター
    public GameObject SpuitUnit
    {
        get { return currentUnit["ORANGE"]; }
        set { currentUnit["ORANGE"] = value; }
    }

    public GameObject currentUnits(string Color)
    {
        if (Color == "GREEN") return currentUnit["GREEN"];
        else if (Color == "BLUE") return currentUnit["BLUE"];
        return null;
    }

    void Start()
    {
        unitPrefabs["GREEN"] = new List<GameObject>();
        unitPrefabs["BLUE"] = new List<GameObject>();
        unitPrefabs["ORANGE"] = new List<GameObject>();

        for (int i = 0; i < GreenBackPrefabs.Count; i++)
        {
            unitPrefabs["GREEN"].Add(GreenBackPrefabs[i]);
        }
        for (int i = 0; i < BlueBackPrefabs.Count; i++)
        {
            unitPrefabs["BLUE"].Add(BlueBackPrefabs[i]);
        }

        currentUnit["GREEN"] = unitPrefabs["GREEN"][Random.Range(0, 2)];
        currentUnit["BLUE"] = unitPrefabs["BLUE"][Random.Range(0, 2)];
        SpuitUnit = null;

        unitUI.SetBack(currentUnit["GREEN"], "GREEN");
        unitUI.SetBack(currentUnit["BLUE"], "BLUE");

        // 六角形のタイル形状における隣接するタイルの相対座標を設定
        // 偶数
        E_hexOffsets = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),    // 上
            new Vector3Int(0, 1, 0),    // 右上
            new Vector3Int(-1, 1, 0),   // 右下
            new Vector3Int(-1, 0, 0),   // 下
            new Vector3Int(-1, -1, 0),  // 左下
            new Vector3Int(0, -1, 0)    // 左上
        };
        // 奇数
        O_hexOffsets = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),    // 上
            new Vector3Int(1, 1, 0),    // 右上
            new Vector3Int(0, 1, 0),    // 右下
            new Vector3Int(-1, 0, 0),   // 下
            new Vector3Int(0, -1, 0),   // 左下
            new Vector3Int(1, -1, 0)    // 左上
        };
    }

    public int GetUnitLevel(GameObject unitObject)
    {
        // オブジェクト名からレベル情報を抽出
        string objectName = unitObject.name;
        int level;

        // オブジェクト名から数字部分を抽出し、intに変換
        string levelStr = System.Text.RegularExpressions.Regex.Replace(objectName, @"\D", "");
        if (int.TryParse(levelStr, out level))
        {
            return level - 1; // 返すのは currentLevel に対応した値
        }

        // レベル情報が見つからない場合、-1 などエラー値を返す
        return -1;
    }

    public bool LevelUpIfPossible(Vector3Int tilePosition, string Color)
    {
        string orange = "ORANGE";
        if (Color == "ORANGE")
        {
            for (int i = 0; i < unitPrefabs["GREEN"].Count; i++)
            {
                if (unitPrefabs["GREEN"][i].name == SpuitUnit.name)
                {
                    unitPrefabs["ORANGE"] = new List<GameObject>(unitPrefabs["GREEN"]);
                    orange = "GREEN";
                    break;
                }
            }

            if (unitPrefabs["ORANGE"].Count == 0)
            {
                for (int i = 0; i < unitPrefabs["BLUE"].Count; i++)
                {
                    if (unitPrefabs["BLUE"][i].name == SpuitUnit.name)
                    {
                        unitPrefabs["ORANGE"] = new List<GameObject>(unitPrefabs["BLUE"]);
                        orange = "BLUE";
                        break;
                    }
                }
            }
        }

        // オブジェクトを生成
        Vector3 tileWorldPos = tilemap.GetCellCenterWorld(tilePosition); // タイル座標をワールド座標に変換
        if (GetUnitLevel(currentUnit[Color]) < 2) tileWorldPos.y += 0.2f; // オブジェクトのy座標を違和感のない数値にする
        else if (GetUnitLevel(currentUnit[Color]) < 4) tileWorldPos.y += 0.4f;
        else tileWorldPos.y += 0.6f;
        tileWorldPos.z = tileWorldPos.y; // 手前のオブジェクトが前に描画されるようにする
        GameObject newObject = null;

        // オブジェクトの生成
        if (TileObjectManager.Instance.ShowObject(tilePosition, currentUnit[Color].name))
        {
            newObject = TileObjectManager.Instance.GetTileObject(tilePosition);
            newObject.transform.position = tileWorldPos;
        }
        else
        {
            newObject = Instantiate(currentUnit[Color], tileWorldPos, Quaternion.identity);
            newObject.gameObject.name = currentUnit[Color].name;

            // 生成したオブジェクトをタイル座標と関連付け
            TileObjectManager.Instance.AddTileObject(tilePosition, newObject);
        }

        // 隣接しているタイルの中で同じ名前のオブジェクトが合計いくつあるか調べる
        CountAdjacent(tilePosition, newObject.name);

        // 次のレベルのオブジェクトが存在し、クリックした位置に同じ名前のオブジェクトが3つ以上ある場合
        if (GetUnitLevel(currentUnit[Color]) < unitPrefabs[Color].Count - 1 && TilePositionList.Count >= composite)
        {
            for (int i = 0; i < TilePositionList.Count; i++)
            {
                TileObjectManager.Instance.HideObject(TilePositionList[i]);
            }

            // 生成するレベルを上げる
            currentUnit[Color] = unitPrefabs[Color][GetUnitLevel(currentUnit[Color]) + 1];

            if (Color == "GREEN" || orange == "GREEN")
            {
                if (GetUnitLevel(currentUnit[Color]) == 4)
                {
                    SCOREandRESULT.Instance.CreateGreenLevelFive++;
                }
                else if (GetUnitLevel(currentUnit[Color]) == 3)
                {
                    SCOREandRESULT.Instance.CreateGreenLevelFour++;
                }
                else if (GetUnitLevel(currentUnit[Color]) == 2)
                {
                    SCOREandRESULT.Instance.CreateGreenLevelthree++;
                }
            }
            if (Color == "BLUE" || orange == "BLUE")
            {
                if (GetUnitLevel(currentUnit[Color]) == 4)
                {
                    SCOREandRESULT.Instance.CreateBlueLevelFive++;
                }
                else if (GetUnitLevel(currentUnit[Color]) == 3)
                {
                    SCOREandRESULT.Instance.CreateBlueLevelFour++;
                }
                else if (GetUnitLevel(currentUnit[Color]) == 2)
                {
                    SCOREandRESULT.Instance.CreateBlueLevelthree++;
                }
            }

            // レベルが上がらなくなるまで続ける
            return true;
        }

        // レベルアップできない場合
        if (Color == "ORANGE")
        {
            SpuitUnit = null;
            unitPrefabs["ORANGE"].Clear();
        }
        else currentUnit[Color] = unitPrefabs[Color][Random.Range(0, 2)];
        return false;
    }

    // RecursiveCount の結果を返す
    private int CountAdjacent(Vector3Int tilePosition, string objectName)
    {
        exploredTiles.Clear();
        TilePositionList.Clear();
        int count = 1;
        RecursiveCount(tilePosition, objectName, ref count);
        return count;
    }

    // 再帰的に隣接するタイルに同じ名前のオブジェクトがいくつあるか調べる
    private void RecursiveCount(Vector3Int tilePosition, string objectName, ref int count)
    {
        exploredTiles[tilePosition] = true; // 自身のタイルを true にする
        TilePositionList.Add(tilePosition); // タイル座標を保存

        // タイルのy座標が偶数の場合
        if (tilePosition.y % 2 == 0)
        {
            // 現在のタイルに隣接する6つのタイルを調べる
            foreach (Vector3Int offset in E_hexOffsets)
            {
                Vector3Int neighborTilePosition = tilePosition + offset; // 相対的な座標

                if (!exploredTiles.ContainsKey(neighborTilePosition)) // 既に調べた同名オブジェクトが乗っているタイル
                {
                    GameObject neighborObject = TileObjectManager.Instance.GetTileObject(neighborTilePosition);

                    if (neighborObject != null && neighborObject.name == objectName)
                    {
                        count++;
                        RecursiveCount(neighborTilePosition, objectName, ref count);
                    }
                }
            }
        }
        // タイルのy座標が奇数の場合
        else
        {
            // 現在のタイルに隣接する6つのタイルを調べる
            foreach (Vector3Int offset in O_hexOffsets)
            {
                Vector3Int neighborTilePosition = tilePosition + offset; // 相対的な座標

                if (!exploredTiles.ContainsKey(neighborTilePosition)) // 既に調べた同名オブジェクトが乗っているタイル
                {
                    GameObject neighborObject = TileObjectManager.Instance.GetTileObject(neighborTilePosition);

                    if (neighborObject != null && neighborObject.name == objectName)
                    {
                        count++;
                        RecursiveCount(neighborTilePosition, objectName, ref count);
                    }
                }
            }
        }
    }
}
