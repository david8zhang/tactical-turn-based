using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{

    [SerializeField]
    internal int rows = 5;

    [SerializeField]
    internal int cols = 10;

    [SerializeField]
    private float tileSize = 1;
    internal Dictionary<string, GameObject> tileMap = new Dictionary<string, GameObject>();

    [SerializeField]
    internal PlayerUnits playerUnits;

    [SerializeField]
    internal EnemyUnits enemyUnits;

    [SerializeField]
    internal Cursor cursor;

    [SerializeField]
    internal GameObject uiManager;

    [SerializeField]
    internal Camera mainCamera;

    // Tile References
    [SerializeField]
    internal GameObject stoneTile;

    [SerializeField]
    internal GameObject waterTile;

    [SerializeField]
    internal GameObject groundTile;

    private void Awake()
    {
        CenterCamera();
        GenerateGrid();
    }

    void CenterCamera()
    {
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        mainCamera.transform.position = new Vector3(gridW / 2 - tileSize / 2, -gridH / 2 + tileSize / 2, -1);
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GenerateTile(row, col);
                
            }
        }
    }

    internal void GenerateTile(int row, int col)
    {
        System.Random rnd = new System.Random();
        GameObject refTile = null;
        int roll = rnd.Next(1, 10);

        // 20% chance to generate stone tile
        if (roll <= 2)
        {
            refTile = stoneTile;
        }
        // 20% chance to generate water tile
        else if (roll > 2 && roll <= 4)
        {
            refTile = waterTile;
        }
        else if (roll > 2)
        {
            refTile = groundTile;
        }
        GameObject tile = SpawnUnit(row, col, refTile);
        tileMap.Add(row + "," + col, tile);
    }

    internal void OnCutsceneFinished()
    {
        // Remove all dead units
        playerUnits.MarkDeadUnits();
        enemyUnits.MarkDeadUnits();
    }

    internal void CheckTurnFinished()
    {
        cursor.CheckTurnFinished();
    }

    internal bool CheckWithinBounds(int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }

    internal void ChangeTileColor(int row, int col, Color color)
    {
        string key = row + "," + col;
        GameObject cursorTile = tileMap[key];
        cursorTile.GetComponent<SpriteRenderer>().color = color;
    }

    internal void ResetTileColor(int row, int col)
    {
        Tile tile = GetTileAtPosition(row, col);
        ChangeTileColor(row, col, tile.GetDefaultColor());
    }

    internal GameObject SpawnUnit(int row, int col, GameObject reference)
    {
        GameObject newObj = (GameObject)Instantiate(reference, transform);
        float posX = col * tileSize;
        float posY = row * -tileSize;
        newObj.transform.position = new Vector2(posX, posY);
        return newObj;
    }

    internal void MoveObject(int row, int col, GameObject obj)
    {
        float posX = col * tileSize;
        float posY = row * -tileSize;
        obj.transform.position = new Vector2(posX, posY);
    }

    internal Vector2 GetPositionOnGrid(int row, int col)
    {
        float posX = col * tileSize;
        float posY = row * -tileSize;
        return new Vector2(posX, posY);
    }

    internal Tile GetTileAtPosition(int row, int col)
    {
        string key = row + "," + col;
        GameObject tileObj = tileMap[key];
        return tileObj.GetComponent<Tile>();
    }
}
