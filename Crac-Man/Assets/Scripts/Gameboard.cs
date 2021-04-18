using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// to use floor
using System;

public class Gameboard : MonoBehaviour
{
    // T18 this array will eventually hold all of our game objects

    // T18 Create array that holds all TurningPoints transforms x,y
    // T18 array size determined by GridGB play area width (0-27) and height (0-30)
    public Transform[,] gBPoints = new Transform[27, 30];

    // T22 create another array, that will validate if it is a valid block to move in, or not; (the entire maze)
    // the arrays dimmensions are based on starting on the outside, of the blue maze border, at the bottom left [0,0]
    // to the far right bottom of the maze[1, 27.5], so we can allow for the pills space, as well as the maze borders
    // then the y axis is [0], to [30.5] to the top of the mazes right border.  We rounded the vectors high
    public bool[,] validBlock = new bool[28, 31];

    // T18 References the empty that contains all the points (empty object TurningPoints)
    // so we can cycle through all of the points, and put them in the array
    private GameObject turningPoints;

    // T18 Use this for initialization
    void Start()
    {

        // Get the Empty named TurningPoints
        turningPoints = GameObject.Find("TurningPoints");

        // Cycle through each point in that empty (turningPoint)
        // to put it in the array
        foreach (Transform point in turningPoints.transform)
        {

            // Get vector position of point
            Vector2 pos = point.position;

            // Put point in the array
            gBPoints[(int)pos.x, (int)pos.y] = point;
        }

        // T22  Add all valid traveling blocks to our validBlock array, for x and y's, and their ranges
        // using their pertinate function names, and passing in thier values
        AddYRowXRange(1, 1, 26);
        AddXColYRange(1, 1, 4);
        AddXColYRange(12, 1, 4);
        AddXColYRange(15, 1, 4);
        AddXColYRange(26, 1, 4);
        AddYRowXRange(4, 1, 6);
        AddYRowXRange(4, 9, 12);
        AddYRowXRange(4, 15, 18);
        AddYRowXRange(4, 21, 26);

        AddXColYRange(3, 4, 7);
        AddXColYRange(6, 4, 29);
        AddXColYRange(9, 4, 7);
        AddXColYRange(18, 4, 7);
        AddXColYRange(21, 4, 29);
        AddXColYRange(24, 4, 7);

        AddYRowXRange(7, 1, 3);
        AddYRowXRange(7, 6, 21);
        AddYRowXRange(7, 24, 26);

        AddXColYRange(1, 7, 10);
        AddXColYRange(12, 7, 10);
        AddXColYRange(15, 7, 10);
        AddXColYRange(26, 7, 10);

        AddXColYRange(9, 10, 19);
        AddXColYRange(18, 10, 19);

        AddYRowXRange(13, 9, 18);

        AddYRowXRange(16, 0, 9);
        AddYRowXRange(16, 18, 27);

        AddYRowXRange(19, 9, 18);

        AddXColYRange(12, 19, 22);
        AddXColYRange(15, 19, 22);

        AddYRowXRange(22, 1, 6);
        AddYRowXRange(22, 9, 12);
        AddYRowXRange(22, 15, 18);
        AddYRowXRange(22, 21, 26);

        AddXColYRange(1, 22, 29);
        AddXColYRange(9, 22, 25);
        AddXColYRange(18, 22, 25);
        AddXColYRange(26, 22, 29);

        AddYRowXRange(25, 1, 26);

        AddXColYRange(12, 25, 29);
        AddXColYRange(15, 25, 29);

        AddYRowXRange(29, 1, 12);
        AddYRowXRange(29, 15, 26);
    }

    // T22 we will create two validating functions, to validate the x, y rows and column areas,
    // within the validBlock array, where the ghosts and pacman, can travel, in the maze bounderies
    // for the rows, has a defined y vector, with a x range, of vectors
    void AddYRowXRange(int yRow, int xStart, int xEnd)
    {
        // cycle through the x range, to get all the x vectors
        for (int i = xStart; i <= xEnd;  i++)
        {
            // add these x vectors to our validBlock, as valid = true
            validBlock[i, yRow] = true;
        }
    }
    // T22 for the columns, has a defined x vector, with a y range, of vectors
    void AddXColYRange(int xColumn, int yStart, int yEnd)
    {
        // cycle through the y range, to get all the y vectors
        for (int i = yStart; i <= yEnd; i++)
        {
            // add these y vectors, and the defined x vectors to our validBlock, as valid = true
            validBlock[xColumn, i] = true;
        }
    }


    // T22 provides a way for other objects to call and see if there is a valid space here or not
    public bool IsValidSpace(float x, float y)
    {
        // take the value that is passed in, convert it from a float to a double, to get the floor version of it
        // then back into a double, as we did this the same way in a prior tut. in this series
        x = (float)Math.Floor(Convert.ToDouble(x));
        y = (float)Math.Floor(Convert.ToDouble(y));

        // we then convert the x and y to ints, add 1, and if it is a valid block, return true 
        if(validBlock[(int)x +1, (int)y + 1])
        {
            return true;
        }
        // if not valid block return false
        else
        {
            return false;
        }
    }
}
