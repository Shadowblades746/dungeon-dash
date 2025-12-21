using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayGame() // method used to play the game
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // loads the next scene
    }
    
    
    public void QuitGame() // method used to quit the game
    {
        Application.Quit(); // quits the application
    } 
}
