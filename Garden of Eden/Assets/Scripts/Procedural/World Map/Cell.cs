using UnityEngine;
using System.Collections;

/* This scripts purpose is to be an object container for each cell in the world map.
 * This container will be used to store information about each cell into the library catalog.
 * The only methods that should be included in this script, are methods used to either 
 * retrieve or manipulate the data in this script. This will allow for easy threading on the logic
 * for the cells, and will make data organization much easier.
 */
public class Cell {

    #region Variables
    
    #region Location
    private int x; // The cells position x
    private float y = 0f; // The cells position y
    private int z; // The cells position Z
    #endregion

    #region Tiles
    private string tileTag = null; // The cell tile name
    private bool walkable = true; // Is the tile walkable by player and enemies
    #endregion

    #region Perlin Noise
    private float perlinStep; // The step rate of the perlin noise, Controls the smoothness between cells
    private float scale = 2.0f; // The emphisis added to the perlin step
    private bool affectedByPerlin = true; // The variable that determines if the Perlin Noise affects the cell
    public float chanceOfDisturbedTile = 0.05f; // The probability that a tile will spawn with a perlin step greater than default
    #endregion

    #region Properties
    public int X { get { return x; } set { x = value; } }
    public float Y { get { return y; } set { y = value; } }
    public int Z { get { return z; } set { z = value; } }
    public string TileTag { get { return tileTag; } set { tileTag = value; } }
    public bool Walkable { get { return walkable; } set { walkable = value; } }
    #endregion

    #endregion

    #region Constructor

    // Default Constructor
    public Cell ( int x, int z ) {
        this.x = x;
        this.z = z;
        if ( Random.value < chanceOfDisturbedTile )
            perlinStep = Random.Range( 0.028f, 0.029f );
        else
            perlinStep = 0.027f;
        y = (Mathf.PerlinNoise( x * perlinStep, z * perlinStep +0.001f ) * scale);
    }
    
    /// <summary>
    /// Perlin Constructor
    /// </summary>
    /// <param name="x">X Position</param>
    /// <param name="z">Z Position</param>
    /// <param name="scale">Scale of Perlin</param>
    /// <param name="minStep">Min Perlin Step: Dafault 0.027</param>
    /// <param name="maxStep">Max Perlin Step: Default 0.03</param>
    public Cell ( int x, int z , float scale, float minStep, float maxStep) {
        this.x = x;
        this.z = z;
        this.scale = scale;
        perlinStep = Random.Range( minStep, maxStep );
        y = (Mathf.PerlinNoise( x * perlinStep, z * perlinStep + 0.001f ) * this.scale);
    }
    #endregion

    #region Public Methods

    public Vector3 GetPosition () {
        Vector3 position = new Vector3( x, y, z );
        return position;
    }

    #endregion
}
