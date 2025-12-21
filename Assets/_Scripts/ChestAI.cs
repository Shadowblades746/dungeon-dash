using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class ChestAI : MonoBehaviour
    {
        public float fullHp = 20f; // enemy health
        public float currentHp; // current health
        public int points = 10; // score value of the enemy
        public static event Action<int> OnEnemyDeath; // event to call when enemy dies
        

        // Start is called before the first frame update
        void Start()
        {
            currentHp = fullHp; // sets the current health to the full health
        }


        // Update is called once per frame
        void Update()
        {

        }

        
        private void Collect() // method used to collect the enemy
        {
            OnEnemyDeath?.Invoke(points); // calls the on enemy death event
        }


        public void Damage(float damageTaken) // method used to take damage
        {
            currentHp -= damageTaken; // takes the damage from the current health
            if (currentHp <= 0) // checks if the current health is less than or equal to 0
            {
                GetComponent<Collider2D>().enabled = false; // disables the collider
                Collect(); // calls the collect method
                Destroy(gameObject); // destroys the enemy
                enabled = false; // disables the script
            }
        }
    }
}

