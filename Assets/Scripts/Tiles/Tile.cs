using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileTypes
    {
        Ground,
        Water,
        Stone
    }

    private TileTypes tileType;

    virtual internal TileTypes GetTileType()
    {
        return tileType;
    }

    virtual internal Color32 GetDefaultColor()
    {
        return new Color32(0, 0, 0, 0);
    }

}
