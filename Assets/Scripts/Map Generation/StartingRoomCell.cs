using UnityEngine;

public class StartingRoomCell : RoomCell
{
    [SerializeField] Transform playerSpawnTransform;

    // Finds the player and places them in the starting cell
    void Start()
    {
        GameObject.FindWithTag("Player").transform.position = playerSpawnTransform.position;
    }

}
