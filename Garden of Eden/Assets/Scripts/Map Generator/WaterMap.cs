using UnityEngine;
using System.Collections;
using System;

public class WaterMap : MonoBehaviour {

    #region Variables

    #region Public Variables
    [Space(10)]

    [Header("Map Properties")]
    [Range(0,100)]
    public int grassFillPercent; // The percentage of section to fill with grass
    public int width; // The overall width of section being built by number of tiles
    public int height; // The overall height of section being built by the number of tiles
    public int smoothingIterations; // The number of passes used to smooth map

    [Space(10)]

    [Header("Seed")]
    //Holds current value of seed if the player wants to keep the same seed. 
    //Also displays the seed for storage so you can use later.
    public string seed; 
    public bool usingRandomSeed; // Bool to control use of random seed 

    [Space(10)]

    public BuildTileMap builder;

    [Space(10)]
    #endregion

    #region Private Variables

    private int[,] map; // Array that will hold states of each tile

    #endregion

    #endregion

    #region Unity Events

    void Start () {
        GenerateMap();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Starting method called outside the Start() method
    /// </summary>
    private void GenerateMap () {
        //Instance the map object
        map = new int[width, height];
        // Fill the map object
        FillMap();
        //Smooth the map object
        for(int i = 0; i < smoothingIterations; i++ ) {
            SmoothMap();
        }
        builder.BuildMap( map, height, width );
    }

    /// <summary>
    /// This method is used to randomly fill the map with the 
    /// appropriate amount of grass by the probability desired.
    /// </summary>
    private void FillMap () {
        // If the user desires a random layout
        if ( usingRandomSeed ) {
            // Set the current time to a string to use as a random seed
            seed = System.DateTime.Now.ToString();
        }

        // Instance a RanGen with the time seed
        System.Random randomNumber = new System.Random( seed.GetHashCode() );

        // Iterate the each element in the array asigning it a random value of either 0 or 1
        for(int x = 0; x < width; x++ ) {
            for ( int z = 0; z < height; z++ ) {
                // If you are an edge peice, then make it a water tile
                if( x == 0 || x == width - 1 || z == 0 || z == height - 1 ) {
                    map[x, z] = 0;
                }
                else {
                    //Assign a random value
                    map[x, z] = (randomNumber.Next( 0, 100 ) < grassFillPercent) ? 1 : 0;
                }
            }
        }

        
    }


    private void SmoothMap () {
        for ( int x = 0; x < width; x++ ) {
            for ( int z = 0; z < height; z++ ) {
                int neighborWallTiles = GetSurroundingWallCount( x, z );

                if(neighborWallTiles > 4 ) {
                    map[x, z] = 1;
                }
                else if(neighborWallTiles < 4) {
                    map[x, z] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Looking at a 9x9 grid neighborhood of the current cell.
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridZ"></param>
    /// <returns></returns>
    private int GetSurroundingWallCount(int gridX, int gridZ ) {
        int wallCount = 0;
        for(int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++ ) {
            for ( int neighborZ = gridZ - 1; neighborZ <= gridZ + 1; neighborZ++ ) {
                if ( neighborX >= 0 && neighborX < width && neighborZ >= 0 && neighborZ < height ) {
                    if ( neighborX != gridX || neighborZ != gridZ ) {
                        wallCount += map[neighborX, neighborZ];
                    }
                }
            }
        }
        return wallCount;
    }
    #endregion
}
