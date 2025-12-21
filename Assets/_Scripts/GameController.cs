using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using _Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        private int progress = 0; // progress of the game
        private int newProgress = 0; // progress of the game
        public Slider progressBar; // reference to the progress bar
        public TMP_Text progressText; // reference to the game over text
        public TMP_Text gameOverText; // reference to the game over text
        public GameObject loadCanvas; // reference to the load canvas
        public GameObject gameOverScreen; // reference to the game over screen
        public GameObject enemy; // reference to the enemy
        public GameObject rangedEnemy; // reference to the ranged enemy
        public GameObject chest; // reference to the chest
        public GameObject healthPotion; // reference to the health potion
        public GameObject strengthPotion; // reference to the strength potion
        public GameObject boss; // reference to the boss
        public List<GameObject> levels; // list of levels
        private int currentLevelIndex = 0; // current level index
        private GameObject walls; // reference to the player
        private Transform player; // reference to the player
        

        // Start is called before the first frame update
        void Start()
        {
            progress = 0; // sets the progress to 0
            progressBar.value = 0; // sets the progress bar value to 0
            EnemyAI.OnEnemyDeath += IncreaseProgress; // subscribes to the on enemy death event
            BossAI.OnEnemyDeath += IncreaseProgress; // subscribes to the on boss death event
            RangedAI.OnEnemyDeath += IncreaseProgress; // subscribes to the on enemy death event
            ChestAI.OnEnemyDeath += IncreaseProgress; // subscribes to the on enemy death event
            HealthPotionAI.OnEnemyDeath += IncreaseProgress; // subscribes to the on enemy death event
            StrengthPotionAI.OnEnemyDeath += IncreaseProgress; // subscribes to the on enemy death event
            PlayerLogic.OnPlayerDeath += GameOver; // subscribes to the on player death event
            BossAI.OnBossDeath += GameComplete; // subscribes to the on boss death event
            HoldToLoad.onHoldComplete += LoadBossFight; // subscribes to the on hold complete event
            loadCanvas.SetActive(false); // sets the load canvas to inactive
        }

        
        void IncreaseProgress(int amount) // method to increase the progress
        {
            progress += amount; // increases the progress by the amount
            progressBar.value = progress; // sets the progress bar value to the progress
            if (progress >= 100) // checks if the progress is greater than or equal to 10
            {
                loadCanvas.SetActive(true); // sets the load canvas to active
            }
        }


        void LoadBossFight() // method to load the boss fight
        {
            var nextlevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
            loadCanvas.SetActive(false); // sets the load canvas to inactive
            levels[currentLevelIndex].gameObject.SetActive(false); // sets the current level to inactive
            levels[nextlevelIndex].gameObject.SetActive(true); // sets the next level to active

            boss.transform.position = new Vector3(-17, 7, 0); // sets the boss position to the start
            AstarPath.active.Scan(); // scans the path
            currentLevelIndex = nextlevelIndex; // sets the current level index to the next level index
            newProgress = progress; // sets the new progress to the progress
            Debug.Log(newProgress);
            progress = 0; // sets the progress to 0
            progressBar.value = 0; // sets the progress bar value to 0
        }
        
        
        void GameOver() // method to load the game over screen
        {
            gameOverScreen.SetActive(true); // sets the game over screen to active
            gameOverText.text = "You lose"; // sets the game over text to you lose
            progressText.text = "EXP: " + progress; // sets the progress text to you lose
        }

        
        void GameComplete()
        {
            gameOverScreen.SetActive(true); // sets the game over screen to active
            gameOverText.text = "You win"; // sets the game over text to you win
            progressText.text = "EXP: " + newProgress; // sets the progress text to you win
        }
        
        
        public void RestartGame() // method to restart the game
        { 
            enemy.SetActive(true); // destroys the enemy game object
            rangedEnemy.SetActive(true); // destroys the ranged enemy game object
            chest.SetActive(true); // destroys the chest game object
            healthPotion.SetActive(true); // destroys the health potion game object
            strengthPotion.SetActive(true); // destroys the strength potion game object
            boss.SetActive(true); // sets the load canvas to inactive
            SceneManager.LoadScene("Dungeon"); // loads the current scene
        }
        
        
        public void QuitGame() // method to quit the game
        {
            Application.Quit(); // quits the application
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

    
