using UnityEngine;
using System.Collections.Generic;
using System.Threading;

/* This script acts the the card catalog system for the library. Its has a dictionary the has a list of all cell 
 * that have already be calculated and spawn. This will prevent from running new logic on cell that have already 
 * been created. This is how we accomplish saving the created world so it remains persistent across every play session
 * for the character. 
 * 
 * This script also manages the diction that holds all the active tile on the current game scene.
 * This list is constantly updated. Every time a new cell tile is spawn it puts a reference to 
 * its vector code in a dictionary. This way other tile cells can check if the tile is already active
 * then skip logic. When a tile is disabled it is removed from this dictionary.
 * 
 * This script is called when the cell manager wants to spawn its tile, and set it's location. When this happens
 * this script will first check if it's already active. Then it will check if it has been created and catalogged.
 * If it is cataloged, create a cell object with the location and tile, catalog it, then return info to cell manager.
 */ 
public class WorldMapLibrary : MonoBehaviour {

    #region Variables

    #region Private Variables
    // This object contains the full list of all cell positions and tile peices for the world map
    private static Dictionary<int, Cell> cellCatalog = new Dictionary<int, Cell>();

    // This dictionary object is a list of all cells currently active in world map
    private static Dictionary<int, Cell> activeLocations = new Dictionary<int, Cell>();

    // This dictionary records which tiles where placed on which cells
    private static Dictionary<int, GameObject> tileCatalog = new Dictionary<int, GameObject>();

    // Thread Access Lockers
    private static readonly object _addLocker = new object(); // The locker id object for the add to dictionary method
    private static readonly object _activeLocker = new object(); // The locker id object for the add to active diction method
    private static readonly object _tagLocker = new object(); // The locker id object for the add to active diction method
    private static WorldMapLibrary instance = null; // Singleton Instance
    #endregion

    #region Public Variables
    public bool isBaseComplete = false; // The variable that holds if the base has finished building
    #endregion

    #region Properties
    public static WorldMapLibrary Instance { get { return instance; } }
    #endregion

    #endregion

    #region Unity Events

    void Awake () {
        // Instance Check for Singleton Pattern
        if ( instance == null )
            instance = this;
        else if ( instance != this )
            Destroy( gameObject );
        // Make persistent
        DontDestroyOnLoad( gameObject );
    }

