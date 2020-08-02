using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Tile
{
    internal override TileTypes GetTileType()
    {
        return TileTypes.Stone;
    }

    internal override Color32 GetDefaultColor()
    {
        return new Color32(203, 200, 200, 255);
    }
}
