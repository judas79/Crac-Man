using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to find out if Ghost hits a wall
using System;

// 2. Create Empty Ghosts -> Move it to X -.5 Y -.49
// Drag SpriteSheet_0 in and name it RedGhost
// Turn it into a Prefab -> Move to X 14 Y 19
// Drag other Ghosts into the sprite block in Inspector

// 2. Add Rigidbody 2D - Mass .001 - Gravity Scale 0
// Collision Continuous - Freeze Rotation Z

// 2. Add Box Collider Size .88 -> Check Is Trigger

public class Ghost : MonoBehaviour
{
    // T21 properties for our ghosts speed
    public float speed = 4f;

    // Used to move the Ghost
    private Rigidbody2D rb;

    // T21 animation sprites, for when the ghost look and go in one of the fur directions
    public Sprite lookLeftSprite;
    public Sprite lookRightSprite;
    public Sprite lookUpSprite;
    public Sprite lookDownSprite;

    // T21 we will be moving the ghosts around the screen, from one vector, to another vector
    // An array of destinations the Ghosts will move toward while on patrol
    // we will start from the center and got to the four corners of our maze
    Vector2[] destinations = new Vector2[]
    {
        // we used our dots and turnpoints to get these vectors.
        // top left corner vector = 1, 29, we will add all four corners, and mid point, to the array
        // this will also be use to track down msPacman
        new Vector2( 1, 29 ),
        new Vector2( 26, 29 ),
        new Vector2( 26, 1 ),
        new Vector2( 1, 1 ),
        new Vector2( 6, 16 )
    };

    // T21 for the array, we will also need a destination index.
    // The index to the first destination the Ghost aims at
    // Each Ghost aims at a different one
    public int destinationIndex;

    // T21 Direction Ghost will move when it hits a Point, which will change constantly
    Vector2 moveVect;

    // T22 in order to change between the 4 sprites, for the RedGhost, making it animated,
    // we have to reference our sprite renderer
    public SpriteRenderer sr;

    // T22 track To See if our ghost is blue, or not, 
    // so sprite animations are not called if ghost is blue, and makes a turn
    //bool isGhostBlue = false;
    // T23 made public, so we can have access to when it is blue, or not
    public bool isGhostBlue = false;

    // T22 store the blue version of our ghost
    public Sprite blueGhost;

    // T23 for the wait time, before ghost start comming out, and move about the screen
    public float startWaitTime = 0;

    // T23 wait time, after ghost has been eaten, the comes out on the screen again
    public float waitTimeAfterEaten = 4f;

    // T23 define where th ghost are located, before they start, (in the central cell box)
    public float CellXPos = 0;
    public float CellYPos = 0;

