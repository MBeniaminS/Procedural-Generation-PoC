using UnityEngine;

public class RoomCell : MonoBehaviour
{
    WallDoorParent[] wallDoorParentsArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wallDoorParentsArray = GetComponentsInChildren<WallDoorParent>();
        print(wallDoorParentsArray.Length);
    }

    public void TurnWallIntoDoorFrame(GridTesting.cardinalDirections direction)
    {
        foreach (var wallDoorParent in wallDoorParentsArray)
        {
            if (direction == wallDoorParent.wallDirection)
            {
                wallDoorParent.wall.SetActive(false);
                wallDoorParent.doorFrame.SetActive(true);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

        

}
