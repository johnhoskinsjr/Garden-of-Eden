using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/* This script manages the cells neighborhood
 * This script is NOT being used currently
 */

public class Neighborhood : MonoBehaviour {

    #region Variables

    #region Private
    private List<Cell> neighbors = new List<Cell>(); // The list of local adjacent neighbors
    private Librarian librarian; // The variable that holds the librarian instance
    #endregion

    #region Public
    public int zOffset; // The amount of cells to spawn above and below player
    public int xOffset; // The amount of cells to spawn to the laft and right of player
    #endregion

    #endregion

    #region Unity Events

    void Start () {
        // Create an instance of the librarian class
        if ( librarian == null )
            librarian = new Librarian();
        PlayerEventsAndDelegates.Instance.PlayerWalking += OnPlayerWalking;
    }

    #endregion

    #region Public Mathods

    public void OnPlayerWalking (object source, PlayerWalkingEventArgs e ) {
        //print( "Player Walking!!! - Neighborhood" );
    }

    #endregion
}
