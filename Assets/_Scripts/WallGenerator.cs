using System.Collections.Generic;
using System.Linq;
using UnityEngine;
     
namespace _Scripts
{
    // class used to generate walls
    public static class WallGenerator
    {   
        // method to place walls on the tile map
        public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer) 
        {
            var wallPositions = FindWallsInDirections(floorPositions, Directions.DirectionsList); // collects the intended wall positions
            foreach (var pos in wallPositions) // loops through each position in wall positions
            {
                tilemapVisualizer.PaintSingleWall(pos); // paints the wall at the position
            }
        }
    
        // method to find where to place walls
        private static HashSet<Vector2Int> FindWallsInDirections(ICollection<Vector2Int> floorPositions, IReadOnlyCollection<Vector2Int> directionList) 
        { 
            var wallPositions = new HashSet<Vector2Int>(); // to remove duplicates
            // loops through each position in floor positions and each direction in the direction list
            foreach (var neighborPosition in floorPositions.SelectMany(_ 
                         => directionList, (pos, direction) // selects the floor positions and directions
                         => pos + direction).Where(neighborPosition // selects the neighbor position
                         => floorPositions.Contains(neighborPosition) == false)) // checks if the position is not a floor position
            {
                wallPositions.Add(neighborPosition); // adds all non floor positions to the wall positions list
            }
            return wallPositions; // returns the hash set of wall positions
        }
    }
}
