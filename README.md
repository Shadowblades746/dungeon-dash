# Dungeon Crawler Game

A 2D dungeon crawler game built with Unity, featuring procedural dungeon generation, AI-driven enemies, challenging boss battles, and interactive elements like potions and chests.

## Features

- **Procedural Generation**: Utilizes random walk algorithms to create unique dungeon layouts with corridors, rooms, and a variety of obstacles.
  
- **Enemy AI**: Features pathfinding using the A* algorithm, with both melee and ranged attacks, as well as ragdoll physics on death for a dynamic feel.
  
- **Boss Fight**: A special boss enemy with unique mechanics, including increased speed and an enrage mode when its health gets low.
  
- **Player Mechanics**: Includes health management, damage system, knockback effects, and potions with various buffs.
  
- **Interactive Objects**: Collectible items like chests, health potions, and strength potions that can be used to enhance the player’s abilities.
  
- **UI Elements**: Dialogue system for NPC interactions and hold-to-load mechanics to trigger the boss fight.
  
- **Audio**: Dynamic music that switches between dungeon exploration and boss battle themes for immersive gameplay.

## Installation

1. Clone the repository.

   ```bash
   pip install -r requirements.txt
   ```

2. Open the project in Unity (version 2021 or later recommended).

3. Install the **A* Pathfinding Project** via the Unity Asset Store.

4. Build and run the game.

## Usage

- Navigate through the procedurally generated dungeon, explore rooms, and defeat enemies.
  
- Collect items like health and strength potions to aid in your journey.
  
- Interact with NPCs using right-click to initiate dialogue or quests.
  
- Hold 'Z' to activate the boss fight when you're ready for a challenge.

## Scripts Overview

- **BossAI.cs**: Handles the boss's behavior, pathfinding, and attack patterns.
  
- **EnemyAI.cs**: Manages enemy AI, movement, and ragdoll death effects.
  
- **PlayerLogic.cs**: Controls player health, damage output, and potion effects.
  
- **CorridorGenerator.cs**: Generates dungeon corridors and rooms using procedural generation.
  
- **ObjectSpawner.cs**: Spawns enemies, chests, and potions at random locations on the tilemap.
  
- **ProceduralGenerationAlgorithms.cs**: Implements random walk algorithms for dungeon creation.
  
- **TilemapVisualizer.cs**: Paints the floor and wall tiles for the dungeon layout.
  
- **WallGenerator.cs**: Identifies and places walls in the dungeon based on generated corridors.

## Dependencies

- Unity 2021+
- **A* Pathfinding Project**
- **TextMeshPro**
- **Unity UGUI**

## Contributing

Feel free to fork the repository and submit pull requests for improvements. Contributions are welcome!

## License

This project is licensed under the **MIT License**.
