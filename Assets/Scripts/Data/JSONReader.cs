using System;
using UnityEngine;

public class JSONReader: MonoBehaviour
{
    public TextAsset jsonFile;
    private void Start()
    {
        UnitDataList unitDataList = JsonUtility.FromJson<UnitDataList>(jsonFile.text);
        foreach (UnitData u in unitDataList.units)
        {
            Debug.Log("Found unit: " + u.name);
        }
    }
}
