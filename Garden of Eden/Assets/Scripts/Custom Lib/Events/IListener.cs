using UnityEngine;

// Enum defining all possible game events
// More events should be added to the list
public enum EVENT_TYPE {
    BuildWaterMap // Event to build a random water section
};

public interface IListener {

    // Notification function invoked when events happen
    void OnEvent ( EVENT_TYPE Event_Type , Component sender , Object param = null );

}