    void Start () {
        WorldEventsAndDelegates.Instance.BaseCompleted += OnBaseCompleted;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Call when a script wants to know its information
    /// of its position and tile in world map
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public Cell GetCellLocation ( Cell location ) {
        // Instance cell object to capture dictionary out
        Cell newLocation = new Cell( 0, 0 );

        // Convert cell variables to a string for hashcoding
        int instanceId = GetInstanceId( location );

        // If this key is already in location catalog then return location
        if ( cellCatalog.TryGetValue( instanceId, out newLocation ) ) {
            return newLocation;
        }
        else {
            // If the base is completed then check if its time to spawn sections
            if ( isBaseComplete )
                SectionEventsAndDelegates.Instance.CellTriggered( location.GetPosition() );
            // Thread to add cell to the dictionary
            Thread t1 = new Thread( () => AddToDict( instanceId, location ) );
            t1.Start();
            // Thread to add the cell to active dictionary
            Thread t2 = new Thread( () => AddActiveTile( instanceId, location ) );
            t2.Start();
        }

        return location;

    }

    /// <summary>
    /// Checks to see if cell is already active
    /// </summary>
    /// <param name="location">The tile location being check</param>
    /// <returns>True if the tile is already active</returns>
    public bool IsTileActive ( int code ) {
        bool active = false;
        active = activeLocations.ContainsKey( code );
        return active;
    }

    /// <summary>
    /// Checks to see if the cell is already catalogged.
    /// </summary>
    /// <param name="code">Hash Code</param>
    /// <returns>If cell is already catalogged</returns>
    public bool IsCellCatalogged ( int code ) {
        bool active = false;
        active = cellCatalog.ContainsKey( code );
        return active;
    }

    /// <summary>
    /// Get the number of cell that are currently active.
    /// </summary>
    /// <returns></returns>
    public int GetActiveCount () {
        return activeLocations.Count;
    }

    /// <summary>
    /// Assign the tile property to the cell object located in
    /// dictionary, and the reassign cell to dictionary.
    /// </summary>
    /// <param name="tileName">The tag name of tile.</param>
    /// <param name="cell">The cell object to change.</param>
    public void SetCellTileProperty ( string tileName, Vector3 cell ) {
        lock ( _tagLocker ) {
            int instanceId = GetInstanceId( cell );
            // If the cell is recorded into the location Catalog
            if ( cellCatalog.ContainsKey( instanceId ) ) {
                // Get the cell
                Cell newCell = cellCatalog[instanceId];
                // Set the tile name property
                newCell.TileTag = tileName;
                // Reassign the cell object back to dictionary
                cellCatalog[instanceId] = newCell;
            }
        }
    }

    /// <summary>
    /// Add the tile that just actived to an active tile dictionary
    /// </summary>
    /// <param name="location">Location of tile activated</param>
    public void AddActiveTile ( int instanceId, Cell location ) {
        lock ( _activeLocker ) {
            // Double Check
            if ( !activeLocations.ContainsKey( instanceId ) )
                // Add location to the dictionary of active cells
                activeLocations.Add( instanceId, location );
        }
    }

    /// <summary>
    /// Add the cell object to the dictionary for later
    /// referencing.
    /// </summary>
    /// <param name="instanceId">The dictionary key</param>
    /// <param name="location">The Cell object to be recorded</param>
    public void AddToDict ( int instanceId, Cell location ) {
        lock ( _addLocker ) {
            // Double Check
            if ( !cellCatalog.ContainsKey( instanceId ) ) {
                // Add this location to the catalog than activate tile
                cellCatalog.Add( instanceId.GetHashCode(), location );
            }
        }
    }

    /// <summary>
    /// See if the cell is already in the cell library.
    /// If it is then return cell, if it isn't then 
    /// return a null object.
    /// </summary>
    /// <param name="instanceId">The hash code for the cell object.</param>
    /// <returns></returns>
    public Cell GetCellFromCatalog ( int instanceId ) {
        if ( cellCatalog.ContainsKey( instanceId ) ) {
            return cellCatalog[instanceId];
        }
        return null;
    }

    /// <summary>
    /// The cell properties uses this method to retrieve the info about 
    /// the cell of current location.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public Cell GetCellFromCatalog ( Vector3 location ) {
        if ( cellCatalog.ContainsKey( GetInstanceId( location ) ) ) {
            return cellCatalog[GetInstanceId( location )];
        }
        return null;
    }

    /// <summary>
    /// The Librarian class uses this method.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public Cell GetNeighborFromCatalog ( Cell cell ) {
        if ( cellCatalog.ContainsKey( GetInstanceId( cell ) ) ) {
            return cellCatalog[GetInstanceId( cell )];
        }
        return cell;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Set the cell code to the dictionary key so no errors on the wrong key
    /// </summary>
    /// <param name="cell">Cell to get key</param>
    /// <returns></returns>
    private int GetInstanceId ( Cell cell ) {
        string id = string.Format( "{0}, {1}", cell.X, cell.Z );
        return id.GetHashCode();
    }

    /// <summary>
    /// Set the cell code to the dictionary key so no errors on the wrong key
    /// </summary>
    /// <param name="cell">Cell to get key</param>
    /// <returns></returns>
    private int GetInstanceId ( Vector3 cell ) {
        string id = string.Format( "{0}, {1}", cell.x, cell.z );
        return id.GetHashCode();
    }

    #endregion

    #region Events

    public void OnBaseCompleted (object source) {
        isBaseComplete = true;
        print( "Base Completed!" );
    }

    #endregion

    #region Gizmos

    //void OnDrawGizmos () {
    //    if(cellCatalog != null ) {
    //        foreach(KeyValuePair<int, Cell> cell in cellCatalog ) {
    //            Vector3 pos = new Vector3(cell.Value.X, cell.Value.Y - 30, cell.Value.Z);
    //            Gizmos.DrawCube( pos, Vector3.one * 2 );
    //        }
    //    }
    //}

    #endregion

}
