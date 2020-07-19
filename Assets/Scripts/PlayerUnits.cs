using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnits : MonoBehaviour
{

    [SerializeField]
    internal GameMap gameMap;

    List<GameObject> units = new List<GameObject>();
    List<string> movedUnits = new List<string>();
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnUnits();
    }

    void SpawnUnits()
    {
        // Hardcoded unit starting positions
        List<int[]> unitPositions = new List<int[]>();
        unitPositions.Add(new int[] { 0, 0 });
        unitPositions.Add(new int[] { 1, 1 });
        unitPositions.Add(new int[] { 2, 0 });

        GameObject frogReference = (GameObject)Instantiate(Resources.Load("Frog"));
        for (int i = 0; i < unitPositions.Count; i++)
        {
            int[] pos = unitPositions[i];
            GameObject unitObj = gameMap.SpawnUnit(pos[0], pos[1], frogReference);
            Unit unit = unitObj.GetComponent<Unit>();
            unit.Create(pos, "unit " + i, unitObj);
            units.Add(unitObj);
        }
        Destroy(frogReference);
    }

    internal bool IsUnitAtPosition(int row, int col)
    {
        for (int i = 0; i < units.Count; i++)
        {
            Unit u = units[i].GetComponent<Unit>();
            if (u.row == row && u.col == col)
            {
                return true;
            }
        }
        return false;
    }

    internal GameObject GetUnitObjAtPosition(int row, int col)
    {
        for (int i = 0; i < units.Count; i++)
        {
            GameObject unitObj = units[i];
            Unit u = unitObj.GetComponent<Unit>();
            if (u.row == row && u.col == col)
            {
                return unitObj;
            }
        }
        return null;
    }

    internal void MoveUnit(int row, int col, GameObject obj)
    {
        Unit unit = obj.GetComponent<Unit>();
        unit.Move(row, col, gameMap);
        unit.GetComponent<SpriteRenderer>().color = Color.gray;
        movedUnits.Add(unit.unitName);
    }

    internal void FinishTurn()
    {
        Debug.Log("Called finish turn!");
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
            if (gameMap.enemyUnits.IsEnemyInSquare(coord[0], coord[1]))
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
            if (gameMap.enemyUnits.IsEnemyInSquare(coord[0], coord[1]))
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

    // Update is called once per frame
    void Update()
    {
    }
}