    // T21 Add Rigidbody to Ghosts, before they start being used
    void Awake()
    {
        // Get Ghost Rigidbody created
        rb = GetComponent<Rigidbody2D>();

        // T22 create the sr spriteRenderer, which is attached to our RedGhost, and will be used to animate it
        // by changing the sprites attached to our ghost
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // T23 Invoke ghosts, to start them at different times
    void Start()
    {
        // T23 ghosts starting time specified in Unity property settings, startWaitTime
        // to start different ghost a specified time.
        Invoke("StartMoving", startWaitTime);
    }

    // T23 everything in the Start() function was moved into this function, including comments
    // the position where the ghosts start was added this lesson
    // T21 Start is called before the first frame update
    // get the x of the destination turning point at the start
    void StartMoving()
    {
        // T23 position where the ghosts will start moving, within the maze, but not on top of a Point
        transform.position = new Vector2(13.5f, 18.5f);

        // T21 have option of the ghost going left or right, when it first comes on the screen
        float xDest = destinations[destinationIndex].x;

        // T21 If Ghost x pos > destination x
        if (transform.position.x > xDest)
        {

            // T21 Move the Ghost left
            rb.velocity = new Vector2(-1, 0) * speed;
        }
        else
        {
            // T21 Move the Ghost right
            rb.velocity = new Vector2(1, 0) * speed;
        }

    }

    // T21 We will not be using update, but instead, use the turningPoint and
    // base everything on collisions and reactions to said collisions, using isTrigger
    // to detect every trigger point collision of, pivot points, turning points, mspacman, with our ghost.
    void OnTriggerEnter2D(Collider2D col)
    {

        // T21 verify If the Ghost hit a Point
        if (col.gameObject.tag == "Point")
        {
            // get the vector the ghost wants to move in, from GetNewDirection function
            moveVect = GetNewDirection(col.transform.position);

            // position the ghosts in the middle, but between Point(s)
            // keeping in mind, the int array, uses integers
            // this will take our ghost integer position in the grid,
            // that collided with a Point and add .5, to keep it off the point, in the middle of the screen
            transform.position = new Vector2((int)col.transform.position.x + .5f,
                (int)col.transform.position.y + .5f);

            // if it is equal to 2 send back an error message, because vector wise it should be 1, -1, or 0
            // if not a 2, change the direction of the ghost
            if (moveVect.x != 2)
            {
                // T22 change the sprite associated with our redghost
                // if the ghost is moving to the right, left, up, or down
                if (moveVect == Vector2.right)
                {
                    // T21 Changes the direction of the Ghost
                    rb.velocity = moveVect * speed;

                    // T22 default IsGhostBlue is false, So if ghost is not blue, we can use our sprite animation
                    if (!isGhostBlue)
                    {
                        sr.sprite = lookRightSprite;
                    }

                    // T22 change our sprite so it looks in the cooriponding direction, that its moving
                    // T22 moved above into if statement
                    //sr.sprite = lookRightSprite;
                }
                else if (moveVect == Vector2.left)
                {
                    rb.velocity = moveVect * speed;
                    if (!isGhostBlue)
                    {
                        sr.sprite = lookLeftSprite;
                    }
                    //sr.sprite = lookLeftSprite;
                }
                else if (moveVect == Vector2.up)
                {
                    rb.velocity = moveVect * speed;
                    if (!isGhostBlue)
                    {
                        sr.sprite = lookUpSprite;
                    }
                    //sr.sprite = lookUpSprite;
                }
                else if (moveVect == Vector2.down)
                {
                    rb.velocity = moveVect * speed;
                    if (!isGhostBlue)
                    {
                        sr.sprite = lookDownSprite;
                    }
                    //sr.sprite = lookDownSprite;
                }

                // T21 Changes the direction of the Ghost
                // T22 moved into if statements, above
                //rb.velocity = moveVect * speed;
            }

        }

        // T22 verify If our ghost collides with a Portal Point, 
        Vector2 ghostMoveVect = new Vector2(0, 0);

        // T22 if its the Point next to the entrance to  the Left portal 
        if (transform.position.x < 2 && transform.position.y == 15.5)
        {
            // T22 change the position of the ghost, to the other end of the portal
            transform.position = new Vector2(24.5f, 15.5f);

            // T22 make sure the ghost continues moving left, after jumping to the right portal
            ghostMoveVect = new Vector2(-1, 0);

            // T22 set the ghosts velocity, by its rigidbody
            rb.velocity = ghostMoveVect * speed;
        }
        // T22 if its the Point next to the entrance to  the Right portal 
        else if (transform.position.x > 25 && transform.position.y == 15.5)
        {
            // T22 change the position of the ghost, to the other side of the portal
            transform.position = new Vector2(2f, 15.5f);

            // T22 make sure the ghost continues moving right, after jumping to the right portal
            ghostMoveVect = new Vector2(1, 0);

            // T22 set the ghosts velocity, by its rigidbody
            rb.velocity = ghostMoveVect * speed;
        }
    }


    // T23 setup our pacman, so it can, have the ability of being eaten, later, in the following two functions.
    GameObject pacmanGO = null;

    // T23 recieves the pacman game object
    public void ResetGhostAfterEaten(GameObject pacman)
    {
        // move the ghosts to the predifined cell position, after eaten, in the cell
        transform.position = new Vector2(CellXPos, CellYPos);

        // make the ghost imobile, while in the cell
        rb.velocity = Vector2.zero;

        // assign the passed in pacman, to our pacman object, so we can use it outside of this function
        pacmanGO = pacman;

        // invoke the function for the ghosts to start comming out again, 
        // after waiting 4 seconds (the value of waitTimeAfterEaten)
        Invoke("StartMovingAfterEaten", waitTimeAfterEaten);
    }

    // T23 start moving the ghost again at thier start moving position
    void StartMovingAfterEaten()
    {
        transform.position = new Vector2(13.5f, 18.5f);

        // T21 have option of the ghost going left or right, when it first comes on the screen
        float xDest = destinations[destinationIndex].x;

        // T21 If Ghost x pos > destination x
        if (transform.position.x > xDest)
        {

            // T21 Move the Ghost left
            rb.velocity = new Vector2(-1, 0) * speed;
        }
        else
        {
            // T21 Move the Ghost right
            rb.velocity = new Vector2(1, 0) * speed;
        }
    }


    // T21 this will handle all the ghosts movement, receive a vector2 and return a vector2
    Vector2 GetNewDirection(Vector2 pointVect)
    {
        // if the ghost hits a Point, get x y position minus the .5 we added to it, above in the onTrigger function
        // convert to float, then round down, the x and y positions, geting rid of the .5
        float xPos = (float)Math.Floor(Convert.ToDouble(transform.position.x));
        float yPos = (float)Math.Floor(Convert.ToDouble(transform.position.y));

        // if the pivot points colides with soemthing, get it x y positions, and get rid of the .5
        pointVect.x = (float)Math.Floor(Convert.ToDouble(pointVect.x));
        pointVect.y = (float)Math.Floor(Convert.ToDouble(pointVect.y));

        // T21 get the destination that we want the ghost to go
        Vector2 dest = destinations[destinationIndex];

        // T23 after pacman eatsghost, ghost now, go after pacman
        if(pacmanGO != null)
        {
            dest = pacmanGO.transform.position;
        }

        // T21 see if the ghost reached his destination and if so incrment to go to the next destination
        // +1 because there is a discrpency between the onscreen grid and the array grid, of 1
        if (((pointVect.x + 1) == dest.x) && ((pointVect.y + 1) == dest.y))
        {
            // set the destinationIndex to 0 if it is equal to 4, because there are 4 members in the array, using ternary operator ?
            // if not 4 then set the destination index to whatever it is + 1
            destinationIndex = (destinationIndex == 4) ? 0 :
                destinationIndex + 1;

            // show in the console that a new destination has been set
            Debug.Log("NEW DESTINATION " + destinations[destinationIndex]);
        }
        // T21 we will also update our destinations
        dest = destinations[destinationIndex];

        // T23 after pacman eatsghost, ghost now, go after pacman
        if (pacmanGO != null)
        {
            dest = pacmanGO.transform.position;
        }

        // T21 Will hold the new direction Ms. Pac-Man will move to
        // setup so if the vector is 2, that we will be notified above in the OnTrigger function
        Vector2 newDir = new Vector2(2, 0);

        // property that Holds previous direction traveled
        Vector2 prevDir = rb.velocity.normalized;

        // T21 property that Holds opposite of previous direction traveled
        Vector2 oppPrevDir = prevDir * -1;

        // make the code more redable by defining the up, down right left directions
        Vector2 goRight = new Vector2(1, 0);
        Vector2 goLeft = new Vector2(-1, 0);
        Vector2 goUp = new Vector2(0, 1);
        Vector2 goDown = new Vector2(0, -1);

        // Distance from destinations is used to decide if I
        // should move based off of which of X or Y is closest
        float destXDist = dest.x - xPos;
        float destYDist = dest.y - yPos;

        Debug.Log("GET NEW DIRECTION");
        Debug.Log("X POSITION " + xPos);
        Debug.Log("Y POSITION " + yPos);
        Debug.Log("DEST X POSITION " + dest.x);
        Debug.Log("DEST Y POSITION " + dest.y);
        Debug.Log("POINT X POSITION " + pointVect.x);
        Debug.Log("POINT Y POSITION " + pointVect.y);

        // This section will be dedicated to the AI coraling of MsPacman by the ghosts.
        // We will do things like, avoid the porals, and go right or left dependant on the nearest route for the ghosts to get MsPacman.

        // Upper Left

        // Keeps Ghost from going toward the portal, and setsup going towards the top left area of the maze
        // after calculating destXDist and destYDist, by subtracting the ghosts position, from the final destination position
        // Then the x vector (less than 0) and the y vector (greater than 0) will be located in the top left
        // so this will point the ghost to move to the left top destination.
        // Example: The ghost position is, 14, 19, our top left position is 1, 29, destXDist = 1 - 14, which is -13, 
        // which is destYDist > 0; for the y destination, destYDist = 29 - 19,  which equals 10, which in our if statement (destYDist > 0)
        // which satisfies both stipulations in the if statement, to move the ghost, top left.

        if (destYDist > 0 && destXDist < 0)
        {
            // if we hit the Point, for the portal, ignore it, 
            if (pointVect.x == 5 && pointVect.y == 15)
            {
                // go up instead, if it can, and do not backtrack where you just came from, no repeating, back and forth mvoement
                if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    // this will be returned later on, if all is true
                    newDir = goUp;
                }

                // Pick Up or Left depending whether I'm closest to
                // the X or Y destination
            }
            else if (destYDist > destXDist)
            {
                // can I go left, without back tracking in the direction I just moved in
                if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    // return this, if we can go left
                    newDir = goLeft;
                }
                // can I go up, without back tracking in the direction I just moved in
                else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                // T21 we do NOT need this,but it doesn't hurt anyhing in th code
                // can I go in the opposite direction, yes backtrack, as final heading
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }
            }

