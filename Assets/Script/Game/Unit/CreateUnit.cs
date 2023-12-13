using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CreateUnit : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; // タイルマップ
    [SerializeField] private LevelUpUnit levelUpUnit; // スクリプト
    [SerializeField] private UnitUI unitUI; // スクリプト
    [SerializeField] private List<Image> images;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void ClickCreateUnit(string Color)
    {
        if (Input.GetMouseButtonUp(0) && unitUI.GetAnimationCount(Color) > 0)
        {
            // マウスクリックした位置をワールド座標からTilemap座標に変換
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (IsMouseInImageBounds(mouseWorldPos)) { return; }
            Vector3Int tilePosition = tilemap.WorldToCell(mouseWorldPos);
            tilePosition.z = 0;

            // Tilemapからクリックした位置のタイルを取得
            TileBase tile = tilemap.GetTile(tilePosition);
            
            // タイルが存在するかどうか
            if (tile != null)
            {
                if (TileObjectManager.Instance.GetTileObject(tilePosition) == null)
                {
                    if (unitUI.GetAnimationCount(Color) > 0) unitUI.SetAnimationCount(Color);
                    while (levelUpUnit.LevelUpIfPossible(tilePosition, Color)) { }
                    
                    if (levelUpUnit.currentUnits(Color) != null) unitUI.SetBack(levelUpUnit.currentUnits(Color), Color);
                }
            }
        }
    }

    public void ClickSpuitUnit()
    {
        if (Input.GetMouseButtonUp(0) && unitUI.GetAnimationCount("ORANGE") > 0)
        {
            // マウスクリックした位置をワールド座標からTilemap座標に変換
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (IsMouseInImageBounds(mouseWorldPos)) { return; }
            Vector3Int tilePosition = tilemap.WorldToCell(mouseWorldPos);
            tilePosition.z = 0;

            // Tilemapからクリックした位置のタイルを取得
            TileBase tile = tilemap.GetTile(tilePosition);
            
            // タイルが存在するかどうか
            if (tile != null)
            {
                if (TileObjectManager.Instance.GetTileObject(tilePosition) != null)
                {
                    // 吸っているオブジェクトがない場合
                    if (levelUpUnit.SpuitUnit == null)
                    {
                        // クリックしたタイルにオブジェクトがある場合、吸い取る
                        levelUpUnit.SpuitUnit = TileObjectManager.Instance.GetTileObject(tilePosition);
                        TileObjectManager.Instance.HideObject(tilePosition);
                        unitUI.SetBack(levelUpUnit.SpuitUnit, "ORANGE");
                    }
                }
                else
                {
                    if (levelUpUnit.SpuitUnit != null)
                    {
                        if (unitUI.GetAnimationCount("ORANGE") > 0) unitUI.SetAnimationCount("ORANGE");
                        while (levelUpUnit.LevelUpIfPossible(tilePosition, "ORANGE")) { }
                        unitUI.SetBack(levelUpUnit.SpuitUnit, "ORANGE");
                    }
                }
            }
        }
    }

    // マウスクリック位置が images のいずれかの範囲内にあるかを判定するメソッド
    private bool IsMouseInImageBounds(Vector3 mouseWorldPos)
    {
        foreach (Image image in images)
        {
            RectTransform imageRect = image.rectTransform;

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect, mouseWorldPos, null, out localPoint))
            {
                Rect rect = imageRect.rect;
                if (rect.Contains(localPoint)) return true;
            }
        }
        return false;
    }
}
