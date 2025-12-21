using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts
{
    public class BossAI : MonoBehaviour
    {
        public float fullHp = 100f; // enemy health
        public float currentHp; // current health
        public int points = 10; // score value of the enemy
        public float speed = 200f; // enemy speed
        public float damage = 10f; // damage of the attack
        public float attackRange = 0.5f; // range of the attack
        public float attackRate = 5f; // rate of the attack
        public GameObject boss; // reference to the boss
        public static event Action<int> OnEnemyDeath; // event to call when enemy dies
        public static event Action OnBossDeath; // event to call when boss dies
        public LayerMask players; // reference to the boss layers
        public Transform hitPoint; // reference to the attack point
        public Transform target; // reference to the player
        public Rigidbody2D rigidBody; // reference to the rigidbody
        public Animator animator; // reference to animator 
        
        private float nextAttackTime; // time of the next attack
        private bool isFlipped = true; // initialises character as facing right
        private float timeBelowThreshold = 0f; // time below threshold
        private int currentWaypoint; // current waypoint
        private float nextWaypointDistance = 3f; // distance to the next waypoint
        private bool reachedEndOfPath = false; // checks if reached end of path
        private Path path; // the calculated path
        private Seeker seeker; // reference to the seeker
        private Transform player; // reference to the player

        
        // Start is called before the first frame update
        private void Start()
        {
            currentHp = fullHp; // sets the current health to the full health
            player = GameObject.Find("Player").transform; // finds the player
            animator.SetBool("Died", true); // sets the trigger to died
            animator.SetBool("Died", false); // sets the trigger to died
            seeker = GetComponent<Seeker>(); // gets the seeker component
            rigidBody = GetComponent<Rigidbody2D>(); // gets the rigidbody component
            
            InvokeRepeating(nameof(UpdatePath), 0f, .1f); // calls the update path function
           
            seeker.StartPath(rigidBody.position, target.position, OnPathComplete); // starts the path
        }

        
        private void UpdatePath() // updates the path
        {
             if(seeker.IsDone()) // checks if the seeker is not calculating a path
             {
                seeker.StartPath(rigidBody.position, target.position, OnPathComplete); // starts the path
             }
        }
        
        
        private void OnPathComplete(Path p) // called when the path is calculated
        {
            if (p.error) return; // checks if there is no error
            path = p; // sets the path
            currentWaypoint = 0; // reset current waypoint  
        }

        
        // Update is called once per frame
        private void FixedUpdate()
        {
             if(path == null) // checks if path is null
             {
                 return; // returns
             }
             if(currentWaypoint >= path.vectorPath.Count) // checks if current waypoint is greater than the waypoint count
             {
                 reachedEndOfPath = true; // sets reached end of path to true
                 return; // returns
             }
             if(currentWaypoint < path.vectorPath.Count) // checks if current waypoint is less than the waypoint count
             {
                 reachedEndOfPath = false; // sets reached end of path to false
             }
      
             var direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody.position).normalized; // gets the direction of the next waypoint
             var force = direction * (speed * Time.deltaTime); // gets the force to apply to the enemy
             rigidBody.AddForce(force); // adds the force to the enemy
             var distance = Vector2.Distance(rigidBody.position, path.vectorPath[currentWaypoint]); // gets the distance to the next waypoint
        
             if(distance < nextWaypointDistance) // checks if the distance is less than the next waypoint distance
             {
                 currentWaypoint++; // increments the current waypoint
             }
             
             Vector3 flipped = transform.localScale; // gets the local scale of the enemy
             flipped.z *= -1f; // flips the enemy on the z axis
        
             if (transform.position.x > player.position.x && isFlipped) // checks if the player is to the right of the enemy and the enemy is flipped
             {
                 transform.localScale = flipped; // flips the enemy
                 transform.Rotate(0f, 180f, 0f); // rotates the enemy
                 isFlipped = false; // sets the isFlipped to false
             }
             else if (transform.position.x < player.position.x && !isFlipped) // checks if the player is to the left of the enemy and the enemy is not flipped
             {
                 transform.localScale = flipped; // flips the enemy
                 transform.Rotate(0f, 180f, 0f); // rotates the enemy
                 isFlipped = true; // sets the isFlipped to true
             }
            
             animator.SetFloat("Speed", math.abs(rigidBody.linearVelocity.x)); // sets the speed of the enemy to the absolute value of the speed for the animations
             if(Time.time >= nextAttackTime) // checks if the time is greater than the next attack time
             {
                 if ((Vector2.Distance(player.position, rigidBody.position) <= attackRange) &&  currentHp > 50 ) // checks if the player is in range
                 {
                     animator.SetBool("Attack", true); // sets the trigger to attack
                     Attack(); // calls the attack method
                     nextAttackTime = Time.time + 1f / attackRate; // sets the next attack time
                 }
                 else if ((Vector2.Distance(player.position, rigidBody.position) <= attackRange) &&  currentHp < 50 ) // checks if the player is in range
                 {
                     animator.SetBool("Enraged", true); // sets the trigger to attack
                     Attack(); // calls the attack method
                     nextAttackTime = Time.time + 1f / attackRate; // sets the next attack time
                 }
                 else if (Vector2.Distance(player.position, rigidBody.position) > attackRange) // checks if the player is not in range
                 {
                     animator.SetBool("Attack", false); // sets the trigger to attack
                     animator.SetBool("Enraged", false); // sets the trigger to attack
                 }
             }
             
             if (currentHp <= 50) // checks if the current health is less than or equal to 50
             {
                 if (speed < 2000)
                 {
                        speed += 5; // sets the speed to 300
                 }
             }
        }


        private void Collect() // method used to collect the enemy
        {
            OnEnemyDeath?.Invoke(points); // calls the on enemy death event
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void Attack() // method used to attack
        {
            var health = player.GetComponent<PlayerLogic>().currentHp; // gets the current health of the player
            switch (health) // checks the health of the player
            {
                case > 0: // checks if the current health is greater than 0
                {
                    player.GetComponent<PlayerLogic>().Damage(damage); // calls the damage method on the player
                    break; // breaks the switch statement
                }
                case <= 0: // checks if the current health is less than or equal to 0
                {
                    animator.SetBool("Attack", false); // sets the trigger to attack
                    var model = GameObject.Find("model"); // finds the player
                    Destroy(model); // destroys the player
                    break; // breaks the switch statement
                }
            }
        }
        
        
        public void Damage(float damageTaken) // method used to take damage
        {
            currentHp -= damageTaken; // takes the damage from the current health
            if (currentHp <= 0) // checks if the current health is less than or equal to 0
            {
                GetComponent<Collider2D>().enabled = false; // disables the collider
                animator.Play("Death"); // plays the death animation
                StartCoroutine(DeathAnimation()); // calls the execute with delay method
            }
        }

        
        private IEnumerator DeathAnimation() // method used to destroy the enemy
        {
            yield return new WaitForSeconds(1f); // waits for 1 seconds
            Collect(); // calls the collect method
            boss.SetActive(false); // sets the boss to inactive
            OnBossDeath?.Invoke(); // calls the on boss death event
        }
        
    }
}
