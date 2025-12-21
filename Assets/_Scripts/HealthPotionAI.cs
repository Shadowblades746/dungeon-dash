using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class HealthPotionAI : MonoBehaviour
    {
        public float health = 10f; // health value of the player
        public int points = 10; // score value of the enemy
        public static event Action<int> OnEnemyDeath; // event to call when enemy dies
        

        // Start is called before the first frame update
        void Start()
        {
            
        }


        // Update is called once per frame
        void Update()
        {

            
        }

        
        private void Collect() // method used to collect the enemy
        {
            OnEnemyDeath?.Invoke(points); // calls the on enemy death event
        }

        
        private void OnCollisionEnter2D(Collision2D other) // method used to check for collisions
        {
            if (other.gameObject.tag == "Player") // checks if the other object has the player tag
            {
                other.gameObject.GetComponent<PlayerLogic>().HealthPotion(); // heals the player
                Collect(); // calls the collect method
                Destroy(gameObject); // destroys the projectile
            }
        }
    }
}
