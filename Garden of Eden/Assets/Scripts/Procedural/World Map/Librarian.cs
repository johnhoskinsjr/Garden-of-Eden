using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class Librarian {

    #region Public Methods

    /// <summary>
    /// Call this method if you want to know if the cell is active
    /// </summary>
    /// <param name="cell">this</param>
    /// <returns></returns>
    public bool IsAlreadyActive(Cell cell ) {
        bool activeState = WorldMapLibrary.Instance.IsTileActive( GetInstanceId( cell ) );
        if ( !activeState )
            AddToActive( cell );
        return activeState;
    }

    /// <summary>
    /// Call this method when you want to add 
    /// the cell to the active cell list.
    /// </summary>
    /// <param name="cell">this</param>
    public void AddToActive(Cell cell ) {
        Thread t = new Thread( () => WorldMapLibrary.Instance.AddActiveTile( GetInstanceId( cell ), cell ) );
        t.Start();
    }

    /// <summary>
    /// Call this method when you want to 
    /// add the cell to the cell catalog.
    /// </summary>
    /// <param name="cell"></param>
    public void AddToLibraryCatalog(Cell cell ) {
        Thread t = new Thread(() => WorldMapLibrary.Instance.AddToDict(GetInstanceId(cell), cell));
        t.Start();
    }

    /// <summary>
    /// Call this method when you want a cell object returned to you.
    /// If the cell object already exist, then a copy will be returned.
    /// If the cell object doesn't exist then a template object will be returned.
    /// </summary>
    /// <param name="cell">this</param>
    /// <returns>NULL if the cell hasn't been catalogged</returns>
    public Cell GetCellFromCatalog(Cell cell ) {
        // Initialize a temporary cell object from the catalog
        Cell tmpCell = WorldMapLibrary.Instance.GetCellFromCatalog( GetInstanceId( cell ) );
        // Returns null if the cell doesn't exist.
        return tmpCell;
    }

    /// <summary>
    /// Returns the cell of the neighbor
    /// </summary>
    /// <param name="neighborPos"></param>
    /// <returns>The cell object that contains the neighbors data</returns>
    public Cell GetNeighbor ( Vector3 neighborPos) {
        Cell tmpCell = new Cell( (int)neighborPos.x, (int)neighborPos.z );
        tmpCell = WorldMapLibrary.Instance.GetNeighborFromCatalog( tmpCell );
        return tmpCell;
    }

    /// <summary>
    /// See if the neighbor has already been catalogged
    /// </summary>
    /// <param name="neighborPos"></param>
    /// <returns>True if the neighbor has already been catalogged</returns>
    public bool IsNeighborCatalogged(Vector3 neighborPos ) {
        return WorldMapLibrary.Instance.IsCellCatalogged( GetInstanceId( neighborPos ) );
    }

    /// <summary>
    /// See if the neighbor is currently active in map
    /// </summary>
    /// <param name="neighborPos"></param>
    /// <returns></returns>
    public bool IsNeighborActive(Vector3 neighborPos ) {
        return WorldMapLibrary.Instance.IsTileActive( GetInstanceId( neighborPos ) );
    }

    /// <summary>
    /// This method is called when its time to place a section on the map.
    /// You will send a dictionary as an argument, and that dictionary 
    /// will be appended to the library catalog.
    /// </summary>
    /// <param name="section">The dictionary of section to be appended.</param>
    public void MergeWithCatalog (Dictionary<int, Cell> section) {

    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Get the hash code for the cell 
    /// being passed as an argument.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private int GetInstanceId ( Cell cell ) {
        string hashCode = string.Format( "{0}, {1}", cell.X, cell.Z );
        return hashCode.GetHashCode();
    }

    /// <summary>
    /// Get the hash code for the cell 
    /// being passed as an argument.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private int GetInstanceId ( Vector3 cell ) {
        string hashCode = string.Format( "{0}, {1}", cell.x, cell.z );
        return hashCode.GetHashCode();
    }

    #endregion
}
