using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        private enum ObjectType // enum to store which objects to spawn
        {
            Enemy, // enemy object type
            RangedEnemy, // ranged enemy object type
            Chest, // chest object type
            HealthPotion, // health potion object type
            StrengthPotion // strength potion object type
        }
        public Tilemap tilemap; // reference to the tile map
        public float enemy = 0.3f; // chance to spawn an enemy
        public float rangedEnemy = 0.3f; // chance to spawn a ranged enemy
        public float chest = 0.2f; // chance to spawn a chest
        public float healthPotion = 0.1f; // chance to spawn a health potion
        public float strengthPotion = 0.1f; // chance to spawn a strength potion
        public int maxObjects = 20; // maximum number of objects to spawn
        public GameObject[] objectPrefabs; // array of game objects to spawn
        
        private List<Vector3Int> validObjectPositions = new(); // list to store the object positions
        private List<GameObject> spawnedObjects = new(); // list to store the spawned objects
        private bool hasSpawned = false; // flag to check if the object has spawned
        
        
        // Start is called before the first frame update
        void Start()
        {
            var corridorGenerator = GameObject.Find("CorridorFirstDungeonGenerator"); // finds the corridor generator game object
            corridorGenerator.GameObject().GetComponent<CorridorGenerator>().CorridorGeneration(); 
            AstarPath.active.Scan(); // scans the path
            GetValidSpawnPositions(); // gets the valid spawn positions
            SpawnObjects(); // spawns the objects
            GameObject.Find("Enemy").SetActive(false); // destroys the enemy game object
            GameObject.Find("RangedEnemy").SetActive(false); // destroys the ranged enemy game object
            GameObject.Find("Chest").SetActive(false); // destroys the chest game object
            GameObject.Find("HealthPotion").SetActive(false); // destroys the health potion game object
            GameObject.Find("StrengthPotion").SetActive(false); // destroys the strength potion game object
            GameObject.Find("Boss").SetActive(false); // sets the load canvas to inactive
        }

        
        // Update is called once per frame
        void Update()
        {
        
        }
        
        
        private ObjectType GetRandomObject() // method to get a random object
        {
            var random = Random.Range(0f, 1f); // gets a random number between 0 and 1
            if (random < enemy) // checks if the random number is less than the enemy chance
            {
                return ObjectType.Enemy; // returns the enemy object type
            }
            else if (random <= enemy + rangedEnemy)
            {
                return ObjectType.RangedEnemy; // returns the ranged enemy object type
            }
            else if (random <= enemy + rangedEnemy + chest)
            {
                return ObjectType.Chest; // returns the chest object type
            }
            else if (random <= enemy + rangedEnemy + chest + healthPotion)
            {
                return ObjectType.HealthPotion; // returns the health potion object type
            }
            else if (random <= enemy + rangedEnemy + chest + healthPotion + strengthPotion)
            {
                return ObjectType.StrengthPotion; // returns the strength potion object type
            }
            return ObjectType.Enemy; // returns the enemy object type
        }

        
        private void GetValidSpawnPositions() // method to get the valid spawn positions
        {
            validObjectPositions.Clear(); // clears the list of valid object positions
            foreach (var pos in tilemap.cellBounds.allPositionsWithin) // loops through all the positions in the tile map
            {
                var localPlace = new Vector3Int(pos.x, pos.y, pos.z); // gets the local position of the tile
                if (tilemap.HasTile(localPlace)) // checks if the tile map has a tile at this position
                {
                    validObjectPositions.Add(localPlace); // adds the local place to the valid object positions list
                }
            }
        }

        
        private void SpawnObjects() // method to spawn the objects
        {
            if (hasSpawned) // checks if the object has spawned
            {
                return; // returns
            }
            hasSpawned = true; // sets the flag to true
            for (var i = 0; i < maxObjects; i++) // loops through the maximum number of objects
            {
                var spawnIndex = Random.Range(0, validObjectPositions.Count); // gets a random index from the valid object positions list
                var spawnPosition = validObjectPositions[spawnIndex]; // gets the spawn position from the valid object positions list
                var objectType = GetRandomObject(); // gets a random object type
                var objectToSpawn = objectPrefabs[(int)objectType]; // gets a random object to spawn from the object prefabs array
                var spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity); // instantiates the object to spawn at the spawn position
                spawnedObjects.Add(spawnedObject); // adds the spawned object to the spawned objects list
            }
        }
    }
}

