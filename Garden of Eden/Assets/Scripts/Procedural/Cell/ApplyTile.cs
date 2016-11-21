using UnityEngine;
using System.Collections;
using System.Threading;

public class ApplyTile : MonoBehaviour {

    #region Variables

    #region Private
    private static Inventory inventory; // The variable that holds the inventory instance
    #endregion

    #region Public 

    #endregion

    #endregion

    #region Unity Events

    // Use this for initialization
    void Start () {
        // Instance the inventory object
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    #endregion

    #region Public Methods

    public void DetermineTile () {
        // If grass pool is empty
        GameObject grass = Instantiate( inventory.grass.grass);

        // Make the cellObj a parent to the tile
        grass.transform.parent = transform;

        // Zero out the tile local position
        grass.transform.localPosition = Vector3.zero;

        // Name the tile tag
        grass.tag = "Grass";
        Vector3 location = transform.position;

        // Store the tile property to the cell object in library catalog
        Thread t1 = new Thread( () => WorldMapLibrary.Instance.SetCellTileProperty( "Grass", location ) );
        t1.Start();
    }

    #endregion

}
