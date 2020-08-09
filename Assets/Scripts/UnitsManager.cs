using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitsManager : MonoBehaviour
{
    [SerializeField]
    internal GameMap gameMap;

    internal List<GameObject> units = new List<GameObject>();
    internal List<string> movedUnits = new List<string>();
    internal List<GameObject> deadUnits = new List<GameObject>();

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

    internal void SpawnUnits(GameObject reference, UnitDataList unitDataList)
    {
        int unitDataIndex = 0;
        UnitData[] unitDataArr = unitDataList.units;
        for (int i = 0; i < gameMap.rows; i++)
        {
            for (int j = 0; j < gameMap.cols; j++)
            {
                bool spawnCondition = side == Side.Player ? j <= gameMap.cols / 2 : j > gameMap.cols / 2;
                Tile tile = gameMap.GetTileAtPosition(i, j);
                if (tile.GetTileType() != Tile.TileTypes.Stone &&
                    tile.GetTileType() != Tile.TileTypes.Water &&
                    spawnCondition && unitDataIndex < unitDataArr.Length)
                {
                    UnitData unitData = unitDataArr[unitDataIndex++];
                    CreateUnit(i, j, reference, unitData);
                }
            }
        }
        Destroy(reference);
    }

    internal void CreateUnit(int i, int j, GameObject reference, UnitData unitData)
    {
        GameObject unitObj = gameMap.SpawnUnit(i, j, reference);
        Unit unit = unitObj.GetComponent<Unit>();
        unit.CreateFromUnitData(new int[2] { i, j }, unitData);
        units.Add(unitObj);
    }

    internal GameObject GetUnitObjAtPosition(int row, int col)
    {
        for (int i = 0; i < units.Count; i++)
        {
            GameObject unitObj = units[i];
            Unit u = unitObj.GetComponent<Unit>();
            if (u.row == row && u.col == col && !u.isDead)
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
            if (u.row == row && u.col == col && !u.isDead)
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


    internal void MarkDeadUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit.isDead)
            {
                unit.Die();
                deadUnits.Add(unit.gameObject);
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
                bool isStone = gameMap.GetTileAtPosition(position[0], position[1]).GetTileType() == Tile.TileTypes.Stone;
                if (!set.Contains(position[0] + ", " + position[1]) && !isStone)
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