            // pick 'up' or 'left' if our X destination distance is closer than our Y destination distance, is to our final destination 
            else if (destYDist < destXDist)
            {

                if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }

        }

        // Upper Right
        // T21 this section will test for when the y's are equal, and we want to move to the top right
        // just like we did above, but in a different sequence of if and if else statements, for the AI movement
        if (destYDist > 0 && destXDist > 0)
        {

            if (destYDist > destXDist)
            {

                if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }
            else if (destYDist < destXDist)
            {

                if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }

        }

        // Lower Right

        if (destYDist < 0 && destXDist > 0)
        {

            if (destYDist > destXDist)
            {

                if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }
            else if (destYDist < destXDist)
            {

                if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }

        }

        // Lower Left

        if (destYDist < 0 && destXDist < 0)
        {

            if (destYDist > destXDist)
            {

                if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }
            else if (destYDist < destXDist)
            {

                if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
                {
                    newDir = goDown;
                }
                else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
                {
                    newDir = goLeft;
                }
                else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
                {
                    newDir = goRight;
                }
                else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
                {
                    newDir = goUp;
                }
                else if (canIMoveInDirection(oppPrevDir, pointVect))
                {
                    newDir = oppPrevDir;
                }

            }

        }

        // Ys Equal and Want to go Right
        // Done because the above don't test for if Xs & Ys are equal

        if ((int)(dest.y) == (int)(yPos)
            && destXDist > 0)
        {

            Debug.Log("5");

            if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
            {
                newDir = goRight;
            }
            else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
            {
                newDir = goUp;
            }
            else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
            {
                newDir = goDown;
            }
            else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
            {
                newDir = goLeft;
            }

        }

        // Ys Equal and Want to go Left

        if ((int)(dest.y) == (int)(yPos)
            && destXDist < 0)
        {

            Debug.Log("6");

            if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
            {
                newDir = goLeft;
            }
            else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
            {
                newDir = goUp;
            }
            else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
            {
                newDir = goDown;
            }
            else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
            {
                newDir = goRight;
            }

        }

        // Xs Equal and Want to go Up

        if ((int)(dest.x) == (int)(xPos)
            && destYDist > 0)
        {

            Debug.Log("7");

            if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
            {
                newDir = goUp;
            }
            else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
            {
                newDir = goRight;
            }
            else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
            {
                newDir = goLeft;
            }
            else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
            {
                newDir = goDown;
            }
        }


        // Xs Equal and Want to go Down

        if ((int)(dest.x) == (int)(xPos)
            && destYDist < 0)
        {

            Debug.Log("8");

            if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir)
            {
                newDir = goDown;
            }
            else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir)
            {
                newDir = goRight;
            }
            else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir)
            {
                newDir = goLeft;
            }
            else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir)
            {
                newDir = goUp;
            }

        }
        return newDir;
    }

    // Gets a chosen direction and searches for it in the array
    // that holds references to all the pivot points
    bool canIMoveInDirection(Vector2 dir, Vector2 pointVect)
    {

        // Ghost position
        Vector2 pos = transform.position;

        // Used to find if there a Point in the array or null
        Transform point = GameObject.Find("GBGrid").GetComponent<Gameboard>().gBPoints[(int)pointVect.x, (int)pointVect.y];

        // Did I find a Point here?
        if (point != null)
        {

            // Get Points associated GameObject
            GameObject pointGO = point.gameObject;

            // Get vectToNextPoint array attached to the Point
            Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint>().vectToNextPoint;

            Debug.Log("Checking Vects " + dir);

            // Cycle through the attached vectToNextPoint array
            foreach (Vector2 vect in vectToNextPoint)
            {

                Debug.Log("Check " + vect);

                if (vect == dir)
                {
                    return true;
                }
            }
        }
        return false;
    }


    // T22 create a way to be able, for outside objects, to change our ghost blue, for a couple of seconds
    public void TurnGhostBlue()
    {
        // T22 call our Ienumerator co-routine
        StartCoroutine(TurnGhostBlueAndBack());
    }

    // T22 start go routine to turn our ghost back from being blue
    IEnumerator TurnGhostBlueAndBack()
    {
        // T22
        isGhostBlue = true;

        // T22 change the sprite for out ghost, by calling sprite renderer
        sr.sprite = blueGhost;

        // T22 wait 6 seconds before turning our ghost back to its normal color
        yield return new WaitForSeconds(6.6f);
        isGhostBlue = false;
    }
}