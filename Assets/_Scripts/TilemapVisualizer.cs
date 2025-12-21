using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // module to use tile map

namespace _Scripts
{
    // used to put tiles on tile map depending on input positions
    public class TilemapVisualizer : MonoBehaviour
    {
        [SerializeField] // expose to inspector
        private Tilemap floorTilemap, wallTilemap; //reference to tile map
        [SerializeField] // expose to inspector
        private TileBase floorTile, wallTop; // picks which tile to paint

        // public method to paint tiles on tile map
        public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions) // IEnumerable allows to pass in any type of collection
        {
            PaintTiles(floorPositions, floorTilemap, floorTile); // paint all the tiles
        }

        // method used to paint tiles 
        private static void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
        {
            foreach (var pos in positions) // loops through the positions in the positions collection
            {
                PaintSingleTile(tilemap, tile, pos); // paint the tile on the tile map
            }
        }     
    
        // method used to paint the wall
        internal void PaintSingleWall(Vector2Int pos)
        {
            PaintSingleTile(wallTilemap, wallTop, pos); // paints the wall at the position
        }
    
        // method used to get position on the tile map
        private static void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
        { 
            var tilePos = tilemap.WorldToCell((Vector3Int)pos); // converts a world position to a tile position
            tilemap.SetTile(tilePos, tile); // sets the tile on this position
        }

        // method used to clear the tiles so algorithm produces new dungeon every time
        public void Clear()
        {
            floorTilemap.ClearAllTiles(); // clears the tiles
            wallTilemap.ClearAllTiles(); // clears the wall tiles
        }
    }
}

