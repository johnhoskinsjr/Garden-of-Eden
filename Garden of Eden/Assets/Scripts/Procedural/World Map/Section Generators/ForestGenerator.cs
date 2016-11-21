using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class ForestGenerator : MonoBehaviour {

    #region Variables

    #region Public
    public AnimationCurve fillCurve; // The curve used to determine the fill percent based on the size of the section
    private int cleaningCycles; // The number of cycles made by section smoother
    #endregion

    #region Private
    private int maxSizeX; // The maximum size either x can be
    private int minSizeX; // The minimum size either x can be
    private int maxSizeZ; // The maximum size either z can be
    private int minSizeZ; // The minimum size either z can be
    private Cell[,] sectionGrid; // The array that holds the cell objects
    private int width; // The random width of the section
    private int height; // The random height of the section
    private float fillPercent; // The variable to hold the random fill percent
    private Dictionary<int, Cell> section = new Dictionary<int, Cell>(); // The dictionary to hold the new section 
    private string tileName; // The variable that holds the type of forest  being built
    #endregion

    #region Properties
    public int MaxSizeX { get { return MaxSizeX; } set { MaxSizeX = value; } }
    public int MaxSizeZ { get { return MaxSizeZ; } set { MaxSizeZ = value; } }
    #endregion

    #endregion

    #region Unity Events

    //void Start () {
    //    GenerateForest();
    //}

    //void Update () {
    //    if ( Input.GetMouseButtonDown( 0 ) ) {
    //        GenerateForest();
    //    }
    //}

    #endregion

    #region Public Methods

    /// <summary>
    /// This method starts the process of 
    /// generating a forest for the map.
    /// </summary>
    public void GenerateForest () {
        // Determine a random height and width that initialize the section grid
        width = Random.Range( minSizeX, maxSizeX ) * 2;
        height = Random.Range( minSizeZ, maxSizeZ ) * 2;
        // Determine fill percent
        fillPercent = 0.72f - ((fillCurve.Evaluate( (width / 2) - 20 ) + fillCurve.Evaluate((height/ 2) - 20)) / 100);
        // Initialize the section grid with the random width and height
        sectionGrid = new Cell[width, height];
        // Start filling grid with cells
        RandomFillSection();

        // Smoothing Iterations
        for(int i = 0; i < cleaningCycles; i++ ) {
            Thread t1 = new Thread(() => OrganizeSection());
            t1.Start();
            t1.Join();
        }
        // Put the section in a dictionary
        Thread t2 = new Thread(() => BuildDictionary());
        t2.Start();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method fills the section grid with 
    /// random tiles and a tile border.
    /// </summary>
    private void RandomFillSection () {
        DetermineTypeOfForest();
        // For each x in the section grid
        for(int x = 0; x < width; x += 2 ) {
            // For each z in the section grid
            for ( int z = 0; z < height; z += 2 ) {
                // Create a new cell object with the given x and z values
                Cell cell = new Cell( x, z );
                // If the current cell is on the border, then make it a grass cell
                // Else determine a random tile based on probabilities
                if(x == 0 || x == width - 2 || z == 0 || z == height - 2 ) {
                    cell.TileTag = "Grass";
                }
                else {
                    cell.TileTag = (Random.value < fillPercent) ? tileName : "Grass";
                    if(cell.TileTag == tileName)
                        cell.Walkable = false; // If bush, then tile is not walkable
                }
                // Add the cell to the section grid array
                sectionGrid[x, z] = cell;
            }
        }
    }

    /// <summary>
    /// This method is called when its time to organize the sections
    /// </summary>
    private void OrganizeSection () {
        // For each x in the section grid
        for ( int x = 0; x < width; x += 2 ) {
            // For each z in the section grid
            for ( int z = 0; z < height; z += 2 ) {
                // Get the grass neighbor count
                int neighborGrassTiles = GetNeighborGrassCount( x, z );
                // Change tile logic
                if ( neighborGrassTiles > 4 )
                    sectionGrid[x, z].TileTag = "Grass";
                else if (neighborGrassTiles < 4 ) {
                    sectionGrid[x, z].TileTag = tileName;
                    sectionGrid[x, z].Walkable = false; // Bush tiles are not walkable
                }
                    
            }
        }
    }

    /// <summary>
    /// Get the number of tiles in the 
    /// neighborhood that are grass tiles.
    /// </summary>
    /// <param name="gridX">Current cell x</param>
    /// <param name="gridZ">Current cell z</param>
    /// <returns>The number of grass neighbors</returns>
    private int GetNeighborGrassCount (int gridX, int gridZ) {
        int grassCount = 0;
        // For each x in the neighborhood
        for(int neighborX = gridX - 2; neighborX <= gridX + 2; neighborX += 2) {
            // For each z in the neighborhood
            for ( int neighborZ = gridZ - 2; neighborZ <= gridZ + 2; neighborZ += 2 ) {
                // Make sure that we stay on the grid
                if (neighborX >= 0 && neighborX < width && neighborZ >= 0 && neighborZ < height) { 
                    // If we are not looking at the center tile and the tile tag is grass
                    if ( (neighborX != gridX || neighborZ != gridZ) && sectionGrid[neighborX, neighborZ].TileTag == "Grass" ) {
                        // Increment the grass count
                        grassCount++;
                    }
                }
                // If we are off the grid, still increment grass count to help promote grass tiles on the edge of section
                else {
                    grassCount++;
                }
            }
        }
        return grassCount;
    }

    /// <summary>
    /// This method takes all the cells in the 
    /// 2D array and adds them to the dictionary.
    /// </summary>
    private void BuildDictionary () {
        // For each x in the section grid
        for ( int x = 0; x < width; x += 2 ) {
            // For each z in the section grid
            for ( int z = 0; z < height; z += 2 ) {
                // Grab each cell from the section grid 2D array and add them to dictionary
                Cell cell = sectionGrid[x, z];
                string id = string.Format( "{0}, {1}", cell.X, cell.Z );

                section.Add( id.GetHashCode(), cell );
                
            }
        }
    }

    /// <summary>
    /// This method is used to randomly determine 
    /// what type of forest tiles will be used.
    /// </summary>
    private void DetermineTypeOfForest () {
        float randomValue = Random.value;
        if( randomValue < 1 ) {
            tileName = "Bush";
        }
        else if ( randomValue < 0.66f ) {
            tileName = "Tree";
        }
    }

    #endregion

    #region Gizmos

    // This method draws gizmos on the screen for easy building of sections.
    //void OnDrawGizmos () {
    //    if(sectionGrid != null ) {
    //        for ( int x = 0; x < width; x += 2 ) {
    //            for ( int z = 0; z < height; z += 2 ) {
    //                // Make bush tiles black and grass tiles white
    //                Gizmos.color = (sectionGrid[x, z].TileTag == "Bush") ? Color.black : Color.white;
    //                Vector3 pos = new Vector3( -width / 2 + x + 1, 0, -height / 2 + z + 1 );
    //                Gizmos.DrawCube(pos, Vector3.one * 2);
    //            }
    //        }
    //    }
    //}

    #endregion
}
