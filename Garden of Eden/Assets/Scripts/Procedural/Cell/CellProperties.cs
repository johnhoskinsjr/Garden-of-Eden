using UnityEngine;
using System.Collections;

/* This class is basically the same thing as the cell class. This is the script
 * that holds the data  for the cell at the current location. When a cell object 
 * is pulled out of the library catalog, the cell object is applied to this class.
 * This class manages the cell in the game to take on the features of the cell 
 * stored in the catalog.
 * 
 * This class is the cell data that exist on the actual cell in game scene. As the 
 * in-game cell moves around, he has to change his properties to match the cell in the
 * new location. Those new properties are applied to this script. This script basically
 * manages the cell properties for the in-game cells.
 */
public class CellProperties : MonoBehaviour {

    #region Variables

    #region Private
    private int x; // The cells position x
    private float y = 0f; // The cells position y
    private int z; // The cells position Z
    private string tileTag = null; // The cell tile name
    private ApplyTile tileLogic; // The instance variable
    #endregion

    #region Properties
    public int X { get { return x; } set { x = value; } }
    public float Y { get { return y; } set { y = value; } }
    public int Z { get { return z; } set { z = value; } }
    public string TileTag { get { return tileTag; } set { tileTag = value; } }
    #endregion

    #endregion

    #region Unity Events

    void Start () {
        // Create the instance of the tile logic
        tileLogic = gameObject.GetComponent<ApplyTile>();
        SetCellProperties(WorldMapLibrary.Instance.GetCellFromCatalog( transform.position ));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns the vector of the cell to the caller of method
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCellVector () {
        Vector3 cellPos = new Vector3( X, Y, Z );
        return cellPos;
    }

    /// <summary>
    /// This method is used to set the properties of this cell 
    /// based on the cell that is stored in library catalog for 
    /// this location.
    /// </summary>
    /// <param name="cell"></param>
    public void SetCellProperties ( Cell cell ) {
        this.x = cell.X;
        this.y = cell.Y;
        this.z = cell.Z;
        this.tileTag = cell.TileTag;
        // If the tile logic has never ran for this script, then run tile logic
        if(tileTag == null)
            // Assign the initial tile for this cell
            tileLogic.DetermineTile();
    }

    /// <summary>
    /// This method is called when the cell is moved to a new location.
    /// This method is used to set the values for the tile based on the
    /// location of the tile.
    /// </summary>
    public void CellMoved () {
        // Create a cell object for the new location
        Cell cell = new Cell( (int)transform.position.x, (int)transform.position.z );
        // Pass the cell object with new location to library catalog to get the cell that belongs at this location.
        cell = WorldMapLibrary.Instance.GetCellLocation(cell);
        // Set the properties for this cell based off cell stored in library catalog for this location.
        SetCellProperties( cell );
    }

    #endregion
}
