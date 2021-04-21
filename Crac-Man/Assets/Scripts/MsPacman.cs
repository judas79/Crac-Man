using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// T18 get system library, Used to find out if Pac-Man hits a wall
using System;
// T20 Library to use the scoring UI interface
using UnityEngine.UI;

public class MsPacman : MonoBehaviour
{
    // T18 MsPacmans properties; speed
    public float speed = 4f;

    // T18 move mrspacman using rigidbody 2d
    private Rigidbody2D rb;

    // T19 Sprite used when Pac-Man is paused, to stop the sprite; 
    // stop the animation from moving, at a selected single sprite image
    // public so we can drag selected single sprite into our script, to use for this purpose
    public Sprite pausedSprite;

    // T20 Get reference to the SoundManager, for its sound functions, from inside of here
    SoundManager soundManager;

    // T20 get refrences from within SoundManager, for our sound clips, we will use here
    public AudioClip eatingGhost;
    public AudioClip pacmanDies;
    public AudioClip powerupEating;

    // T22 refference to Gameboard, so we can user it here, to acces IsValidSpace()
    Gameboard gameBoard;


    // T22 add reference to the redGhostScript, so we can use it here
    // T23 add references for the other 3 ghosts scripts
    Ghost redGhostScript;
    Ghost pinkGhostScript;
    Ghost blueGhostScript;
    Ghost orangeGhostScript;


    // T18 awake function, to create component(s) before they are to be used
    private void Awake()
    {
        // T18 create the rb
        rb = GetComponent<Rigidbody2D>();

        // T22 acess Gameboard, the script itself, by finding it
        gameBoard = FindObjectOfType(typeof(Gameboard)) as Gameboard;

        // T22 find and create the redGhostScript game object, so it is started before it is used
        // T23 find and create the game objects for the 3 remaining ghosts, and start them
        GameObject redGhostGO = GameObject.Find("RedGhost");
        GameObject pinkGhostGO = GameObject.Find("PinkGhost");
        GameObject blueGhostGO = GameObject.Find("BlueGhost");
        GameObject orangeGhostGO = GameObject.Find("OrangeGhost");

        // T22 get the script (Ghost) attached to gameObject redGhostGO as a Ghost type
        // T23 get the script (Ghost) attached to the remaining 3 gameObjects as a Ghost type
        redGhostScript = (Ghost)redGhostGO.GetComponent(typeof(Ghost));
        pinkGhostScript = (Ghost)pinkGhostGO.GetComponent(typeof(Ghost));
        blueGhostScript = (Ghost)blueGhostGO.GetComponent(typeof(Ghost));
        orangeGhostScript = (Ghost)orangeGhostGO.GetComponent(typeof(Ghost));
    }

    // T18 Start is called before the first frame update
    // get mspacman and start moving her to the left, at the default speed, when game begins
    void Start()
    {
        rb.velocity = new Vector2(-1, 0) * speed;

        // T20 get reference, to be able to use, our SoundManager, from within here
        // find the name soundManager by finding GameObject, SoundManager
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }


