using UnityEngine;
using System.Collections;

public class WorldEventsAndDelegates : MonoBehaviour {

    #region Variables

    #region Public
    // Player Walking Event
    public delegate void BaseCompletedEventHandler ( object source);
    public event BaseCompletedEventHandler BaseCompleted;
    #endregion

    #region Private
    private static WorldEventsAndDelegates instance; // The Singleton Instance Variable
    #endregion

    #region Properties
    public static WorldEventsAndDelegates Instance { get { return instance; } } // The getter/setter for instance
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
    public void BaseComplete () {
        OnBaseCompleted();
    }

    #endregion

    #region Events
    // The Player Walking Event Method
    protected virtual void OnBaseCompleted () {
        if ( BaseCompleted != null )
            BaseCompleted( this);
    }

    #endregion
}
