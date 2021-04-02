using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    // T18 this array will eventually hold all of our game objects

    // T18 Create array that holds all TurningPoints transforms x,y
    // T18 array size determined by GridGB play area width (0-27) and height (0-30)
    public Transform[,] gBPoints = new Transform[27, 30];

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
    }

}
