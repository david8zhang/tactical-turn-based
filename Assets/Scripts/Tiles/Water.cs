using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Tile
{
    internal override TileTypes GetTileType()
    {
        return TileTypes.Water;
    }

    internal override Color32 GetDefaultColor()
    {
        return new Color32(109, 145, 253, 255);
    }
}
