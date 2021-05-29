using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Timers;
using System;

public class PlayerController : MonoBehaviour
{

    //Defines the player stats.
    public float ballSpeed = 2;
    public float ballScore = 0;
    public int ballExit = 0;
    public int ballPlatform = 0;
    public GameObject ballCamera;
    public Rigidbody ballBody;
    public Vector3 cameraAngle;
    public AudioSource ballPickup;
    public AudioSource ballWhoosh;
    public bool ballFalling;
    public bool ballPaused = false;

    //Defines the level platforms and exits.
    public float platformX = 0;
    public float platformZ = 0;
    public GameObject[] exit = new GameObject[3];
    public GameObject[] platform = new GameObject[3];

    //Defines the camera variables.
    public int cameraPlatform;
    public Vector3 cameraOffset;
    public Vector3 cameraPosition;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI highText;
    public GameObject winCanvas;
    public GameObject hudCanvas;
    public GameObject pauseCanvas;
    private int timeSeconds;

    //Start is called before the first frame update.
    private void Start()
    {

        //Gets the inital camera angle.
        cameraAngle = ballCamera.transform.eulerAngles;

        //Gets the offset from the first platform.
        cameraOffset = ballCamera.transform.position - platform[ballPlatform].transform.position;
        cameraPosition = platform[ballPlatform].transform.position + cameraOffset;

        //Starts a timer which counts every second.
        InvokeRepeating("OnTimerUpdate", 1.0f, 1.0f);

        //Disables the win canvas.
        winCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

    }

    private void Update()
    {
        //Checks if the pause button has been pressed.
        if ((Input.GetKeyDown(KeyCode.Escape)) && (winCanvas.activeSelf == false))
        {
            if (ballPaused == false)
            {
                ballPaused = true;
                Time.timeScale = 0f;
                pauseCanvas.SetActive(true);
            }
            else
            {
                ballPaused = false;
                Time.timeScale = 1f;
                pauseCanvas.SetActive(false);
            }
        }
    }

    //Updates every frame.
    private void FixedUpdate()
    {

        //Gets the direction the player is pressing.
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveS = 0.1f;

        //Changes the rotation of the platform.
        platformX = Mathf.Lerp(platformX, moveX * 10, moveS);
        platformZ = Mathf.Lerp(platformZ, moveZ * 10, moveS);

        //Updates the platform rotation.
        ballCamera.transform.eulerAngles = cameraAngle + new Vector3(-platformZ, 0, platformX);

        //Adds force to the ball.
        ballBody.AddForce(new Vector3(moveX, 0, moveZ) * ballSpeed);

        //Approaches the position of the platform.
        ballCamera.transform.position = Vector3.Lerp(ballCamera.transform.position, cameraPosition, 0.1f);

        //Checks if the ball is below the platform.
        if(transform.position.y < platform[ballPlatform].transform.position.y)
        {

            //Checks if the score is maximum.
            if (ballScore == 30)
            {

                //Enables the win screen.
                winCanvas.SetActive(true);

                //Checks if the score is greater than the high score.
                if((GlobalContoller.highSeconds < timeSeconds) && (GlobalContoller.highSeconds != 0))
                {

                    //Displays the highscore.
                    int seconds = (GlobalContoller.highSeconds % 60);
                    int minutes = (GlobalContoller.highSeconds / 60);
                    highText.text = "Best Time: " + minutes.ToString() + ":" + seconds.ToString();

                }
                else
                {

                    //Displays the highscore.
                    int seconds = (timeSeconds % 60);
                    int minutes = (timeSeconds / 60);
                    highText.text = "Best Time: " + minutes.ToString() + ":" + seconds.ToString();
                    GlobalContoller.highSeconds = timeSeconds;

                }

                //Stops the timer.
                CancelInvoke();

                //Hides the hud canvas.
                hudCanvas.SetActive(false);

            }
            else
            {

                //Increases the ball platform.
                ballPlatform += 1;

            }

            //Updates the position of the ball camera.
            cameraPosition = platform[ballPlatform].transform.position + cameraOffset;

        }

        //Checks if the ball is falling fast.
        if((ballBody.velocity.magnitude > 10) && (ballFalling == false))
        {

            //Plays the whoosh sound effect.
            ballWhoosh.Play();

            //Sets the falling boolean to true.
            ballFalling = true;

        }
        else if (ballBody.velocity.magnitude <= 10)
        {

            //Sets the falling boolean to false.
            ballFalling = false;

        }

    }

    //Detects collision with collectables.
    private void OnTriggerEnter(Collider other)
    {

        //Checks if the trigger is a pickup.
        if (other.gameObject.CompareTag("Pickup"))
        {

            //Increases the score value.
            ballScore += 1;
            scoreText.text = "Score: " + ballScore.ToString();
            ballPickup.Play();

            //Disables the game object.
            other.gameObject.SetActive(false);

            //Checks if the player has reached 10 collectables.
            if((ballScore % 10) == 0)
            {

                //Decativates the exit.
                exit[ballExit].SetActive(false);

                //Increments the exit.
                ballExit += 1;
                ballExit = Mathf.Clamp(ballExit, 0, 2);

            }

        }

    }

    private void OnTimerUpdate()
    {

        //Adds to the timer seconds.
        timeSeconds += 1;

        //Sets the time text.
        int seconds = (timeSeconds % 60);
        int minutes = (timeSeconds / 60);
        timeText.text = "Time: " + minutes.ToString() + ":" + seconds.ToString();

    }

}
