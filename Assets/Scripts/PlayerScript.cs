using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //These are the player's Variables, the raw info that defines them
    
    //The Rigidbody2D is a component that gives the player physics, and is what we use to move
    public Rigidbody2D RB;

    //TextMeshPro is a component that draws text on the screen.
    //We use this one to show our score.
    public TextMeshPro ScoreText;

    //This one will track health
    public TextMeshPro HealthText;
    
    //This will control how fast the player moves
    public float Speed = 5;
    
    //This is how many points we currently have
    public int Score = 0;

    //This is the current player health
    public int Health = 5;

    //Float to track cooldown time for the dash?
    public float dashTimer = 0f;
    public float dashReset = 2f;
    
    //Start automatically gets triggered once when the objects turns on/the game starts
    void Start()
    {
        
        //During setup we call UpdateScore to make sure our score text looks correct
        UpdateScore();
        //we call UpdateHealth to make sure our health text looks correct
        UpdateHealth();
    }

    //Update is a lot like Start, but it automatically gets triggered once per frame
    //Most of an object's code will be called from Update--it controls things that happen in real time
    void Update()
    {
        //check to see if dash is on cooldown
        if(dashTimer > 2f)
        {
            //if it is, count down the rest timer
            dashReset -= Time.deltaTime;
        }

        //if dash reset is up, cycle the timers back
        if(dashReset <= 0f)
        {
            dashTimer = 0f;
            dashReset = 2f;
        }

        float currentSpeed = Speed; //set current speed to default value
        if (dashTimer <= 2) //check to see if player can dash
        {
            //check to see if player is pressing the dash key
            if (Keyboard.current.rightShiftKey.isPressed || Keyboard.current.leftShiftKey.isPressed)
            {
                currentSpeed = Speed * 2f; //set speed to dash 
                dashTimer += Time.deltaTime; //increase dash timer when dashing
            }
        }

        //Finally, I take that variable and I feed it to the component in charge of movement
        //we're using a new function to calculate this now!
        Vector3 velocity = Move(currentSpeed);
        //once we've gotten the return value from our Move() function, we can set the player RB
        RB.linearVelocity = velocity;
    }

    //prefix(scope) [return type] name(parameters)
    //this is a modular function that takes a speed value and calculates a move vector
    //based off the player inputs when the function is called (in update)
    private Vector3 Move(float currentSpeed)
    {
        //The code below controls the character's movement
        //First we make a variable that we'll use to record how we want to move
        Vector2 vel = new Vector2(0, 0);
        //Then we use if statement to figure out what that variable should look like
        //If I hold the right arrow key, the player should move right. . .
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            vel.x = currentSpeed;
        }
        //If I hold the left arrow, the player should move left. . .
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            vel.x = -currentSpeed;
        }
        //If I hold the up arrow, the player should move up. . .
        if (Keyboard.current.upArrowKey.isPressed)
        {
            vel.y = currentSpeed;
        }
        //If I hold the down arrow, the player should move down. . .
        if (Keyboard.current.downArrowKey.isPressed)
        {
            vel.y = -currentSpeed;
        }
        return vel;
    }

    //This gets called whenever you bump into another object, like a wall or coin.
    private void OnCollisionEnter2D(Collision2D other)
    {
        //This checks to see if the thing you bumped into had the Hazard tag
        //If it does...
        if (other.gameObject.CompareTag("Hazard"))
        {
            //take damage
            UpdateHealth(-1);
            //check to see if health is 0 or lower
            if (Health <= 0)
            {
                //if dead, run the lose function
                Die();
            }

        }
        
        //This checks to see if the thing you bumped into has the CoinScript script on it
        CoinScript coin = other.gameObject.GetComponent<CoinScript>();
        //If it does, run the code block belows
        if (coin != null)
        {
            //Tell the coin that you bumped into them so they can self destruct or whatever
            coin.GetBumped();
            //Make your score variable go up by one. . .
            //And then update the game's score text
            UpdateScore(coin.value);
        }
    }

    //This function updates the game's score text to show how many points you have
    //Even if your 'score' variable goes up, if you don't update the text the player doesn't know
    public void UpdateScore(int v)
    {
        Score += v;
        ScoreText.text = "Score: " + Score;
    }

    //This function that updates the player health on screen
    public void UpdateHealth(int deltaHealth)
    {
        Health += deltaHealth;
        HealthText.text = "Health: " + Health;
    }

    //If this function is called, the player character dies. The game goes to a 'Game Over' screen.
    public void Die()
    {
        SceneManager.LoadScene("Game Over");
    }
}
