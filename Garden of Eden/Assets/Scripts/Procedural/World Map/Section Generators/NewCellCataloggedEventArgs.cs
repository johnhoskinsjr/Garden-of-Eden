using UnityEngine;
using System.Collections;
using System;

public class NewCellCataloggedEventArgs : EventArgs{

    public Vector3 cataloggedCellPosition { get; set; } // The position of the cell the player just triggered

}
