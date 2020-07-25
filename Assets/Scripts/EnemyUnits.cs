using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnits : UnitsManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        SetSide(Side.Enemy);
        List<int[]> unitPositions = new List<int[]>
        {
            new int[] { 0, gameMap.cols - 1 },
            new int[] { 1, gameMap.cols - 2 },
            new int[] { 2, gameMap.cols - 1 }
        };
        GameObject maskReference = (GameObject)Instantiate(Resources.Load("Mask"));
        SpawnUnits(maskReference, unitPositions);
        FlipUnits();
    }

    internal void FlipUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    internal void StartTurn()
    {
        // Move towards the player units
        /*
         * For each unit in units list:
         *   - Similar to player unit, calculate all moveable squares
         *   - For each moveable square return the one that is closest to a player unit
         */

        foreach (GameObject unitObj in units)
        {
            int[] moveDes = GetClosestSquareToPlayerUnit(unitObj);
            MoveUnit(moveDes[0], moveDes[1], unitObj);
        }
        gameMap.playerUnits.StartTurn();
    }

    private int ManhattanDistance(int[] start, int[] end) {
        return Math.Abs(start[0] - end[0]) + Math.Abs(start[1] - end[1]);
    }

    private int[] GetClosestSquareToPlayerUnit(GameObject unitObj)
    {
        Unit unit = unitObj.GetComponent<Unit>();
        List<SquareWithRange> moveableSquares = GetSquaresWithinRange(unit.row, unit.col, unit.moveRange);
        int[] coord = null;
        int distance = Int32.MaxValue;
        foreach (SquareWithRange sq in moveableSquares)
        {
            foreach (GameObject playerUnitObj in gameMap.playerUnits.units)
            {
                Unit playerUnit = playerUnitObj.GetComponent<Unit>();
                int[] playerCoordinates = new int[] { playerUnit.row, playerUnit.col };
                int distToPlayer = ManhattanDistance(sq.coordinates, playerCoordinates);
                if (distToPlayer < distance &&
                    !IsUnitAtPosition(sq.coordinates[0], sq.coordinates[1]) &&
                    !gameMap.playerUnits.IsUnitAtPosition(sq.coordinates[0], sq.coordinates[1]))
                {
                    coord = sq.coordinates;
                    distance = distToPlayer;
                }
            }
        }
        return coord;
    }
}
