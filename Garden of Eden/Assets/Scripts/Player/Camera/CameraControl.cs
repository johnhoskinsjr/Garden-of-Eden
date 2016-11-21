using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public GameObject player;
    private Vector3 camPos;
    private Vector3 playerPos;
    public float offsetX=0;
    public float offsetY=0;
    public float offsetZ=0;
    Vector3 velocity;
    public float smoothDamp = 2;

    // Use this for initialization
    void Start () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }
       
      
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        velocity = Vector3.zero;
        playerPos = player.transform.position;

        camPos = new Vector3(playerPos.x -offsetX, playerPos.y - offsetY, playerPos.z - offsetZ);
        transform.position = Vector3.SmoothDamp(camPos,playerPos,ref velocity,smoothDamp*Time.deltaTime);
        
	
	}
}
