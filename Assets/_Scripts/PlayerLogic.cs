using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class PlayerLogic : MonoBehaviour
    {
        public float fullHp = 100f; // player health
        public float currentHp; // current health
        public int healthPotion = 0; // health potion count
        public float speed = 5f; // speed of the player
        public float damage; // damage of the attack
        public int strengthPotion = 0; // damage potion count
        public float attackRange = 0.5f; // range of the attack
        public float attackRate = 5f; // rate of the attack
        private float nextAttackTime; // time of the next attack
        private bool isFacingRight = true; // initialises character as facing right
        public static event Action OnPlayerDeath; // event to call when player dies
        public Image healthBar; // reference to the health bar
        public TextMeshProUGUI healthAmount; // reference to the health text
        public TextMeshProUGUI healthPotionAmount; // reference to the health potion text
        public TextMeshProUGUI damageAmount; // reference to the damage text
        public TextMeshProUGUI strengthPotionAmount; // reference to the damage potion text
        public LayerMask enemies; // reference to the enemy layers
        public Transform hitPoint; // reference to the attack point
        public Rigidbody2D rigidBody; // reference to rigid body
        public Animator animator; // reference to animator 
        
        
        // Start is called before the first frame update
        void Start()
        {
            currentHp = fullHp; // sets the current health to the full health
            healthAmount.text = currentHp.ToString(); // sets the health text to the current health
            healthPotionAmount.text = 0.ToString(); // sets the health potion text to the health potion
            damageAmount.text = damage.ToString(); // sets the damage potion text to the damage potion
            strengthPotionAmount.text = 0.ToString(); // sets the damage potion text to the damage potion
        }
        
        
        private static float MovementCheck(Vector3 movement) // method to check the movement
        {
            if (Mathf.Abs(movement.x) > 0.01) // checks if player is moving in the x direction
            {
                return Mathf.Abs(movement.x); // returns the positive value of movement
            }
            if (Mathf.Abs(movement.y) > 0.01) // checks if the player is moving in the y direction
            {
                return Mathf.Abs(movement.y); // returns the positive value of movement
            }
            return 0;
        }
        
        
        // Update is called once per frame
        void Update()
        {
            var movement = new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0.0f); // get horizontal and vertical input from the player and resolves the vector direction using trigonometry
            transform.position += movement * Time.deltaTime; // move the player in the direction of the input using a transformation
            animator.SetFloat("speed", MovementCheck(movement)); // sets the speed of the player to the absolute value of the vertical input


            if(Input.GetKeyDown(KeyCode.Space)) // checks if player presses mouse1
            {
                rigidBody.AddForce(new Vector2(0f,5f), ForceMode2D.Impulse); // adds a force to the player to make them jump
                animator.Play("Jump"); // plays the attack2 animation
            }
            
                   
            var localScale = transform.localScale; // get the local scale of the player
            if (Input.GetKeyDown(KeyCode.A)  || Input.GetKeyDown(KeyCode.LeftArrow)) // checks if player presses A
            {
                localScale.x *= isFacingRight ? 1 : -1; // flips the player on the x axis
                isFacingRight = true; // sets the isFacingRight to true
                transform.localScale = localScale; // sets the local scale of the player to the local scale
            }
            else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // checks if player presses D
            {
                localScale.x *= isFacingRight ? -1 : 1; // flips the player on the x axis
                isFacingRight = false; // sets the isFacingRight to false
                transform.localScale = localScale; // sets the local scale of the player to the local scale
            }
            
            if (Input.GetKeyDown(KeyCode.H)) // checks if player presses mouse1
            {
                if (healthPotion > 0) // checks if the health potion is greater than 0
                {
                    Heal(20); // calls the heal method
                    healthPotion--; // decrements the health potion
                    healthPotionAmount.text = healthPotion.ToString(); // sets the health potion text to the health potion
                }
            }
            
            if (Input.GetKeyDown(KeyCode.J)) // checks if player presses mouse1
            {
                if (strengthPotion > 0) // checks if the strength potion is greater than 0
                {
                    Strength(10); // calls the strength method
                    strengthPotion--; // decrements the damage potion
                    strengthPotionAmount.text = strengthPotion.ToString(); // sets the damage potion text to the damage potion
                }
            }
            
            
            if(Time.time >= nextAttackTime) // checks if the time is greater than the next attack time
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) // checks if player presses mouse0
                {
                    Attack(); // calls the attack method
                    nextAttackTime = Time.time + 1f / attackRate; // sets the next attack time
                }
            }
        }

        
        private void Attack() // method used to attack
        {
            animator.Play("Attack"); // plays the attack animation
            var hurtEnemies = Physics2D.OverlapCircleAll(hitPoint.position, attackRange, enemies); // gets the size of the overlap circle
            foreach (var enemy in hurtEnemies) // loops through each enemy
            {
                if (enemy.GetComponent<EnemyAI>()) // checks if the enemy has the enemy AI script
                {
                    enemy.GetComponent<EnemyAI>().Damage(damage); // calls the damage method on the enemy
                }
                else if (enemy.GetComponent<BossAI>()) // checks if the enemy has the boss AI script
                {
                    enemy.GetComponent<BossAI>().Damage(damage); // calls the damage method on the enemy
                }
                else if (enemy.GetComponent<RangedAI>()) // checks if the enemy has the ranged AI script
                {
                    enemy.GetComponent<RangedAI>().Damage(damage); // calls the damage method on the enemy
                }
                else if (enemy.GetComponent<ChestAI>()) // checks if the enemy has the melee AI script
                {
                    enemy.GetComponent<ChestAI>().Damage(damage); // calls the damage method on the enemy
                }
                
            }   
        }
        
        
        public void Damage(float damageTaken) // method used to take damage
        {   
            currentHp -= damageTaken; // takes the damage from the current health
            healthBar.fillAmount = currentHp / fullHp; // sets the health bar to the current health divided by the full health
            healthAmount.text = currentHp.ToString(); // sets the health text to the current health
            if (currentHp <= 0) // checks if the current health is less than or equal to 0
            {
                animator.Play("Die"); // plays the die animation
                OnPlayerDeath?.Invoke(); // calls the on player death event
                GetComponent<Collider2D>().enabled = false; // disables the collider
            }
        }
        
        public void HealthPotion() // method used to add health potions
        {
            healthPotion++; // increments the health potion
            healthPotionAmount.text = healthPotion.ToString(); // sets the health potion text to the health potion
        }
        
        public void StrengthPotion() // method used to add damage potions
        {
            strengthPotion++; // increments the damage potion
            strengthPotionAmount.text = strengthPotion.ToString(); // sets the damage potion text to the damage potion
        }
        
        
        public void Heal(float healAmount) // method used to heal the player
        {
            currentHp += healAmount; // adds the heal amount to the current health
            healthBar.fillAmount = currentHp / fullHp; // sets the health bar to the current health divided by the full health
            healthAmount.text = currentHp.ToString(); // sets the health text to the current health
        }
        
        public void Strength(float strengthAmount) // method used to increase the strength of the player
        {
            damage += strengthAmount; // adds the strength amount to the damage
            damageAmount.text = damage.ToString(); // sets the damage potion text to the damage potion
        }
    }
}
