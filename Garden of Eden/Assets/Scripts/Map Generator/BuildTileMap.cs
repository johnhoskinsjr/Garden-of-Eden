using UnityEngine;
using System.Collections;
using System;

public class BuildTileMap : MonoBehaviour {

    #region Variables

    #region Public Variables

    public GameObject grassTile; // The reference to the grass tile prefab
    public GameObject waterTile; // The reference to the water tile prefab
    public float tileScale; // The size to scale up tile per unity unit
    public float perlinStepX; // The rate to step through perlin noise in X direction function
    public float perlinStepZ; // The rate to step through perlin noise in Z direction function

    #endregion


    #endregion

    #region Unity Events

    void Start () {

    }

    #endregion

    #region Public Methods

    public void BuildMap (int[,] map, int height, int width) {
        for ( int x = 0; x < width; x++ ) {
            for ( int z = 0; z < height; z++ ) {
                if(map[x,z] == 1 ) {
                    Instantiate( grassTile, new Vector3( x * tileScale, Mathf.PerlinNoise(x * perlinStepX, z * perlinStepZ), z * tileScale ), Quaternion.identity );
                }
                else {
                    Instantiate( waterTile, new Vector3( x * tileScale, -0.25f, z * tileScale ), Quaternion.identity );
                } 
            }
        }
        
    }

    #endregion
}
