using System.Collections.Generic;
using System.Linq;
using _Scripts.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    // creates a random walk dungeon generator class
    public class RandomWalkDungeonGenerator : AbstractDungeonGenerator // inherits from the abstract class
    {
        [SerializeField] // serialize field to expose attribute to inspector
        protected RandomWalkSo randomWalkParameters; // adds the randomWalkSO class and all the parameters
    
        // this method runs the procedural generation
        protected override void RunProceduralGeneration() // overrides the method in the abstract class
        {
            var floorPositions = RunRandomWalk(randomWalkParameters, startPos); // creates the floor positions
            tilemapVisualizer.Clear(); // clears the tiles before each new generation
            tilemapVisualizer.PaintFloorTiles(floorPositions); // paints the tiles on the positions
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); // paints the walls on the positions
        }
    
        // this method allows to start from any random position to ensure that each path will be connected to the previously created floor
        protected static HashSet<Vector2Int> RunRandomWalk(RandomWalkSo parameters, Vector2Int pos) // allows to pass in the parameters
        {
            var currentPos = pos; // sets current position to the position
            var floorPositions = new HashSet<Vector2Int>(); // floor positions
            for (var i = 0; i < parameters.iterations; i++) // iterate over 10 iterations as seen above and run the random walk algorithm
            {
                var path = ProceduralGenerationAlgorithms.RandomWalk(currentPos, parameters.walkLen); // add path created by the algorithm to the floor positions
                floorPositions.UnionWith(path); // allows to create a union -> add positions from path to floor positions without duplicates
                if (parameters.randomStartPos) // if statement used to start randomly each iteration
                    currentPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count)); // select a random position from floor positions hash set
            }
            return floorPositions; // returns the floor positions
        }
    }
}

