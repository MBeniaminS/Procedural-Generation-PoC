using UnityEngine;

/// <summary>
/// Class placed on the WallDoorParent of each room cell to provide an easy way to
/// find each wall in a cell, with which side of the room the wall is on.
/// </summary>
public class WallDoorParent : MonoBehaviour
{
    public MapGenerationManager.CardinalDirections wallDirection;
    public GameObject wall;
    public GameObject doorFrame;
}

