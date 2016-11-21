using UnityEngine;
using System.Collections;

public class SectionScanner : MonoBehaviour {

    #region Variables

    #region Private Variables
    private Vector3 lastPlayerPosition;
    private float waitTimer = 0f; // Timer for waiting to spawn next section
    #endregion

    #region Public Variables
    public float chanceOfSection; // The probability that a section will be spawned
    public SectionChances sectionTree; // The class that holds all the section probabilities for east organization
    #endregion

    #endregion

    #region Unity Events

    // Use this for initialization
    void Start () {
        // Add listener to the Player Walking Event
        SectionEventsAndDelegates.Instance.NewCellCatalogged += OnNewCellCatalogged;
    }
	
	// Update is called once per frame
	void Update () {
        waitTimer += Time.deltaTime;
	}

    #endregion

    #region Events

    public void OnNewCellCatalogged ( object source, NewCellCataloggedEventArgs e ) {
        // Having a wait timer, to make sure cells in same row aren't counted as multiplle spawns
        if ( waitTimer > 1.0f ) {
            if ( Random.value < chanceOfSection ) {
                print( "Section Spawned! " + e.cataloggedCellPosition );
                waitTimer = 0;
            }
            // Assign the new location to the last location
            lastPlayerPosition = e.cataloggedCellPosition;
        }
    }

    #endregion
}

#region Section Classes

[System.Serializable]
public class SectionChances {
    public float chanceOfBushes;
}

#endregion
