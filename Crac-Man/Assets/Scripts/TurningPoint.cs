using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour
{

    // T18 in unity enter the amount of turning Points are next, in a path it can move towards, from the current turning Point 
    // (any where from 2 - 4 turning point entry fields, will be generated, depending on the amount the user enters)
    public TurningPoint[] nextPoints;

    // T18 in unity, drag the turning points, into this field; that are the ones that are possibly the next turning point, 
    // to this current Point. the related Points to the current Point will be stored in this vector2 array.
    public Vector2[] vectToNextPoint;

    // Use this for initialization
    void Start()
    {
        // this will store the x,y movement for givin point to on of it turnpoints it can go to
        // Initialize the array that will hold the vector to each nearby TurningPoint, it can move towards, 
        // for each next point, left (-1,0), right(1,0), up(0,1) or down(0.-1), 
        // sometimes all four directions, depending on the position of the selected Point
        // this array is the same size as the lenght of the nextPoint array
        vectToNextPoint = new Vector2[nextPoints.Length];

        for (int i = 0; i < nextPoints.Length; i++)
        {
            // Get each point so we can find its position
            // in relation to the current TurningPoint
            TurningPoint nextPoint = nextPoints[i];

            // Get the Vector to the next TurningPoint
            // Returns (1, 0) for right, (0, -1) for down, etc.
            Vector2 pointVect = nextPoint.transform.localPosition - transform.localPosition;

            // Store vector to Vector2 array
            // Without normalized the values wouldn't be 0, 1, or -1, it forces larger numbers down to 1 but keeps the sign thr same
            vectToNextPoint[i] = pointVect.normalized;
        }
    }
}
