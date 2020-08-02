using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Tile
{
    internal override TileTypes GetTileType()
    {
        return TileTypes.Ground;
    }


    internal override Color32 GetDefaultColor()
    {
        return new Color32(147, 248, 111, 255);
    }

}
