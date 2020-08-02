using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int row;
    public int col;
    public string unitName;
    public int moveRange;
    public int attackRange;
    public int health;
    public int maxHealth;

    // Unit Stats
    public int attack;
    public int defense;

    public bool isDead = false;
    int maxMoveRange = 3;

    public void Create(int[] pos, string name)
    {
        unitName = name;
        row = pos[0];
        col = pos[1];
        attackRange = 1;
        moveRange = maxMoveRange;
        maxHealth = 100;
        health = 100;

        attack = 50;
        defense = 25;
    }

    public void Move(int row, int col, GameMap gameMap)
    {
        this.row = row;
        this.col = col;
        if (gameMap.GetTileAtPosition(row, col).GetTileType() == Tile.TileTypes.Water)
        {
            this.moveRange = maxMoveRange - 1;
        } else
        {
            this.moveRange = maxMoveRange;
        }
        gameMap.MoveObject(row, col, gameObject);
    }

    public void CalculateDamage(Unit other)
    {

    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            isDead = true;
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Show death cutscene...");
    }
}
