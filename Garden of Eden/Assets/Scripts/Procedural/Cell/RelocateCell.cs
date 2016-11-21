using UnityEngine;
using System.Collections;
using System;

/*This class is job is to check the cells distance from the 
 * player and if the cell is too far from player, then move 
 * to the front of the player.
 * 
 * This script will read the direction of the player. When the player walks to a new cell,
 * this script will check the cell location against the location of the cell the player is 
 * standing on. If the player is walking away from this cell, then move this cell to the 
 * front of the player. I f the player is not walking away from this cell then do nothing.
 */

public class RelocateCell : MonoBehaviour {

    #region Variables

    #region Private
    private Vector3 newLocation; // The location of the cell that was just triggered, the current position of the player
    private CellProperties cellProperties; // The reference to this cells cell properties
    private int zTop; // The reference to the number of cell rows above player
    private int zBottom; // The reference the the number of cell rows below player
    private int xMax; // The variable to hold the maximum size of the base along the x axis
    #endregion

    #endregion

    #region Unity Events

    // Use this for initialization
    void Start () {

        // Add listener to the Player Walking Event
        PlayerEventsAndDelegates.Instance.PlayerWalking += OnPlayerWalking;

        // Get the instance of Cell Properties
        cellProperties = gameObject.GetComponent<CellProperties>();

        // Grab base size from base manager
        zTop = BaseManager.Instance.ZTop;
        zBottom = BaseManager.Instance.ZBottom;
        xMax = BaseManager.Instance.XMax;
    }

    #endregion

    #region Events

    /// <summary>
    /// The method called when the Player Walking Event is triggered.
    /// </summary>
    /// <param name="source">The script calling event</param>
    /// <param name="e">The set of arguments passed by event</param>
    public void OnPlayerWalking (object source, PlayerWalkingEventArgs e) {
        // Is your distance abs of max + 1
        Vector3 displacement = transform.position - e.PlayerPosition;
        if ( displacement.z <= (-zBottom) - 2 ) {
            MoveUp();
            cellProperties.CellMoved();
        }
        else if( displacement.z >= (zTop) + 2 ) {
            MoveDown();
            cellProperties.CellMoved();
        }
        else if ( displacement.x <= (-xMax) - 2 ) {
            MoveRight();
            cellProperties.CellMoved();
        }
        else if ( displacement.x >= (xMax) + 2 ) {
            MoveLeft();
            cellProperties.CellMoved();
        }
    }

    #endregion

    #region Private Methods

    // These methods are called when the tile needs to relocate.
    // The method is named after the direction the tile moves.
    private void MoveUp () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + ((zTop + zBottom) + 2) );
    }

    private void MoveDown () {
        transform.position = new Vector3( transform.position.x, transform.position.y, transform.position.z + ((-zBottom - zTop) - 2) );
    }

    private void MoveRight () {
        transform.position = new Vector3( transform.position.x + ((xMax * 2) + 2), transform.position.y, transform.position.z );
    }

    private void MoveLeft () {
        transform.position = new Vector3( transform.position.x + ((-xMax * 2) - 2), transform.position.y, transform.position.z );
    }

    #endregion
}
