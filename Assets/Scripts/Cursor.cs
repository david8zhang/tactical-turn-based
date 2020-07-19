using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    enum CursorState
    {
        MovingCursor,
        SelectTarget,
        SelectDest,
        EnemyMoving
    }
    struct SquareWithRange
    {
        public int[] coordinates;
        public int range;
    }

    [SerializeField]
    GameMap gameMap;

    private int cursorX = 0;
    private int cursorY = 0;
    GameObject cursor;
    GameObject highlightedUnit;

    [SerializeField]
    CursorState currState = CursorState.MovingCursor;

    List<int[]> attackableEnemies = null;
    int attackableTargetIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        PlaceCursor();
    }

    void PlaceCursor()
    {
        GameObject cursorRef = (GameObject)Instantiate(Resources.Load("Cursor"));
        cursor = gameMap.SpawnUnit(cursorX, cursorY, cursorRef);
        Destroy(cursorRef);
    }


    private void UpdateCursorPosition(int deltaX, int deltaY)
    {
        int newCursorX = cursorX + deltaX;
        int newCursorY = cursorY + deltaY;
        if (gameMap.CheckWithinBounds(newCursorX, newCursorY))
        {
            cursorX = newCursorX;
            cursorY = newCursorY;
            gameMap.MoveObject(cursorX, cursorY, cursor);
        }

    }

    void ListenMovementInput()
    {
        ListenCursorMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (MoveableUnitAtPosition())
            {
                highlightedUnit = gameMap.playerUnits.GetUnitObjAtPosition(cursorX, cursorY);
                HighlightMovablePositions();
                HighlightAttackablePositions();
                currState = CursorState.SelectDest;
            }
        }
    }

    bool MoveableUnitAtPosition()
    {
        GameObject unitObj = gameMap.playerUnits.GetUnitObjAtPosition(cursorX, cursorY);
        if (!unitObj)
        {
            return false;
        }
        return !gameMap.playerUnits.HasUnitMoved(unitObj.GetComponent<Unit>().unitName);
    }

    void ListenDestSelection()
    {
        ListenCursorMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckMovable())
            {
                ClearAllHighlightedSquares();
                MoveUnitToCursor();
                highlightedUnit = null;
                if (gameMap.playerUnits.HasAttackableEnemies(cursorX, cursorY))
                {
                    attackableEnemies = gameMap.playerUnits.GetAttackableEnemies(cursorX, cursorY);
                    attackableTargetIndex = 0;
                    HighlightAttackableTargetTile(Color.red);
                    currState = CursorState.SelectTarget;
                } else
                {
                    if (HasAllPlayerUnitsMoved())
                    {
                        gameMap.playerUnits.FinishTurn();
                    }
                    else
                    {
                        currState = CursorState.MovingCursor;
                    }
                }
            }
            else
            {
                Debug.Log("You can't move to that square!");
            }
        }
    }

    bool HasAllPlayerUnitsMoved()
    {
        return gameMap.playerUnits.HasAllPlayerUnitsMoved();
    }

    void HighlightAttackableTargetTile(Color color)
    {
        int[] coord = attackableEnemies[attackableTargetIndex];
        gameMap.ChangeTileColor(coord[0], coord[1], color);

        Unit attackUnit = gameMap.playerUnits.GetUnitObjAtPosition(cursorX, cursorY).GetComponent<Unit>();
        Unit defenderUnit = gameMap.enemyUnits.GetEnemyObjAtPosition(coord[0], coord[1]).GetComponent<Unit>();
        gameMap.uiManager.GetComponent<UiManager>().ShowAttackMenu(attackUnit, defenderUnit);
    }

    void ListenTargetSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HighlightAttackableTargetTile(Color.white);
            attackableTargetIndex = Mathf.Min(attackableTargetIndex + 1, attackableEnemies.Count - 1);
            HighlightAttackableTargetTile(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HighlightAttackableTargetTile(Color.white);
            attackableTargetIndex = Mathf.Max(attackableTargetIndex - 1, 0);
            HighlightAttackableTargetTile(Color.red);
        }
    }

    void ListenCursorMovement()
    {
        // Move the cursor around the board
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UpdateCursorPosition(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpdateCursorPosition(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdateCursorPosition(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UpdateCursorPosition(0, -1);
        }
    }

    void ListenEnemyMovement()
    {
        if (!gameMap.playerUnits.HasAllPlayerUnitsMoved())
        {
            currState = CursorState.MovingCursor;
            cursor.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    void Update()
    {
        switch (currState)
        {
            case CursorState.MovingCursor:
                ListenMovementInput();
                break;
            case CursorState.SelectDest:
                ListenDestSelection();
                break;
            case CursorState.SelectTarget:
                ListenTargetSelection();
                break;
            case CursorState.EnemyMoving:
                ListenEnemyMovement();
                break;
        }

    }

    bool CheckMovable()
    {
        if (!highlightedUnit || gameMap.playerUnits.IsUnitAtPosition(cursorX, cursorY))
        {
            return false;
        }
        Unit unit = highlightedUnit.GetComponent<Unit>();
        List<SquareWithRange> moveableSquares = GetSquaresToHighlight(unit.row, unit.col, unit.moveRange);
        for (int i = 0; i < moveableSquares.Count; i++)
        {
            int[] coordinates = moveableSquares[i].coordinates;
            if (coordinates[0] == cursorX && coordinates[1] == cursorY)
            {
                return true;
            }
        }
        return false;
    }

    void MoveUnitToCursor()
    {
        gameMap.playerUnits.MoveUnit(cursorX, cursorY, highlightedUnit);
    }

    void ClearAllHighlightedSquares()
    {
        Unit unit = highlightedUnit.GetComponent<Unit>();
        int moveAndAttackRange = unit.moveRange + unit.attackRange;
        List<SquareWithRange> squaresToHighlight = GetSquaresToHighlight(unit.row, unit.col, moveAndAttackRange);
        foreach (SquareWithRange s in squaresToHighlight)
        {
            int[] coord = s.coordinates;
            gameMap.ChangeTileColor(coord[0], coord[1], Color.white);
        }
    }

    internal void ChangeCursorColor(Color color)
    {
        gameMap.ChangeTileColor(cursorX, cursorY, color);
    }

    void HighlightMovablePositions()
    {
        GameObject unit = gameMap.playerUnits.GetUnitObjAtPosition(cursorX, cursorY);
        int moveRange = unit.GetComponent<Unit>().moveRange;
        List<SquareWithRange> squaresToHighlight = GetSquaresToHighlight(cursorX, cursorY, moveRange);
        foreach (SquareWithRange s in squaresToHighlight)
        {
            int[] coord = s.coordinates;
            gameMap.ChangeTileColor(coord[0], coord[1], Color.blue);
        }
    }

    List<SquareWithRange> GetSquaresToHighlight(int startX, int startY, int range)
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

    void HighlightAttackablePositions()
    {
        GameObject unitObj = gameMap.playerUnits.GetUnitObjAtPosition(cursorX, cursorY);
        Unit unit = unitObj.GetComponent<Unit>();
        int totalAttackRange = unit.moveRange + unit.attackRange;
        List<SquareWithRange> squares = GetSquaresToHighlight(cursorX, cursorY, totalAttackRange);
        foreach (SquareWithRange s in squares) {
            if (s.range > unit.moveRange && s.range <= totalAttackRange)
            {
                int[] coord = s.coordinates;
                gameMap.ChangeTileColor(coord[0], coord[1], Color.red);
            }
        }
    }

    public void FinishAttackOrWait()
    {
        if (attackableEnemies.Count > 0)
        {
            int[] coord = attackableEnemies[attackableTargetIndex];
            gameMap.ChangeTileColor(coord[0], coord[1], Color.white);
        }
        currState = CursorState.MovingCursor;
    }

    internal void DisableSelection()
    {
        cursor.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        currState = CursorState.EnemyMoving;
    }

}
