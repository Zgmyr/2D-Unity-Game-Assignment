// Author: Zachary Gmyr
// 9/14/2024
// This script controls player movement as well as win & lose conditions and a 3 point health system


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public float speed;
    private float timer;
    public Text timerText;
    public Text winText;
    public Text loseText;
    public Button restartButton;

    private int health;
    public Image[] hearts;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timer = 60; // timer starts at 1 minute & counts down

        health = 3; // start with 3 health

        // Hide win/lose text & restart button
        winText.text = "";
        loseText.text = "";
        restartButton.gameObject.SetActive(false);
    }

    // Update is in sync with physics engine
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(moveHorizontal,moveVertical);
        
        // rb2d.AddForce(movement * speed);
        rb2d.velocity = movement * speed;
    }

    void Update()
    {
        // WIN CONDITION
        // update the timer countdown (if we haven't already lost)
        if (timer > 0 && health > 0)
        {
            timer -= Time.deltaTime; // decrease timer
            if (timer < 0)
                timer = 0; // stops timer at 0
            Debug.Log("Timer: " + timer);
            UpdateTimerText(); // udpate the UI
        }
        if (timer == 0)
        {
            winText.text = "YOU WIN!";
            restartButton.gameObject.SetActive(true);
        }

        // Health System - start with 3 hearts & each time hit these decrease
        // Source: https://www.youtube.com/watch?v=3uyolYVsiWc (learned how to use arrays of Images)
        for (int i = 0; i < hearts.Length; i++) {
            if (i < health) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }
        }
    }

    // update 'timer' string to 2 decimal places
    void UpdateTimerText()
    {
        // round down into an integer using FloorToInt
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        // string.Format formats using comp3osite strings where {0:00} & {1:00}
        //      specify first & second argument, 2 decimal places
        // Source: https://learn.microsoft.com/en-us/dotnet/api/system.string.format?view=net-8.0
        //  & https://learn.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting
        timerText.text = "TIME LEFT: " + string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // LOSE CONDITION:
        // if we collide with a pickup item (and haven't already won yet)
        if (other.gameObject.CompareTag("Pickup") && string.IsNullOrEmpty(winText.text))
        {
            if (health > 0) {
                // decrease health by one
                health -= 1;

                // check if no health left
                if (health == 0)
                {
                    // LOSE CONDITION
                    loseText.text = "YOU LOSE!";
                    restartButton.gameObject.SetActive(true);
                }
            }
        }
    } 

    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
