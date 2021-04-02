using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// T18 get system library, Used to find out if Pac-Man hits a wall
using System;

public class MsPacman : MonoBehaviour
{
    // T18 MsPacmans properties; speed
    public float speed = 4f;

    // T18 move mrspacman using rigidbody 2d
    private Rigidbody2D rb;

    // T18 awake function, to create component(s) before they are to be used
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // T18 Start is called before the first frame update
    // get mspacman and start moving her to the left, at the default speed, when game begins
    void Start()
    {
        rb.velocity = new Vector2(-1, 0) * speed;
    }

    // T18 move mspacman around our screen, using rigidbodies 2d
    // we will take input from the user, that will instruct which direction to go in
    private void FixedUpdate()
    {
        // get mspacmans horizontal movement, x, for the a or d key to move left or right
        // get vertical movement , for y, keys W of S
        float horzMove = Input.GetAxisRaw("Horizontal");
        float vertMove = Input.GetAxisRaw("Vertical");

        // check if input a was pressed
        if(Input.GetKeyDown("a"))
        {
            // get mspacmans rigidbody make her velocity go left, x is horzMove, y is 0
            rb.velocity = new Vector2(horzMove, 0) * speed;

            // make mspacman face left, using her sprite
            transform.localScale = new Vector2(1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0,0);
        }
        // check if its another input option for horizontal
        else if (Input.GetKeyDown("d"))
        {
            // get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
            rb.velocity = new Vector2(horzMove, 0) * speed;

            // make mspacman face right, using her sprite, she faces left by default
            transform.localScale = new Vector2(-1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        // check if its another input option, vertical up
        else if (Input.GetKeyDown("w"))
        {
            // get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
            rb.velocity = new Vector2(0, vertMove) * speed;

            // make mspacman face right, using her sprite, she faces left by default
            transform.localScale = new Vector2(1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        else if (Input.GetKeyDown("s"))
        {
            // get mspacmans rigidbody make her velocity go right, x is horzMove, y is 0
            rb.velocity = new Vector2(0, vertMove) * speed;

            // make mspacman face right, using her sprite, she faces left by default
            transform.localScale = new Vector2(1, 1);
            // set rotation to its default
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }
}
