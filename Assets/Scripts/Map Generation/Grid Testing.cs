using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    CardinalDirectionsBak directionEnum;

    [Range(1, 4)]
    public int numDoorsToCreate;

    public GameObject startingCellPrefab;
    public GameObject testPrefab;

    [SerializeField] GameObject currentCell;


    Grid grid;

    public enum CardinalDirectionsBak
    {
        North,
        East,
        South,
        West
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GetComponent<Grid>();



        Vector3Int centerCell = new Vector3Int(Mathf.FloorToInt(CellCoordinates.Instance.MaxMapXSize / 2), 0, Mathf.FloorToInt(CellCoordinates.Instance.MaxMapZSize / 2));

        CreateNewCell(centerCell, startingCellPrefab);



        //GenerateBranchOut(currentCell, directionEnum);
    }


    void CreateNewCell(Vector3Int location, GameObject cell)
    {
        //SpawnPrefabAtPoint.instance.SpawnPrefab(testPrefab, grid.GetCellCenterWorld(cellPos), null);
        GameObject newCell = Instantiate(cell);
        newCell.transform.position = grid.GetCellCenterWorld(location);

        // Sets new cell in cell coordinates.
        CellCoordinates.Instance.cellCoordinates[location.x, location.z] = newCell;

        print("Cell Coordinates: ( " + location.x + " , " + location.z + " )");

        GenerateDoors(newCell, numDoorsToCreate);

        currentCell = newCell;

        RoomCell roomCellScript = cell.GetComponent<RoomCell>();
        roomCellScript.cellPosition = location;
    }

    private void GenerateDoors(GameObject cell, int amount)
    {

        if (amount > 4)
        {
            Debug.LogError("Can't generate doors. Amount of doors to generate exceeds max amount allowed.");
            return;
        }
        int[] generatedDoorDirections = new int[amount];

        RoomCell roomCellScript = cell.GetComponent<RoomCell>();

        for (int i = 0; i < amount; i++)
        {
            int doorDirectionInt = GetRandomDoorDirection();

            CardinalDirectionsBak doorDirectionEnum = CardinalDirectionsBak.North;

            while (generatedDoorDirections.Contains(doorDirectionInt))
            {
                print("Door Direction (" + doorDirectionInt + ") already generated, regenerating...");
                doorDirectionInt = GetRandomDoorDirection();
            }

            generatedDoorDirections[i] = doorDirectionInt;


            switch (doorDirectionInt)
            {
                // North Direction
                case 1:
                    Debug.LogError("NORTH DOOR GENERATED");
                    doorDirectionEnum = CardinalDirectionsBak.North;
                    directionEnum = CardinalDirectionsBak.North;
                    break;

                // East Direction
                case 2:
                    Debug.LogError("EAST DOOR GENERATED");
                    doorDirectionEnum = CardinalDirectionsBak.East;
                    directionEnum = CardinalDirectionsBak.East;
                    break;

                // South Direction
                case 3:
                    Debug.LogError("SOUTH DOOR GENERATED");
                    doorDirectionEnum = CardinalDirectionsBak.South;
                    directionEnum = CardinalDirectionsBak.South;
                    break;

                // West Direction
                case 4:
                    Debug.LogError("WEST DOOR GENERATED");
                    doorDirectionEnum = CardinalDirectionsBak.West;
                    directionEnum = CardinalDirectionsBak.West;
                    break;

                default:
                    Debug.LogError("Door direction number generated not between 1 and 4...");
                    break;

            }

            //roomCellScript.TurnWallIntoDoorFrame(doorDirectionEnum);
        }


    }

    void GenerateBranchOut(GameObject cell, GridTesting.CardinalDirectionsBak direction)
    {
        Vector3Int newCellLocation;
        newCellLocation = cell.GetComponent<RoomCell>().cellPosition;
        switch (direction)
        {
            case CardinalDirectionsBak.North:
                newCellLocation.z += 1;
                break;

            case CardinalDirectionsBak.East:
                newCellLocation.x += 1;
                break;

            case CardinalDirectionsBak.South:
                newCellLocation.z -= 1;
                break;

            case CardinalDirectionsBak.West:
                newCellLocation.x -= 1;
                break;

            default:
                Debug.LogError("The input direction given is invalid (how?!)");
                break;
        }

        CreateNewCell(newCellLocation, testPrefab);
    }

    private int GetRandomDoorDirection()
    {
        return UnityEngine.Random.Range(1, 5);
        
    }


}
