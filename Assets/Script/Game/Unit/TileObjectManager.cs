using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileObjectManager : SingletonMonoBehaviour<TileObjectManager>
{
    public Tilemap tilemap;  // タイルマップ

    // タイル座標とオブジェクトの関連を管理する
    private Dictionary<Vector3Int, GameObject> tileObjectDictionary = new Dictionary<Vector3Int, GameObject>();

    // オブジェクトの名前とオブジェクトを管理する
    private Dictionary<string, List<GameObject>> NameObjectDictionary = new Dictionary<string, List<GameObject>>();

    // タイル座標をキーとしてオブジェクトを関連付ける
    public void AddTileObject(Vector3Int tilePosition, GameObject obj)
    {
        if (!tileObjectDictionary.ContainsKey(tilePosition))
        {
            tileObjectDictionary[tilePosition] = obj;
        }
    }

    // タイル座標から関連付けられたオブジェクトを取得する
    public GameObject GetTileObject(Vector3Int tilePosition)
    {
        if (tileObjectDictionary.ContainsKey(tilePosition))
        {
            return tileObjectDictionary[tilePosition];
        }
        return null;
    }

    // タイル座標から関連付けられたオブジェクトを削除する
    public void RemoveTileObject(Vector3Int tilePosition)
    {
        if (tileObjectDictionary.ContainsKey(tilePosition))
        {
            Destroy(tileObjectDictionary[tilePosition]);
            tileObjectDictionary.Remove(tilePosition);
        }
    }

    // タイル座標から関連付けられたオブジェクトを NameObjectDictionary に移動させ、非表示にする
    public void HideObject(Vector3Int tilePosition)
    {
        if (tileObjectDictionary.ContainsKey(tilePosition))
        {
            if (!NameObjectDictionary.ContainsKey(tileObjectDictionary[tilePosition].name))
            {
                NameObjectDictionary[tileObjectDictionary[tilePosition].name] = new List<GameObject>();
            }

            NameObjectDictionary[tileObjectDictionary[tilePosition].name].Add(tileObjectDictionary[tilePosition]);
            tileObjectDictionary[tilePosition].SetActive(false);
            tileObjectDictionary.Remove(tilePosition);
        }
    }

    // タイル座標をキーとして、指定した名前のオブジェクトを NameObjectDictionary から tileObjectDictionary へ移動させる
    public bool ShowObject(Vector3Int tilePosition, string ObjectName)
    {
        if (NameObjectDictionary.ContainsKey(ObjectName))
        {
            if (NameObjectDictionary[ObjectName].Count != 0)
            {
                tileObjectDictionary[tilePosition] = NameObjectDictionary[ObjectName][0];
                tileObjectDictionary[tilePosition].SetActive(true);
                NameObjectDictionary[ObjectName].Remove(tileObjectDictionary[tilePosition]);

                return true;
            }
        }

        return false;
    }

    // オブジェクトの座標からタイル座標を取得し、HideObject を実行する
    public void HideObjectByPosition(Vector3 ObjectPosition)
    {
        // オブジェクトの座標からタイル座標に変換
        Vector3Int tilePosition = tilemap.WorldToCell(ObjectPosition);
        tilePosition.z = 0;

        // タイル座標をキーとしてオブジェクトを非表示にする
        HideObject(tilePosition);
    }

}
