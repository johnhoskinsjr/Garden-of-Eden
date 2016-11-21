/*Class to fade from camera 0 to 1, and back from 1 to 0
 * This class assumes there are only two scene cameras
 */ 
using UnityEngine;
using System.Collections;

public class CameraFader : MonoBehaviour {

    #region Variables

    public Camera[] cameras; // All cameras in the scene to be composited
    public Color[] colors = null; // Color to multiply with render
    public float fadeTime = 2.0f; // Fade in/out in seconds
    public Material mat = null; // Material used as shader to final render

    #endregion

    #region Unity Events
    // Use this for initialization
    void Start () {
	    // Assign render textures to each camera
        foreach(Camera c in cameras ) {
            c.targetTexture = new RenderTexture(Screen.width , Screen.height , 24); // Create Texture
        }
	}
	
	// Update is called once per frame
	void OnPostRender () {
        // Define screen rect
        Rect screenRct = new Rect(0, 0, Screen.width, Screen.height );
        // Source Rect
        Rect sourceRect = new Rect(0, 1, 1, -1);
	}

    #endregion
}
