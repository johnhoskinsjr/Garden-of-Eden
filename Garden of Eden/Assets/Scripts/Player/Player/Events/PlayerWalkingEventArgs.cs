using UnityEngine;
using System.Collections;
using System;

/* The Event Arguments for the player walking event
 */

public class PlayerWalkingEventArgs : EventArgs {

    public Vector3 PlayerPosition { get; set; } // The position of the cell the player just triggered
}
