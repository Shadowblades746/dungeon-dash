using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class ProjectileLogic : MonoBehaviour
    {
        public float speed;
        public float damage;

        
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Destroy()); // starts the destroy coroutine
        }

        
        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime); // moves the projectile in the up direction
        }


        private IEnumerator Destroy() // method used to destroy the enemy
        {
            yield return new WaitForSeconds(5f); // waits for 1 seconds
            Destroy(gameObject); // destroys the enemy
        }

        
        private void OnCollisionEnter2D(Collision2D other) // method used to check for collisions
        {
            if (other.gameObject.tag == "Player") // checks if the other object has the player tag
            {
                other.gameObject.GetComponent<PlayerLogic>().Damage(damage); // damages the player
                Destroy(gameObject); // destroys the projectile
            }
            else if (other.gameObject.tag == "Wall") // checks if the other object has the wall tag
            {
                Destroy(gameObject); // destroys the projectile
            }
        }
    }
}
