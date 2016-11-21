using UnityEngine;

public class LineSegment 
{
    #region Variables
    private Vector3 movingVector; // The position of the moving object
    private Vector3 destinationVector; // The position of the destination object
    private Transform movingObject; // The transform object for checking moving objects new position
    private Transform movingDestinationObject; // The transform for the moving destination object
    private float slope; // The slope of the line segment
    private float zIntercept; // The y-intercept of the line segment
    #endregion

    #region Constructors

    /// <summary>
    /// Creates a line segment from the moving object
    /// to the moving destination object. 
    /// </summary>
    /// <param name="movingObject">The object moving towards destination</param>
    /// <param name="movingDestinationObject">The destination transform</param>
    public LineSegment (Transform movingObject, Transform movingDestinationObject ) {
        movingVector = movingObject.position;
        destinationVector = movingDestinationObject.position;
        this.movingObject = movingObject;
        this.movingDestinationObject = movingDestinationObject;

        FindSlope();
    }

    /// <summary>
    /// Creates a line segment that has a moving object
    /// and a static vector.
    /// </summary>
    /// <param name="movingObject">The transfor of moving object</param>
    /// <param name="destinationVector">The static position</param>
    public LineSegment(Transform movingObject, Vector3 destinationVector ) {
        movingVector = movingObject.position;
        this.destinationVector = destinationVector;
        this.movingObject = movingObject;

        FindSlope();
    }

    /// <summary>
    /// Creates a line segment from two static vectors
    /// </summary>
    /// <param name="firstVector">The first vector</param>
    /// <param name="secondVector">The second vector</param>
    public LineSegment(Vector3 firstVector, Vector3 secondVector ) {
        movingVector = firstVector;
        destinationVector = secondVector;

        FindSlope();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method returns the x value on a line
    /// at the position of a given y value.
    /// </summary>
    /// <param name="z">The y value on the line segment</param>
    /// <returns>The x value on the line segment</returns>
    public float GetX(float z ) {
        float x = 0;

        // Check all value of line segment for any changes
        CheckNewPositions();

        // Linear function for x
        x = (zIntercept - z) / -slope;
        return x;
    }

    /// <summary>
    /// This method returns the z value on a line
    /// at the position of a given x value.
    /// </summary>
    /// <param name="x">The x value on the line segment</param>
    /// <returns>The z value on the line segment</returns>
    public float GetZ(float x ) {
        float z = 0;

        // Check all value of line segment for any changes
        CheckNewPositions();

        // Linear function for y
        z = (slope * x) + zIntercept;
        return z;
    }

    /// <summary>
    /// This method returns the y value on a line
    /// at the position of a given x value.
    /// </summary>
    /// <param name="x">The x value on the line segment</param>
    /// <returns>The y value on the line segment</returns>
    public float GetY(float x ) {
        float y = 0;

        // Check all value of line segment for any changes
        CheckNewPositions();

        // Linear function for y
        y = (slope * x) + zIntercept;
        return y;
    }

    /// <summary>
    /// Get the distance of the moving vector to
    /// the destination vector.
    /// </summary>
    /// <returns>The length of the line as a float</returns>
    public float GetDistance () {
        float magnitude = 0;

        if(movingVector == destinationVector ) 
            return 0.0f;
       
        // Check all value of line segment for any changes
        CheckNewPositions();

        // Find the magnitude of the line
        magnitude = Mathf.Sqrt(Mathf.Pow(2 , movingVector.x - destinationVector.x) + Mathf.Pow(2 , movingVector.z - destinationVector.z));

        // Return the magnitude as the distance between vectors
        return magnitude;
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Checks to see if either moving object has moved to a new
    /// position and if so then change value of position vectors
    /// </summary>
    private void CheckNewPositions () {
        // If the moving object is in a new position then change
        // the value of its vector
        if ( movingObject != null && movingObject.position != movingVector ) 
            movingVector = movingObject.position;
        
        // If the moving destination object is in a new position 
        // then change the value of its vector
        if ( movingDestinationObject != null && movingDestinationObject.position != destinationVector ) 
            destinationVector = movingDestinationObject.position;
       
        // If either of the two vectors were changed, then change the slope
        if(( movingDestinationObject != null && movingDestinationObject.position != destinationVector ) || ( movingObject != null && movingObject.position != movingVector) ) 
            FindSlope();
        
    }

    /// <summary>
    /// Finds the slope and y-intercept of the line segment
    /// </summary>
    private void FindSlope () {
        // Determine the slope of the line segment
        slope = (movingVector.z - destinationVector.z) / (movingVector.x - destinationVector.x);

        // Determine the y-intercept of the line segment
        zIntercept = -(slope * movingVector.x) + movingVector.z;

        // Check for NaN values
        if( slope != slope ) {
            slope = 0;
        }
        if(zIntercept != zIntercept ) {
            zIntercept = 0;
        }
    }

    #endregion
}
