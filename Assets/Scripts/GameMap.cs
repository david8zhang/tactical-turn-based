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
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("GroundTile"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = SpawnUnit(row, col, referenceTile);
                tileMap.Add(row + "," + col, tile);
            }
        }

        Destroy(referenceTile);
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
}
