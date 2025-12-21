using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class NPCAI : MonoBehaviour
    {
        public GameObject dialoguePanel; // reference to the dialogue panel
        public TextMeshProUGUI dialogueText; // reference to the dialogue text
        public string[] dialogue; // array of dialogue
        public int dialogueIndex; // index of the dialogue
        
        public float wordSpeed = 0.1f; // speed of the words
        public bool playerInRange; // checks if the player is in range
        
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse1) && playerInRange) // checks if the player is in range and the E key is pressed
            {
                if(dialogueIndex < dialogue.Length - 1) // checks if the dialogue index is less than the length of the dialogue array
                {
                    dialogueIndex++; // increments the dialogue index
                    StartCoroutine(TypeDialogue()); // starts the type dialogue coroutine
                }
                else
                {
                    dialoguePanel.SetActive(false); // sets the dialogue panel to inactive
                    dialogueIndex = 0; // resets the dialogue index
                }
            }
        }
        
        
        public IEnumerator TypeDialogue() // method used to type the dialogue
        {
            dialogueText.text = ""; // sets the dialogue text to empty
            foreach (char letter in dialogue[dialogueIndex]) // loops through each letter in the dialogue
            {
                dialogueText.text += letter; // adds the letter to the dialogue text
                yield return new WaitForSeconds(wordSpeed); // waits for the word speed
            }
        }
        
        
        private void OnTriggerEnter2D(Collider2D other) // method used to check for collisions
        {
            if (other.gameObject.tag == "Player") // checks if the other object has the player tag
            {
                playerInRange = true; // sets the player in range flag to true
                dialoguePanel.SetActive(true); // sets the dialogue panel to active
                StartCoroutine(TypeDialogue()); // starts the type dialogue coroutine
            }
        }
        
        
        private void OnTriggerExit2D(Collider2D other) // method used to check for collisions
        {
            if (other.gameObject.tag == "Player") // checks if the other object has the player tag
            {
                playerInRange = false; // sets the player in range flag to false
                dialoguePanel.SetActive(false); // sets the dialogue panel to inactive
                StopAllCoroutines(); // stops all coroutines
            }
        }
    }
}