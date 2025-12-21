using UnityEngine;

namespace _Scripts
{
    // Abstract class used to create a custom editor for this class and child classes
    public abstract class AbstractDungeonGenerator : MonoBehaviour 
    {
        [SerializeField] // used to expose attribute to inspector
        protected TilemapVisualizer tilemapVisualizer; // reference to the tilemap visualizer class
        [SerializeField] // used to expose attribute to inspector
        protected Vector2Int startPos = Vector2Int.zero; // start position at (0,0) coordinates in shorthand

        // method to clear tile map and generate dungeon
        public void GenerateDungeon()
        {
            tilemapVisualizer.Clear(); // ensures i don't have to call the tile map clear method in all child classes
            RunProceduralGeneration(); // runs the procedural generation
        }
    
        // method allows to generate a tile map with any algorithm in any child class
        protected abstract void RunProceduralGeneration();
    }
}