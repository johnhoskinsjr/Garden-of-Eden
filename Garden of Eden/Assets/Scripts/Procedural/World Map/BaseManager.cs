using UnityEngine;
using System.Collections;

/* This script is a SingleTon Pattern Script.
 * 
 * The purpose of this script is to build the floor. The only logic in this class is determining 
 * when the player is moving, and which tiles to place based off players location. First we build the
 * initial base at load up for the player to start at. This can be very heavy on load up, and may 
 * require a loading screen. 
 * 
 * We check to see if the player is moving to know if we need to run logic for building tiles. If the player 
 * is not moving, then restrict the coroutine checking players position. If the player is moving 
 * the check much more frquently, but still slightly restricting the rate of checking so you aren't
 * running logic every frame.
 * 
 * When building the map, first call the world map library to see if the cell has already been built and have the
 * cell object returned back. Then we call the warehouse manager and have a prefab cell gameobject
 * already instantiated returned back to you. Assign the properties of the cell gameobject. Then we ask the 
 * warehouse manager for a tile passing the cell object to him for the booking class to record locations of
 * neighbors. the warehouse manager return a already instantiated tile. We then set all the properties of the tile.
 */

public class BaseManager : MonoBehaviour {

    #region Variables

    #region Private
    private GameObject cameraObj; // The variable that hold reference to the player object
    private static BaseManager instance; // The Singleton Instance
    private GameObject parent; // The variable that holds the parent for tiles
    private IEnumerator buildBase; // The reference to the coroutine for building base
    #endregion

    #region Public
    //These number are the difference from the player position
    public int zTop = 30; // The max height for the tiles in the positive z direction
    public int zBottom = 10; // The max height for the tiles in the negative z direction
    public int xMax = 10; // The max width of tile in either x direction
    public GameObject cellPrefab; // The prefab of cell for instantiating when pool is empty
    #endregion

    #region Properties
    public static BaseManager Instance { get { return instance; } }
    public int ZTop { get { return zTop; } }
    public int ZBottom { get { return zBottom; } }
    public int XMax { get { return xMax; } }
    #endregion

    #endregion

    #region Unity Events

    void Awake () {

        // Instance Check for Singleton Pattern
        if ( instance == null )
            instance = this;
    }

    // Use this for initialization
    void Start () {

        // Instance player gameobject
        cameraObj = GameObject.FindGameObjectWithTag( "MainCamera" );

        // Start the coroutine that runs the whole time active
        StartCoroutine( CheckPlayerPosition() );

        //Instance parent gameobject
        parent = GameObject.FindGameObjectWithTag( "Cell_Pool" );

        // The instance of the coroutine for build base
        buildBase = CheckPlayerPosition();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method builds the starting area for the world map.
    /// It is called only once per play session.
    /// </summary>
    private void BuildBase () {
        // Increment by 2 because tile are 4x4
        for ( int _x = -xMax; _x <= xMax; _x = _x + 2 ) {
            for ( int _z = -zBottom; _z <= zTop; _z = _z + 2 ) {

                // Instance cell to be turned on
                Cell cell = new Cell( _x, _z );

                /* Get the cells actual cords
                * This is really only used to set y value
                * Since the x and z are predetermined
                */
                cell = WorldMapLibrary.Instance.GetCellLocation( cell );
                Vector3 cellPos = new Vector3( cell.X, cell.Y, cell.Z );
                TurnOnTile( cell, cellPos );
            }
        }
        // Add a base finished event
        WorldEventsAndDelegates.Instance.BaseComplete();
    }

    /// <summary>
    /// This method is called everytime a new tile
    /// is need to be placed in the world map.
    /// </summary>
    /// <param name="cell">The cell the is being turned on.</param>
    private void TurnOnTile ( Cell cell, Vector3 cellPos ) {
        // Get a prefab cell obj that is already placed in scene at new location
        GameObject cellObj = TurnOnCell( cellPos );
    }

    /// <summary>
    /// The method will be called when ever a 
    /// script wants a prefab cell object already 
    /// instantiated.
    /// </summary>
    /// <returns>A prefab cell gameobject</returns>
    public GameObject TurnOnCell ( Vector3 cellPos ) {
        // If cell pool is empty
        GameObject cell = Instantiate( cellPrefab, cellPos, Quaternion.identity ) as GameObject;
        // Set the cell prefabs parent to be the cell gameobject in scene under the warehouse gameobject
        cell.transform.parent = parent.transform;
        cell.name = "Cell";
        return cell;
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// This coroutine is only used to 
    /// add a delay to the build base method.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckPlayerPosition () {
        // Break out of the initial frames
        yield return new WaitForSeconds( 0.02f );

        // Build the starting base
        BuildBase();

        StopCoroutine( buildBase );
    }

    #endregion
}
