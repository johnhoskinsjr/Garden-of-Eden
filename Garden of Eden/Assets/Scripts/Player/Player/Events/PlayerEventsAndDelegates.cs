using UnityEngine;
using System.Collections;

public class PlayerEventsAndDelegates : MonoBehaviour {

    #region Variables

    #region Public
    // Player Walking Event
    public delegate void PlayerWalkingEventHandler ( object source, PlayerWalkingEventArgs e );
    public event PlayerWalkingEventHandler PlayerWalking;
    #endregion

    #region Private
    private static PlayerEventsAndDelegates instance; // The Singleton Instance Variable
    #endregion

    #region Properties
    public static PlayerEventsAndDelegates Instance { get { return instance; } } // The getter/setter for instance
    #endregion

    #endregion

    #region Unity Events

    void Awake () {
        // Instance the singleton class
        if ( instance == null )
            instance = this;

        DontDestroyOnLoad( gameObject );
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This is the public methods used to trigger the event of the player walking.
    /// </summary>
    /// <param name="cellPos">The location of the cell that was triggered</param>
    public void PlayerOnNewCell ( Vector3 cellPos ) {
        OnPlayerWalking( cellPos );
    }

    #endregion

    #region Events
    // The Player Walking Event Method
    protected virtual void OnPlayerWalking ( Vector3 cellPos ) {
        PlayerWalking( this, new PlayerWalkingEventArgs() { PlayerPosition = cellPos } );
    }

    #endregion
}
