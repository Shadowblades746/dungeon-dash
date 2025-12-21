using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class RangedAI : MonoBehaviour
    {
        public float fullHp = 100f; // enemy health
        public float currentHp; // current health
        public int points = 10; // score value of the enemy
        public static event Action<int> OnEnemyDeath; // event to call when enemy dies
        public Transform player; // reference to the player
        public GameObject projectile; // reference to the projectile

        public float cooldownTime; // time between attacks
        private float cooldown; // cooldown time


        // Start is called before the first frame update
        void Start()
        {
            cooldown = cooldownTime; // sets the cooldown to the cooldown time
            currentHp = fullHp; // sets the current health to the full health
        }


        // Update is called once per frame
        void Update()
        {
            Vector2 direction = new Vector2(player.position.x - transform.position.x,
                player.position.y - transform.position.y); // gets the direction of the player
            transform.up = direction; // sets the direction of the enemy to the direction of the player

            if (cooldown <= 0) // checks if the cooldown is less than or equal to 0
            {
                Instantiate(projectile, transform.position, transform.rotation); // instantiates the projectile
                cooldown = cooldownTime; // resets the cooldown
            }
            else // if the cooldown is not less than or equal to 0
            {
                cooldown -= Time.deltaTime; // reduces the cooldown
            }
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
