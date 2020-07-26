using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnits : UnitsManager
{
    int unitToMoveIndex = 0;
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
        Debug.Log("Starting enemy turn...");
        AttachOnFinishedDelegate();
        ProcessNextUnitTurn();
    }

    public void ProcessNextUnitTurn()
    {
        ProcessMove();
    }

    private void DetachOnFinishedDelegate()
    {
        UiManager uiManager = gameMap.uiManager.GetComponent<UiManager>();
        AttackCutscene attackCutscene = uiManager.attackCutscene.GetComponent<AttackCutscene>();
        attackCutscene.onFinishedDelegate -= ProcessNextUnitTurn;
    }

    private void AttachOnFinishedDelegate()
    {
        UiManager uiManager = gameMap.uiManager.GetComponent<UiManager>();
        AttackCutscene attackCutscene = uiManager.attackCutscene.GetComponent<AttackCutscene>();
        attackCutscene.onFinishedDelegate += ProcessNextUnitTurn;
    }

    private int ManhattanDistance(int[] start, int[] end) {
        return Math.Abs(start[0] - end[0]) + Math.Abs(start[1] - end[1]);
    }

    private void ProcessMove()
    {
        if (unitToMoveIndex < units.Count)
        {
            GameObject unitObj = units[unitToMoveIndex];
            int[] moveDes = GetClosestSquareToPlayerUnit(unitObj);
            MoveUnit(moveDes[0], moveDes[1], unitObj);
            AttackIfPossible(unitObj);
            unitToMoveIndex++;
        } else
        {
            DetachOnFinishedDelegate();
            gameMap.playerUnits.StartTurn();
            unitToMoveIndex = 0;
        }
    }

    private void AttackIfPossible(GameObject unitObj)
    {
        Unit unit = unitObj.GetComponent<Unit>();
        List<SquareWithRange> attackableSquares = GetSquaresWithinRange(unit.row, unit.col, unit.attackRange);
        List<GameObject> possibleTargets = new List<GameObject>();
        foreach (SquareWithRange sq in attackableSquares)
        {
            int[] coord = sq.coordinates;
            if (gameMap.playerUnits.IsUnitAtPosition(coord[0], coord[1]))
            {
                possibleTargets.Add(gameMap.playerUnits.GetUnitObjAtPosition(coord[0], coord[1]));
            }
        }

        if (possibleTargets.Count > 0)
        {
            // TODO: Intelligently select an enemy to attack
            Unit playerUnitToAttack = possibleTargets[0].GetComponent<Unit>();
            gameMap.uiManager.GetComponent<UiManager>().PlayAttackCutscene(unit, playerUnitToAttack);
        } else
        {
            ProcessNextUnitTurn();
        }
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
