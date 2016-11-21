using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Singleton EventManager to send events to listeners
// Works with IListener implements

public class EventManager : MonoBehaviour {

    #region Properties

    // Public access to instance
    public static EventManager Instance {
        get { return Instance; }
        set { }
    }

    #endregion

    #region Variables

    private static EventManager instance = null; // Notification instance manager
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>(); // Dictionary of Listeners

    #endregion

    #region Unity Events

    void Awake () {
        //If no instance exist, then assign this instance
        if(instance = null ) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            DestroyImmediate(this);
        }
    }

    void OnLevelWasLoaded () {
        RemoveRedundancies();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Function to add listener to array of listeners
    /// </summary>
    /// <param name="Event_Type">Event to listen for</param>
    /// <param name="Listener">Object to listen for event</param>
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener ) {
        // List of listeners for this event
        List<IListener> ListenList = null;
        // Check existing event type key. If exist, add to
        if(Listeners.TryGetValue(Event_Type, out ListenList) ) {
            // List exist, so add new item
            ListenList.Add(Listener);
            return;
        }

        // Otherwise create new list as dictionary key
        ListenList = new List<IListener>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type , ListenList);
    }

    /// <summary>
    /// Function to post event to listeners
    /// </summary>
    /// <param name="Event_Type">Event to invoke</param>
    /// <param name="Sender">Object invoking event</param>
    /// <param name="Param">Optional argument</param>
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, Object Param = null ) {
        //Notify all listeners of event
        //List of listeners for this event only
        List<IListener> ListenerList = null;
        // If no event exist, then exit
        if ( !Listeners.TryGetValue(Event_Type , out ListenerList) )
            return;
        // Entry exist. Now notify appropriate listeners
        for(int i = 0; i < ListenerList.Count; i++ ) {
            if ( !ListenerList[i].Equals(null) )
                ListenerList[i].OnEvent(Event_Type , Sender , Param);
        }
    }

    /// <summary>
    /// Remove event from dictionary, including all listeners
    /// </summary>
    /// <param name="Event_Type"></param>
    public void RemoveEvent (EVENT_TYPE Event_Type) {
        // Remove entry from dictionary
        Listeners.Remove(Event_Type);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Remove all redundant entries from dictionary
    /// </summary>
    private void RemoveRedundancies () {
        // Create new dictionary
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();
        // Cycle through all dictionary entries
        foreach(KeyValuePair<EVENT_TYPE, List<IListener>> item in Listeners ) {
            // Cycle all listeners, remove null objects
            for(int i = item.Value.Count - 1; i >= 0; i-- ) {
                // If null, then remove item
                if ( item.Value[i].Equals(null) )
                    item.Value.RemoveAt(i);
            }
            // If items remain in list, then add to tmp dictionary
            if ( item.Value.Count > 0 )
                TmpListeners.Add(item.Key , item.Value);
        }
        // Replace listeners object with new dictionary
        Listeners = TmpListeners;
    }

    #endregion
}
