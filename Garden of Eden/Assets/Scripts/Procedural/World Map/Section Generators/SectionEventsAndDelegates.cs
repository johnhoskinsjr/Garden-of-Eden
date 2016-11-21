using UnityEngine;
using System.Collections;

public class SectionEventsAndDelegates : MonoBehaviour {

    #region Variables

    #region Public
    // Player Walking Event
    public delegate void CellCataloggedEventHandler ( object source, NewCellCataloggedEventArgs e );
    public event CellCataloggedEventHandler NewCellCatalogged;
    #endregion

    #region Private
    private static SectionEventsAndDelegates instance; // The Singleton Instance Variable
    #endregion

    #region Properties
    public static SectionEventsAndDelegates Instance { get { return instance; } } // The getter/setter for instance
    #endregion

    #endregion

    #region Unity Events

    void Awake () {
        // Instance the singleton class
        if ( instance == null )
            instance = this;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This is the public methods used to trigger the event of the player walking.
    /// </summary>
    /// <param name="cellPos">The location of the cell that was triggered</param>
    public void CellTriggered ( Vector3 cellPos ) {
        OnNewCellCatalogged( cellPos );
    }

    #endregion

    #region Events
    // The Player Walking Event Method
    protected virtual void OnNewCellCatalogged ( Vector3 cellPos ) {
        if ( NewCellCatalogged != null )
            NewCellCatalogged( this, new NewCellCataloggedEventArgs() { cataloggedCellPosition = cellPos } );
    }

    #endregion
}
