using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnits : UnitsManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Spawn player units
        SetSide(Side.Player);
        List<int[]> unitPositions = new List<int[]>
        {
            new int[] { 0, 0 },
            new int[] { 1, 1 },
            new int[] { 2, 0 }
        };
        GameObject frogReference = (GameObject)Instantiate(Resources.Load("Frog"));
        SpawnUnits(frogReference, unitPositions);
    }

    internal void FinishTurn()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
        gameMap.enemyUnits.StartTurn();
        gameMap.cursor.DisableSelection();
    }

    internal void StartTurn()
    {
        movedUnits.Clear();
    }

    internal bool HasAttackableEnemies(int row, int col)
    {
        Unit unit = GetUnitObjAtPosition(row, col).GetComponent<Unit>();
        List<int[]> squaresWithinAttackRange = GetSquaresWithinAttackRange(row, col, unit.attackRange);
        foreach (int[] coord in squaresWithinAttackRange)
        {
            if (gameMap.enemyUnits.IsUnitAtPosition(coord[0], coord[1]))
            {
                return true;
            }
        }
        return false;
    }

    internal List<int[]> GetAttackableEnemies(int row, int col)
    {
        Unit unit = GetUnitObjAtPosition(row, col).GetComponent<Unit>();
        List<int[]> squaresWithinAttackRange = GetSquaresWithinAttackRange(row, col, unit.attackRange);
        List<int[]> attackableEnemies = new List<int[]>();
        foreach (int[] coord in squaresWithinAttackRange)
        {
            if (gameMap.enemyUnits.IsUnitAtPosition(coord[0], coord[1]))
            {
                attackableEnemies.Add(coord);
            }
        }
        return attackableEnemies;
    }

    List<int[]> GetSquaresWithinAttackRange(int startX, int startY, int attackRange)
    {
        // Do a BFS to find all positions within moveRange
        int currRange = 0;
        HashSet<string> set = new HashSet<string>();
        Queue<int[]> queue = new Queue<int[]>();
        int[][] directions = new int[][]
        {
            new int[] {-1, 0 },
            new int[] {0, 1 },
            new int[] {1, 0 },
            new int[] {0, -1 }
        };
        List<int[]> squares = new List<int[]>();
        queue.Enqueue(new int[] { startX, startY });
        while (queue.Count != 0 && currRange <= attackRange)
        {
            int size = queue.Count;
            for (int i = 0; i < size; i++)
            {
                int[] position = queue.Dequeue();
                if (!set.Contains(position[0] + ", " + position[1]))
                {
                    set.Add(position[0] + ", " + position[1]);
                    squares.Add(position);
                    foreach (int[] dir in directions)
                    {
                        int newPosX = dir[0] + position[0];
                        int newPosY = dir[1] + position[1];
                        if (gameMap.CheckWithinBounds(newPosX, newPosY))
                        {
                            queue.Enqueue(new int[] { newPosX, newPosY });
                        }
                    }
                }
            }
            currRange++;
        }
        return squares;
    }

    internal bool HasUnitMoved(string name)
    {
        for (int i = 0; i < movedUnits.Count; i++)
        {
            if (movedUnits[i].Equals(name))
            {
                return true;
            }
        }
        return false;
    }

    internal bool HasAllPlayerUnitsMoved()
    {
        return movedUnits.Count == units.Count;
    }
}
