using UnityEngine;

public class RoomCell : MonoBehaviour
{
    WallDoorParent[] wallDoorParentsArray;

    void Awake()
    {
        wallDoorParentsArray = GetComponentsInChildren<WallDoorParent>();
    }

    public void TurnWallIntoDoorFrame(GridTesting.cardinalDirections direction)
    {
        if (wallDoorParentsArray.Length == 0)
        {
            wallDoorParentsArray = GetComponentsInChildren<WallDoorParent>();
        }

        foreach (var wallDoorParent in wallDoorParentsArray)
        {
            if (direction == wallDoorParent.wallDirection)
            {
                wallDoorParent.wall.SetActive(false);
                wallDoorParent.doorFrame.SetActive(true);
                break;
            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }

        

}
