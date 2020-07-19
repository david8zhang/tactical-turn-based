﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int row;
    public int col;
    public string unitName;
    public int moveRange;
    public int attackRange;
    public GameObject gameObj;

    public void Create(int[] pos, string name, GameObject obj)
    {
        unitName = name;
        row = pos[0];
        col = pos[1];
        attackRange = 1;
        moveRange = 6;
        gameObj = obj;
    }

    public void Move(int row, int col, GameMap gameMap)
    {
        this.row = row;
        this.col = col;
        gameMap.MoveObject(row, col, gameObject);
    }
}