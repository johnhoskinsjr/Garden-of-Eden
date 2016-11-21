using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    #region Variables
    public Grass grass;
    public Water water;
    public Trees tree;
    #endregion
}

#region Class Variables

[System.Serializable]
public class Grass {
    public GameObject grass; // Basic grass tile
    public GrassPath grassPath;
    public Bricks brick;
}
[System.Serializable]
public class GrassPath {
    public GameObject grassPathCoupling; // A grass path tile that links 2 paths
    public GameObject grassPathCorner; // A grass path that has 2 connections
    public GameObject grassPathTee; // A grass path that has 3 connects
    public GameObject grassPathCross; // A grass tile where the path has 4 connects
    public GameObject grassPatchy; // A grass tile with patches of grass
    public GameObject grassCracked; // A grass tile with dirt cracks in it
    public GameObject grassCircle; // A grass tile with a dirt circle in it
    public GameObject grassMushroom; // A grass tile that has a mushroom on it
    public GameObject grassPathCouplingFence; // A grass tile path with 2 connects and fences
    public GameObject grassPathCornerFence; // A grass tile path with a turning 2 connections and a fence
}
[System.Serializable]
public class Bricks {
    public GameObject grassBrick; // A standard grass tile with bricks
    public GameObject grassBrickCoupling; // A grass brick tile with 2 connections
    public GameObject grassBrickCorner; // A grass brick tile that makes 2 connection but is a turn
    public GameObject grassBrickTee; // A grass brick tile that has 3 connections
    public GameObject grassBrickCross; // A grass brick tile that has 4 connections
    public GameObject grassBrickPatchy; // A grass brick tile that is patchy
    public GameObject grassBrickCracked; // A grass brick tile with dirt cracks
    public GameObject grassBrickCircle; // A grass brick tile with a circle
}
[System.Serializable]
public class Water {
    public GameObject waterFourWalls; // A water tile that has 4 sides
    public GameObject waterThreeWalls; // A water tile that has 4 sides
    public GameObject waterTwoWalls; // A water tile that has 4 sides
    public GameObject waterOneWalls; // A water tile that has 4 sides
    public GameObject waterNoWalls; // A water tile that has no walls
}
[System.Serializable]
public class Trees {
    public LGPineTree LightPineTree;
    public DGPineTree darkPineTree;
    public AutumnPineTree autumnPineTree;
    public Bush bush;
}
[System.Serializable]
public class LGPineTree {
    public GameObject lightGreenPineTree_1; // A tile with light green trees
    public GameObject lightGreenPineTree_2; // A tile with light green trees
    public GameObject lightGreenPineTree_3; // A tile with light green trees
    public GameObject lightGreenPineTree_4; // A tile with light green trees
    public GameObject lightGreenPineTree_5; // A tile with light green trees
}
[System.Serializable]
public class DGPineTree {
    public GameObject darkGreenPineTree_1; // A tile with light green trees
    public GameObject darkGreenPineTree_2; // A tile with light green trees
    public GameObject darkGreenPineTree_3; // A tile with light green trees
    public GameObject darkGreenPineTree_4; // A tile with light green trees
    public GameObject darkGreenPineTree_5; // A tile with light green trees
}
[System.Serializable]
public class AutumnPineTree {
    public GameObject autumnPineTrees_1; // A tile with autumn color pine trees
    public GameObject autumnPineTrees_2; // A tile with autumn color pine trees
    public GameObject autumnPineTrees_3; // A tile with autumn color pine trees
}
[System.Serializable]
public class Bush {
    public GameObject bushes_1; // A tile with bushy trees and muchrooms
    public GameObject bushes_2; // A tile with bushy trees and muchrooms
    public GameObject bushes_3; // A tile with bushy trees and muchrooms
    public GameObject bushes_4; // A tile with bushy trees and muchrooms
    public GameObject bushes_5; // A tile with bushy trees and muchrooms
    public GameObject bushes_6; // A tile with bushy trees and muchrooms
    public GameObject bushes_7; // A tile with bushy trees and muchrooms
    public GameObject bushes_8; // A tile with bushy trees and muchrooms
    public GameObject bushes_9; // A tile with bushy trees and muchrooms
    public GameObject bushes_10; // A tile with bushy trees and muchrooms
    private GameObject[] bushes = new GameObject[10];

    public GameObject GetRandomBush () {
        GameObject bush = bushes[Random.Range( 0, bushes.Length )];
        if(bush == null ) {
            Populate();
            bush = bushes[Random.Range( 0, bushes.Length )];
        }
        return bush;
    }

    private void Populate () {
        bushes[0] = bushes_1;
        bushes[1] = bushes_2;
        bushes[2] = bushes_3;
        bushes[3] = bushes_4;
        bushes[4] = bushes_5;
        bushes[5] = bushes_6;
        bushes[6] = bushes_7;
        bushes[7] = bushes_8;
        bushes[8] = bushes_9;
        bushes[9] = bushes_10;
    }
}
#endregion