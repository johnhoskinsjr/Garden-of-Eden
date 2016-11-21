using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {

    #region Variables

    #region Private Variables
    private Rigidbody playerRigidbody; // The ridgidbody that is attached to the player
    private Animator playerAnimController; // Reference to the animator controller for the player
    private Vector3 playerPreviousPosition = Vector3.zero; // This variable hold the players last position recorded
    private Vector3 playerRunDirection; // The destination vector that the player will move towards
    private bool isPlayerWalking = false; // Is the player currently walking
    #endregion

    #region Public Variables
    public float playerVelocity = 2.0f; // The velocity that the player moves across map
    #endregion

    #endregion

    #region Unity Events
    
    void Start () {

        // Initialize the rigidbody attached to player
        playerRigidbody = GetComponent<Rigidbody>();

        // Initialize the animator component attached to the player
        playerAnimController = GetComponent<Animator>();
        
    }

    void Update () {

        if ( isPlayerWalking ) {

            // Round all the cords up to the nearest interger for easy math
            Vector3 pos = new Vector3( Mathf.CeilToInt( transform.position.x ), Mathf.CeilToInt( transform.position.y ), Mathf.CeilToInt( transform.position.z ) );

            // If player moves in the x direction
            if ( (pos.x % 2 == 0) && (playerPreviousPosition.x != pos.x) ) {
                // Post player moved a cell event
                //PlayerEventsAndDelegates.Instance.PlayerOnNewCell( pos );
            }

            // If player moved in the z direction
            else if ( (pos.z % 2 == 0) && (playerPreviousPosition.z != pos.z) ) {

                // Post player moved a cell event
                //PlayerEventsAndDelegates.Instance.PlayerOnNewCell( pos );
            }

            // Assign the current location to the previous location for the next loop
            playerPreviousPosition = pos;
        }
    }
	
	void FixedUpdate () {

        // If user is sending controller input.
        if ( Input.anyKey ) {

            // Get the raw input of either 1 or -1 from player 
            float xMovement = Input.GetAxisRaw( "Horizontal" );
            float zMovement = Input.GetAxisRaw( "Vertical" );

            // Set the destination vector for the player movement and direction
            playerRunDirection.Set( xMovement, 0f, zMovement );

            // Make the player move in the direction of input
            MovePlayer( xMovement,zMovement );

            // Make the player rotate to face direction running
            TurnPlayer();

            // If the player running animation trigger is false, then start the running animation
            if ( !playerAnimController.GetBool( "Run" )){
                StartRunningAnimation();
            }
        }

        // If the user isn't pressing a key, but the run animation trigger is set to true, then stop run animation
        else if(playerAnimController.GetBool("Run")){
            StopRunningAnimation();
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method determines where the player needs to 
    /// move to, and then moves the player there. This 
    /// method is called on a fixed update.
    /// </summary>
    /// <param name="xMovement">The X input value</param>
    /// <param name="zMovement">The Z input value</param>
    private void MovePlayer (float xMovement, float yMovement) {

        // Assign the temp vector for the player movement so you dont set the move towards variable for turning
        Vector3 tmpDirection = playerRunDirection;

        // Normalize the destination vector to constrain it to 1
        tmpDirection = tmpDirection.normalized * playerVelocity * Time.fixedDeltaTime;

        // Move the player to the new position
        playerRigidbody.MovePosition( transform.position + tmpDirection );
    }

    /// <summary>
    /// This method is called when the player is 
    /// running to make the player face the running 
    /// direction.
    /// </summary>
    /// <param name="direction">The direction vector from controller input.</param>
    private void TurnPlayer () {

        // Rotates the player to face forward with a smooth transition
        Quaternion turnPlayer = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(playerRunDirection),10f);

        // Apply rotation
        transform.rotation = turnPlayer;
    }

    /// <summary>
    /// This method is called when ever the player starts to run.
    /// </summary>
    private void StartRunningAnimation()
    {
        // Start trhe player run animation
        playerAnimController.SetBool("Run", true);
        isPlayerWalking = true;
    }

    /// <summary>
    /// This method is called whenever the player stops running.
    /// </summary>
    private void StopRunningAnimation()
    {
        // Stop the run player run animation
        playerAnimController.SetBool("Run", false);
        isPlayerWalking = false;
    }

    #endregion
}
