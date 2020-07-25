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
    public GameObject gameObj;
    public int health;
    public int maxHealth;

    public bool isDead = false;

    public void Create(int[] pos, string name, GameObject obj)
    {
        unitName = name;
        row = pos[0];
        col = pos[1];
        attackRange = 1;
        moveRange = 3;
        gameObj = obj;
        maxHealth = 100;
        health = 100;
    }

    public void Move(int row, int col, GameMap gameMap)
    {
        this.row = row;
        this.col = col;
        gameMap.MoveObject(row, col, gameObject);
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
        Debug.Log("Show death cutscene...");
    }
}
