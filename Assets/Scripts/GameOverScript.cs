using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    //This script controls what happens on the Game Over screen
    //Currently, the only option is for the player to hit 'Space' and restart the game
    
    //Every frame, we check for player inputs
    void Update()
    {
        //If the player hit space. . .
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //Then load the 'Example 3' scene
            SceneManager.LoadScene("Example 3");
        }
    }
}
