using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitsManager : MonoBehaviour
{
    [SerializeField]
    internal GameMap gameMap;

    internal List<GameObject> units = new List<GameObject>();
    internal List<string> movedUnits = new List<string>();

    public struct SquareWithRange
    {
        public int[] coordinates;
        public int range;
    }

    internal Side side;

    public enum Side
    {
        Player,
        Enemy
    }


    internal void SetSide(Side side)
    {
        this.side = side;
    }

    internal void SpawnUnits(GameObject reference, List<int[]> unitPositions)
    {
        string namePrefix = side == Side.Player ? "player unit " : "enemy unit ";
        for (int i = 0; i < unitPositions.Count; i++)
        {
            int[] pos = unitPositions[i];
            GameObject unitObj = gameMap.SpawnUnit(pos[0], pos[1], reference);
            Unit unit = unitObj.GetComponent<Unit>();
            unit.Create(pos, namePrefix + i, unitObj);
            units.Add(unitObj);
        }
        Destroy(reference);
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

    internal void MoveUnit(int row, int col, GameObject obj)
    {
        Unit unit = obj.GetComponent<Unit>();
        unit.Move(row, col, gameMap);
        unit.GetComponent<SpriteRenderer>().color = Color.gray;
        movedUnits.Add(unit.unitName);
    }


    internal void ClearDeadUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit.isDead)
            {
                unit.Die();
                units.Remove(unit.gameObject);
                Destroy(unit.gameObject);
            }
        }
    }

    public void ResetUnitHighlight()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public List<SquareWithRange> GetSquaresWithinRange(int startX, int startY, int range)
    {
        // Do a BFS to find all positions within moveRange
        int currRange = 0;
        HashSet<string> set = new HashSet<string>();
        Queue<int[]> queue = new Queue<int[]>();
        int[][] directions = new int[][]
        {
            new int[] {0, 1 },
            new int[] {0, -1 },
            new int[] {-1, 0 },
            new int[] {1, 0 }
        };
        List<SquareWithRange> squares = new List<SquareWithRange>();
        queue.Enqueue(new int[] { startX, startY });
        while (queue.Count != 0 && currRange <= range)
        {
            int size = queue.Count;
            for (int i = 0; i < size; i++)
            {
                int[] position = queue.Dequeue();
                if (!set.Contains(position[0] + ", " + position[1]))
                {
                    set.Add(position[0] + ", " + position[1]);
                    SquareWithRange sq;
                    sq.coordinates = new int[] { position[0], position[1] };
                    sq.range = currRange;
                    squares.Add(sq);
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


    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    internal void Update()
    {

    }
}
