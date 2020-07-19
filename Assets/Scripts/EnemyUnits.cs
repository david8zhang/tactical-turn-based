using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnits : MonoBehaviour
{
    [SerializeField]
    internal GameMap gameMap;

    List<GameObject> units = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnUnits();
    }

    void SpawnUnits()
    {
        // Hardcoded enemy unit starting positions
        List<int[]> unitPositions = new List<int[]>();
        unitPositions.Add(new int[] { 0, gameMap.cols - 1 });
        unitPositions.Add(new int[] { 1, gameMap.cols - 2 });
        unitPositions.Add(new int[] { 2, gameMap.cols - 1 });
        GameObject maskReference = (GameObject)Instantiate(Resources.Load("Mask"));
        for (int i = 0; i < unitPositions.Count; i++)
        {
            int[] pos = unitPositions[i];
            GameObject unitObj = gameMap.SpawnUnit(pos[0], pos[1], maskReference);
            unitObj.GetComponent<SpriteRenderer>().flipX = true;
            Unit unit = unitObj.GetComponent<Unit>();
            unit.Create(pos,"enemy " + i, unitObj);
            units.Add(unitObj);
        }
        Destroy(maskReference);
    }

    internal bool IsEnemyInSquare(int row, int col)
    {
        for (int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit.row == row && unit.col == col)
            {
                return true;
            }
        }
        return false;
    }

    internal void StartTurn()
    {
        // TODO: Do some AI logic to move the enemy units here
        Debug.Log("Enemy moving...");
        gameMap.playerUnits.StartTurn();
    }

    internal GameObject GetEnemyObjAtPosition(int row, int col)
    {
        for (int i = 0; i < units.Count; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit.row == row && unit.col == col)
            {
                return units[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
