using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D.IK;

namespace _Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        public float fullHp = 100f; // enemy health
        public float currentHp; // current health
        public int points = 10; // score value of the enemy
        public float speed = 200f; // enemy speed
        public float damage = 1f; // damage of the attack
        public float attackRange = 0.5f; // range of the attack
        public float attackRate = 5f; // rate of the attack
        public float knockbackForce = 500f; // knockback force
        public static event Action<int> OnEnemyDeath; // event to call when enemy dies
        public LayerMask players; // reference to the player layers
        public Transform hitPoint; // reference to the attack point
        public Transform target; // reference to the player
        public Rigidbody2D rigidBody; // reference to the rigidbody
        public Animator animator; // reference to animator 
        
        public List<Collider2D> colliders; // reference to the colliders
        public List<HingeJoint2D> joints; // reference to the joints
        public List<Rigidbody2D> rigidbodies; // reference to the rigidbodies
        public List<LimbSolver2D> limbSolvers; // reference to the limb solvers
        
        private float nextAttackTime; // time of the next attack
        private bool isFacingRight = true; // initialises character as facing right
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
            Ragdoll(false); // calls the ragdoll method
            currentHp = fullHp; // sets the current health to the full health
            player = GameObject.Find("Player").transform; // finds the player
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
            
             var localScale = transform.localScale; // get the local scale of the player
             switch (rigidBody.linearVelocity.x) // checks the velocity of the enemy
             {
                 case > -0.1f: // checks if the velocity is less than -0.01f
                     timeBelowThreshold += Time.deltaTime; // increments the time below threshold
                     if (timeBelowThreshold > 0.25f) // checks if the time below threshold is greater than 0.25f
                     {
                         localScale.x *= isFacingRight ? 1 : -1; // flips the player on the x axis
                         isFacingRight = true; // sets the isFacingRight to true
                         transform.localScale = localScale; // sets the local scale of the enemy to the local scale
                         timeBelowThreshold = 0; // resets the time below threshold
                     }
                     break; // breaks the switch statement
                 case < 0.1f: // checks if the velocity is greater than 0.01f
                     timeBelowThreshold += Time.deltaTime; // increments the time below threshold
                     if (timeBelowThreshold > 0.25f) // checks if the time below threshold is greater than 0.25f
                     {
                         localScale.x *= isFacingRight ? -1 : 1; // flips the player on the x axis
                         isFacingRight = false; // sets the isFacingRight to false
                         transform.localScale = localScale; // sets the local scale of the enemy to the local scale
                         timeBelowThreshold = 0; // resets the time below threshold
                     }
                     break; // breaks the switch statement
             }
            
             animator.SetFloat("Speed", math.abs(rigidBody.linearVelocity.x)); // sets the speed of the player to the absolute value of the speed for the animations
             if(Time.time >= nextAttackTime) // checks if the time is greater than the next attack time
             {
                 if (Vector2.Distance(player.position, rigidBody.position) <= attackRange) // checks if the player is in range
                 {
                     animator.SetBool("Attack", true); // sets the trigger to attack
                     Attack(); // calls the attack method
                     nextAttackTime = Time.time + 1f / attackRate; // sets the next attack time
                 }
                 else if (Vector2.Distance(player.position, rigidBody.position) > attackRange) // checks if the player is not in range
                 {
                     animator.SetBool("Attack", false); // sets the trigger to attack
                 }
             }
        }


        private void Collect() // method used to collect the enemy
        {
            OnEnemyDeath?.Invoke(points); // calls the on enemy death event
        }
        
        
        private void Attack() // method used to attack
        {
            var health = player.GetComponent<PlayerLogic>().currentHp; // gets the current health of the player
            switch (health) // checks the health of the player
            {
                case > 0: // checks if the current health is greater than 0
                {
                    var hurtPlayer = Physics2D.OverlapCircle(hitPoint.position, attackRange, players); // gets the size of the overlap circle
                    hurtPlayer.GetComponent<PlayerLogic>().Damage(damage); // calls the damage method on the player
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
            var enemy = GameObject.Find("Cow1"); // finds the enemy
            var player1 = GameObject.Find("Player"); // finds the player
            currentHp -= damageTaken; // takes the damage from the current health
            Vector3 direction = (enemy.transform.position - player1.transform.position).normalized; // calculates the direction of the knockback
            rigidBody.AddForce(direction * knockbackForce, ForceMode2D.Impulse); // applies the knockback force to the enemy
            if (currentHp <= 0) // checks if the current health is less than or equal to 0
            {
                GetComponent<Collider2D>().enabled = false; // disables the collider
                Ragdoll(true); // calls the ragdoll method
                StartCoroutine(DeathAnimation()); // calls the execute with delay method
            }
        }

        
        private IEnumerator DeathAnimation() // method used to destroy the enemy
        {
            yield return new WaitForSeconds(3f); // waits for 5 seconds
            Collect(); // calls the collect method
            Destroy(gameObject); // destroys the enemy
            enabled = false; // disables the script
        }


        private void Ragdoll(bool ragdollEnabled) // method to enable the ragdoll
        {
            animator.enabled = !ragdollEnabled; // enables the animator
            foreach (var collider in colliders) // loops through each collider
            {
                collider.enabled = ragdollEnabled; // enables the collider
            }
            
            foreach (var joint in joints) // loops through each joint
            {
                joint.enabled = ragdollEnabled; // enables the joint
            }
            
            foreach (var rigidbody in rigidbodies) // loops through each rigidbody
            {
                rigidbody.simulated = ragdollEnabled; // enables the rigidbody
            }
        }
    }
}