    // T18 move mspacman around our screen, using rigidbodies 2d
    // we will take input from the user, that will instruct which direction to go in
    private void FixedUpdate()
    {
        // get mspacmans horizontal movement, x, for the a or d key to move left or right
        // get vertical movement , for y, keys W of S
        float horzMove = Input.GetAxisRaw("Horizontal");
        float vertMove = Input.GetAxisRaw("Vertical");

        // T19 Whenever one of the letter keys are clicked, we need to store the direction we want to move in
        Vector2 moveVect;

        // T19 Used to get direction Pac-Man is moving on the X & Y access
        // so mspacman can reverse direction, anywhere, without having to be on a turning Point
        var localVelocity = transform.InverseTransformDirection(rb.velocity);

        // T18 check if input a was pressed
        if (Input.GetKeyDown("a"))
        {
            // T19 enable mspacman to reverse direction anywhere, without being on turn points
            // if mspacman is moving in the right direction
            //if(localVelocity.x > 0)
            // T22 if statement ogmented to verify that pacman can move left, using the gameBoard script
            // we are checking the position x to the left of its current position, so (-1), y remains the same, so its valid 
            // this will keep pacman withing the inner, and outer borders of out maze
            if(localVelocity.x > 0 && gameBoard.IsValidSpace(transform.position.x -1, transform.position.y))
            {
                // T19 define moveVect, for moving horizontally, y is 0
                moveVect = new Vector2(horzMove, 0);

                // T19 Position Pac-Man at middle of the lane, centered for movement changes
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                // T19 get mspacmans and make her velocity go left, since she can
                rb.velocity = moveVect * speed;

                // make mspacman face left, using her sprite
                transform.localScale = new Vector2(1, 1);
                // set rotation to its default
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // T19 define moveVect, for moving horizontally, y is 0
                moveVect = new Vector2(horzMove, 0);

                // T19 since the user chose 'a' he wants to move horizontaly to the left
                // we need to verify, that it is posible to move to the left, or not
                // so we will create a bool function below: CanIMoveInDirection()

                // if we can move in a requested direction
                if (CanIMoveInDirection(moveVect))
                {
                    // T19 Position Pac-Man at middle of the lane, centered for movement changes
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                    // T19 get mspacmans and make her velocity go left, since she can
                    rb.velocity = moveVect * speed;

                    // make mspacman face left, using her sprite
                    transform.localScale = new Vector2(1, 1);
                    // set rotation to its default
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }

            /* T19 Moved in T19 into if and else statements above this section
             * 
            // T18 get mspacmans rigidbody make her velocity go left, x is horzMove, y is 0
            // T19 changed to to ...below the commented out one
            //rb.velocity = new Vector2(horzMove, 0) * speed;
            rb.velocity = moveVect * speed;

            // make mspacman face left, using her sprite
            transform.localScale = new Vector2(1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            */

        }

        // check if its another input option for horizontal
        else if (Input.GetKeyDown("d"))
        {
            // T19 enable mspacman to reverse direction anywhere, without being on turn points
            // if mspacman is moving in the left direction
            //if (localVelocity.x < 0)
            // T22 augmented to verify that pacman can move right 1 space, by using gameBoard.cs IsValidSpace()
            if (localVelocity.x < 0 && gameBoard.IsValidSpace(transform.position.x + 1, transform.position.y))
            {
                // T19 define moveVect, for moving horizontally, y is 0
                moveVect = new Vector2(horzMove, 0);

                // T19 Position Pac-Man at middle of the lane, centered for movement changes
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                // T19 get mspacmans and make her velocity go left, since she can
                rb.velocity = moveVect * speed;

                // make mspacman face left, using her sprite
                transform.localScale = new Vector2(-1, 1);

                // set rotation to its default
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // T19 define moveVect, for moving horizontally, y is 0
                moveVect = new Vector2(horzMove, 0);

                // T19 since the user chose 'a' he wants to move horizontaly to the left
                // we need to verify, that it is posible to move to the left, or not
                // so we will create a bool function below: CanIMoveInDirection()

                // if we can move in a requested direction
                if (CanIMoveInDirection(moveVect))
                {
                    // T19 Position Pac-Man at middle of the lane, centered for movement changes
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                    // T19 get mspacmans and make her velocity go left, since she can
                    rb.velocity = moveVect * speed;

                    // make mspacman face left, using her sprite
                    transform.localScale = new Vector2(-1, 1);

                    // set rotation to its default
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }

            /* T19 Moved in T19 into if and else statements above this section
             * 
            // get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
            //rb.velocity = new Vector2(horzMove, 0) * speed;
            rb.velocity = moveVect * speed;

            // make mspacman face right, using her sprite, she faces left by default
            transform.localScale = new Vector2(-1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            */

        }

        // check if its another input option, vertical up
        else if (Input.GetKeyDown("w"))
        {
            // T19 enable mspacman to reverse direction anywhere, without being on turn points
            // if mspacman is moving in the left direction
            //if (localVelocity.y > 0)
            // T22 augmented to verify that pacman can move up 1 space, by using gameBoard.cs IsValidSpace()
            if (localVelocity.y > 0 && gameBoard.IsValidSpace(transform.position.x, transform.position.y + 1))
            {
                // T19 define moveVect, for moving vertically, x is 0
                moveVect = new Vector2(0, vertMove);

                // T19 Position Pac-Man at middle of the lane, centered for movement changes
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                // T18 get mspacmans rigidbody make her velocity go up, x is vertMove,x is 0
                //rb.velocity = new Vector2(0, vertMove) * speed;
                // T19
                rb.velocity = moveVect * speed;

                // make mspacman face right, using her sprite, she faces left by default
                transform.localScale = new Vector2(1, 1);
                // set rotation to its default
                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else
            {


                // T19 define moveVect, for moving vertically, x is 0
                moveVect = new Vector2(0, vertMove);

                // if we can move in a requested direction
                if (CanIMoveInDirection(moveVect))
                {
                    // T19 Position Pac-Man at middle of the lane, centered for movement changes
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                    // T18 get mspacmans rigidbody make her velocity go up, x is vertMove,x is 0
                    //rb.velocity = new Vector2(0, vertMove) * speed;
                    // T19
                    rb.velocity = moveVect * speed;

                    // make mspacman face right, using her sprite, she faces left by default
                    transform.localScale = new Vector2(1, 1);
                    // set rotation to its default
                    transform.localRotation = Quaternion.Euler(0, 0, 270);
                }
            }

            /* T19 Moved in T19 into if and else statements above this section
             * 
            // T18 get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
            //rb.velocity = new Vector2(0, vertMove) * speed;
            // T19
            rb.velocity = moveVect * speed;

            // make mspacman face right, using her sprite, she faces left by default
            transform.localScale = new Vector2(1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            */

        }

        else if (Input.GetKeyDown("s"))
        {
            // T19 enable mspacman to reverse direction anywhere, without being on turn points
            // if mspacman is moving in the left direction
            //if (localVelocity.y < 0)
            // T22 augmented to verify that pacman can move down 1 space, by using gameBoard.cs IsValidSpace()
            if (localVelocity.y < 0 && gameBoard.IsValidSpace(transform.position.x, transform.position.y - 1))
            {
                // T19 define moveVect, for moving vertically, x is 0
                moveVect = new Vector2(0, vertMove);

                // T19 Position Pac-Man at middle of the lane, centered for movement changes
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                // T18 get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
                //rb.velocity = new Vector2(0, vertMove) * speed;
                // T19
                rb.velocity = moveVect * speed;

                // make mspacman face right, using her sprite, she faces left by default
                transform.localScale = new Vector2(1, 1);

                // set rotation to its default
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {


                // T19 define moveVect, for moving vertically, x is 0
                moveVect = new Vector2(0, vertMove);

                // if we can move in a requested direction
                if (CanIMoveInDirection(moveVect))
                {
                    // T19 Position Pac-Man at middle of the lane, centered for movement changes
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);

                    // T18 get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
                    //rb.velocity = new Vector2(0, vertMove) * speed;
                    // T19
                    rb.velocity = moveVect * speed;

                    // make mspacman face right, using her sprite, she faces left by default
                    transform.localScale = new Vector2(1, 1);

                    // set rotation to its default
                    transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }

            /* T19 Moved in T19 into if and else statements above this section
             * 
            // T18 get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
            //rb.velocity = new Vector2(0, vertMove) * speed;
            // T19
            rb.velocity = moveVect * speed;


            // make mspacman face right, using her sprite, she faces left by default
            transform.localScale = new Vector2(1, 1);

            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            */

        }
        // T19 call our paused mspacman image, to see if we hit a wall, and are going to stop
        UpdateEatingAnimation();
    }


    // T19 Find out if Pac-man is on a TurningPoint, 
    // to verify if mspacman  can move in the direction (dir), inputed by the user, using keypad
    bool CanIMoveInDirection(Vector2 dir)
    {
        // get mspacmans current position
        Vector2 pos = transform.position;

        // Used to find if there a Point in the array or null
        // is there a point in the array that matches up with mspacmans current position
        // find the GBGrid game object, to get hold of the gBPoints array, through the GameBoard script component,
        // the gBPoints array, inside of the Gameboard script holds the dimmnsions of the x,y gameplay turningpoint area: [27, 30];
        Transform point = GameObject.Find("GBGrid").GetComponent<Gameboard>().gBPoints[(int)pos.x, (int)pos.y];

        // did we find a turningpoint in the array, we just looked for one in, 
        // not a null, everywhere else will  have a turning Point
        if (point != null)
        {
            // get Points associated with that specific game object, point
            GameObject PointGO = point.gameObject;

            // Get vector To Next Point array attached to the point, and store in an array
            // by getting specifically, the TurningPoint, and assigning it to itself
            Vector2[] vectToNextPoint = PointGO.GetComponent<TurningPoint>().vectToNextPoint;

            // Cycle through the attached vectToNextPoint array,
            // to see if we have a match with the direction the user wants to go in
            foreach (Vector2 vect in vectToNextPoint)
            {
                if (vect == dir)
                {
                    // if we get a match
                    return true;
                }
            }
        }
        return false;
    }


    // T19 Check if Pac-Man hit a wall
    // Change all Dots to Dot Tag and Pills to Pill Tag
    // Add 'Is Trigger' to Points with Circle Collider Radius .1
    // Add Tag Point to all Points
    private void OnTriggerEnter2D(Collider2D col)
    {
        bool hitAWall = false;

        // T19 if mspacman hits the component that has a tag of 'Point' associated with it,
        // not dots or Pills, we will get the vector of the next turning point, to verify if we can continue 
        // moving, in the direction we are moving in, or if we hit a wall; if the turningpoint doesn't exist means its a wall there
        if (col.gameObject.tag == "Point")
        {
            Vector2[] vectToNextPoint = col.GetComponent<TurningPoint>().vectToNextPoint;

            // Cycle through the attached vectToNextPoint array
            // to see if there is a Vector2 == Pac-Mans velocity 
            // or the direction Pac-Man wants to travel in 
            // see if the vectToNextPoint exists, and look in its elements to see if,
            // mspacmans rigidbody s speed or direction is in that array, and normalize since we only need the direction
            if (Array.Exists(vectToNextPoint, element => element == rb.velocity.normalized))
            {
                // yes there is a posibble direction to go in
                hitAWall = false;
            }
            else
            {
                // yes we hit a wall; can't go in that direction
                hitAWall = true;
            }

            // verify the mspacman is centered in the maze borders
            // on a turning point then stop, by getting the Point that mspacman col with
            // Needed to make Pac-Man stop on the Point when it 
            // hits a wall
            transform.position = new Vector2((int)col.transform.position.x + .5f,
                (int)col.transform.position.y + .5f);

            // If we hit a wall stop mspacman from moving
            if (hitAWall)
            {
                rb.velocity = Vector2.zero;
            }
        }

        // T21 Simulates going through the portal
        // setup vector2 so mspacman can move around, from one side of the maze,
        // to the other opposite side, simulating, going through the portal
        Vector2 pmMoveVect = new Vector3(0, 0);

        // T21 We want the y axis to be exited close to, but not right on the turning point
        // otherwise mspacman could collide with the turning points
        // left and right sides turning point for y are both 16
        // left side x position = 1, right side x position is 26
        // get mspacmans positions, relative to the turning point position, 
        // in proximity to the portal,( where it exists)
        if (transform.position.x < 2 && transform.position.y == 15.5)
        {
            // (transform) set mspacmans position to this new position, that avoids landing on a turning point 
            transform.position = new Vector2(24.5f, 15.5f);

            // mspacman procceds turning to the left, on other side of the portal
            pmMoveVect = new Vector2(-1, 0);

            // set our velocity
            rb.velocity = pmMoveVect * speed;
        }
        // T21 handle the other side of the portal
        else if (transform.position.x > 25 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(2f, 15.5f);
            pmMoveVect = new Vector2(1, 0);
            rb.velocity = pmMoveVect * speed;
        }

        // T20 If Pac-Man hits a Pill
        if (col.gameObject.tag == "Pill")
        {
            // call function and pass in the pill MsPacman collided with
            APillWasEaten(col);
        }
        // T22 If Pac-Man hits a Dot
        if (col.gameObject.tag == "Dot")
        {
            // T22 play Dot eaten Dynomite sound effect
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.Dynomite);

            // T22 call our redGhostScript, to turn ghost blue then back
            // T23 call our other 3 ghost scripts, to turn the ghosts blue then back
            redGhostScript.TurnGhostBlue();
            pinkGhostScript.TurnGhostBlue();
            blueGhostScript.TurnGhostBlue();
            orangeGhostScript.TurnGhostBlue();

            // T23 pass 50 to IncreaseTextUIScore(), when Dot is collided with
            IncreaseTextUIScore(50);

            // T23 destroy the dot
            Destroy(col.gameObject);
        }

        // T23 if pacman hits something, with the tag, Ghost
        if (col.gameObject.tag == "Ghost")
        {
            // get the name of the ghost, pacman collided with
            String ghostName = col.GetComponent<Collider2D>().gameObject.name;

            // get the audiosource so we can turn it (the audiosource) off, when pacman dies
            AudioSource audioSource = soundManager.GetComponent<AudioSource>();

            // if we collided with the red ghost
            if (ghostName == "RedGhost")
            {
                // and if the ghost is dark blue
                if (redGhostScript.isGhostBlue)
                {
                    // reset the redGhost in the cell, it was killed, pass in thepacman game object
                    redGhostScript.ResetGhostAfterEaten(gameObject);

                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.eatingGhost);

                    // increase Score for killed redGhost
                    IncreaseTextUIScore(200);
                }
                else
                {
                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.pacmanDies);

                    // shut off all audio sources
                    audioSource.Stop();

                    // destroy pacman object
                    Destroy(gameObject);
                }
            }
            // if we collided with the red ghost
            else if (ghostName == "PinkGhost")
            {
                // and if the ghost is dark blue
                if (pinkGhostScript.isGhostBlue)
                {
                    // reset the redGhost in the cell, it was killed, pass in thepacman game object
                    pinkGhostScript.ResetGhostAfterEaten(gameObject);

                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.eatingGhost);

                    // increase Score for killed redGhost
                    IncreaseTextUIScore(400);
                }
                else
                {
                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.pacmanDies);

                    // shut off all audio sources
                    audioSource.Stop();

                    // destroy pacman object
                    Destroy(gameObject);
                }
            }

            // if we collided with the blue ghost
            if (ghostName == "BlueGhost")
            {
                // and if the ghost is dark blue
                if (blueGhostScript.isGhostBlue)
                {
                    // reset the redGhost in the cell, it was killed, pass in thepacman game object
                    blueGhostScript.ResetGhostAfterEaten(gameObject);

                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.eatingGhost);

                    // increase Score for killed redGhost
                    IncreaseTextUIScore(600);
                }
                else
                {
                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.pacmanDies);

                    // shut off all audio sources
                    audioSource.Stop();

                    // destroy pacman object
                    Destroy(gameObject);
                }
            }
            // if we collided with the red ghost
            if (ghostName == "OrangeGhost")
            {
                // and if the ghost is dark blue
                if (orangeGhostScript.isGhostBlue)
                {
                    // reset the redGhost in the cell, it was killed, pass in thepacman game object
                    orangeGhostScript.ResetGhostAfterEaten(gameObject);

                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.eatingGhost);

                    // increase Score for killed redGhost
                    IncreaseTextUIScore(200);
                }
                else
                {
                    // play the ghost being eaten sound
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.pacmanDies);

                    // shut off all audio sources
                    audioSource.Stop();

                    // destroy pacman object
                    Destroy(gameObject);
                }
            }
        }
    }

    // T19 When Pac-Man hits a wall the animation will stop, and so will the animation itself
    // we will cal this function in fixed update
    void UpdateEatingAnimation()
    {
        // if the rigidbody2d (mspacman) velocity is stopped(zero)
        if(rb.velocity == Vector2.zero)
        {
            // get the animator and set it to false,
            // then get the sprite renderer and render our stopped sprite
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = pausedSprite;

            // T20 Pause Pac-Man eating sound, when MsPacman stops
            soundManager.PausePacman();
        }
        else
        {
            // otherwise keep the animation moving
            GetComponent<Animator>().enabled = true;

            // T20 Unpause Pac-Man eating sound, when he is moving again
            soundManager.UnPausePacman();
        }       
    }


    // T20 When a Pill object was collided with
    void APillWasEaten(Collider2D col)
    {
        // T20 call another function to increase the score, when pill is collided with
        // T23 pass 10 to IncreaseTextUIScore()
        IncreaseTextUIScore(50);

        // destroy the game object pill MsPacman collided with
        Destroy(col.gameObject);
    }


    // T20 increase the score when a pill is collided with 'eaten'  'sniffed up'
    //void IncreaseTextUIScore()
    // T23 added, the ability to pass in the point values for the score
    void IncreaseTextUIScore(int points)
    {
        // find the score UI component
        Text textUIComp = GameObject.Find("Score").GetComponent<Text>();

        // get the string stored inside of the Score and convert it to int
        int score = int.Parse(textUIComp.text);

        // increment the score
        score += points;

        // resave the updated score in the textUIComp score string
        textUIComp.text = score.ToString();
    }
}
 
 