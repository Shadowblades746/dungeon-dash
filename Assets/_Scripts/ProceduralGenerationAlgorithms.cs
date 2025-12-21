using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    // this class will hold the random walk algorithms
    public static class ProceduralGenerationAlgorithms
    {
        // random walk method takes in the start position and the walk length
        public static IEnumerable<Vector2Int> RandomWalk(Vector2Int startPos, int walkLen)// IEnumerable is used to remove duplicate positions
        {
            var path = new HashSet<Vector2Int> {startPos}; // adds the starting position and creates the path collection
            var previousPos = startPos; // sets the previous position to the start position
            // for loop used to allow the algorithm to walk 
            for(var i = 0; i < walkLen; i++) 
            {
                var newPos = previousPos + Directions.GetRandomDirection(); // gives a random direction for the random walk algorithm
                path.Add(newPos); // adds the new position to the path
                previousPos = newPos; // sets the previous position to the new position
            }
            return path; // returns the path
        }
        
        // select a single direction and walk through the corridor length using list to keep order of positions
        public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLen, int corridorWidth)
        {
            var corridor = new List<Vector2Int>(); // creates the corridor collection
            var direction = Directions.GetRandomDirection(); // gets the random direction
            var currentPos = startPos; // sets the current position to the start position
            corridor.Add(currentPos); // adds the current position to the corridor
            for (var i = 0; i < corridorLen; i++) // for loop used to allow the algorithm to walk 
            {
                currentPos += direction; // adds the direction to the current position
                for (var k = 0; k < corridorWidth; k++) // nested for loop to make different corridor width
                {
                    for (var j = 0; j < corridorWidth; j++) // nested for loop to make different corridor width in other direction
                    {
                        var offset = new Vector2Int(k, j); // sets the offset to the x and y positions
                        corridor.Add(currentPos + offset); // adds the corridor to the current position and the offset 
                    }
                }
            }
            return corridor; // returns the corridor
        }
    }
    

    // this class gets the random direction
    public static class Directions
    {
        // holds the list of random directions
        public static readonly List<Vector2Int> DirectionsList = new()
        {
            // sets directions
            new Vector2Int(0, 1), // north
            new Vector2Int(1, 0), // east
            new Vector2Int(0, -1), // south
            new Vector2Int(-1, 0), // west

        };
    
        // method to get the random direction
        public static Vector2Int GetRandomDirection()
        {
            var direction = DirectionsList[Random.Range(0, 3)]; // picks a random direction from 0 to 4 as four directions
            return direction; // returns the direction
            
        }
    }
}