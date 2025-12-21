using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    // inherits from RandomWalkDungeonGenerator as contains random walk method
        public class CorridorGenerator : RandomWalkDungeonGenerator
    {
        [SerializeField] // serialize field to expose attribute to inspector
        private int corridorLen = 15, corridorCount = 5, corridorWidth = 2; // sets the corridor length and count
        
        // overrides the original method in the parent class to create walls
        protected override void RunProceduralGeneration()
        {
            CorridorGeneration(); // runs the corridor first generation
        }

        // method used to generate the corridors
        public void CorridorGeneration()
        {
            var floorPositions = new HashSet<Vector2Int>();  // creates the floor positions
            var potentialRoomPositions = new HashSet<Vector2Int>(); // creates the potential room positions
            
            CreateCorridors(floorPositions, potentialRoomPositions); // creates the corridors
            var roomPositions = CreateRooms(potentialRoomPositions); // creates the rooms
            
            floorPositions.UnionWith(roomPositions); // creates the rooms 
            tilemapVisualizer.PaintFloorTiles(floorPositions); // paints the floor tiles
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); // creates the walls

        }

        // method used to create rooms and return a collection of room positions
        private IEnumerable<Vector2Int> CreateRooms(ICollection<Vector2Int> potentialRoomPositions)
        {
            var roomPositions = new HashSet<Vector2Int>(); // creates the room positions to return
            var roomToCreateCount = potentialRoomPositions.Count; // calculates number of rooms to create
            var roomsToCreate = potentialRoomPositions.OrderBy(_ => Guid.NewGuid()).Take(roomToCreateCount).ToList(); // sorts potential room positions in a random order and puts in a list      

            // loops through each room position
            foreach (var roomFloor in roomsToCreate.Select(roomPosition => RunRandomWalk(randomWalkParameters, roomPosition)))
            {
                roomPositions.UnionWith(roomFloor); // adds the room floor to the room positions
            }
            return roomPositions; // returns the room positions
        }

        // method used to create corridors
        private void CreateCorridors(ISet<Vector2Int> floorPositions, ISet<Vector2Int> potentialRoomPositions) // ISet used to remove duplicates
        {
            var currentPosition = startPos; // sets the current position to the start position
            potentialRoomPositions.Add(currentPosition);  // adds the first position to the potential room positions

            for (var i = 0; i < corridorCount; i++) // for loop used to generate corridor positions
            {
                var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLen, corridorWidth); // generates corridor positions
                currentPosition = corridor[^1]; // sets the current position to the last position in the path ensuring corridors are connected
                potentialRoomPositions.Add(currentPosition); // adds the current position to the potential room positions
                floorPositions.UnionWith(corridor); // adds the corridor to the floor positions
            }
        }
    }
}

