using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    [SerializeField] int useSeed;


    [Range(1, 4)]
    public int numDoorsToCreate;

    public GameObject startingCellPrefab;
    public GameObject testPrefab;


    Grid grid;
    Camera mainCam;

    InputAction fireAction;

    public enum cardinalDirections
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
        mainCam = Camera.main;

        

        Vector3Int centerCell = new Vector3Int(Mathf.FloorToInt(CellCoordinates.Instance.MaxMapXSize / 2), 0, Mathf.FloorToInt(CellCoordinates.Instance.MaxMapYSize / 2));

        CreateNewCell(centerCell, startingCellPrefab);


        fireAction = InputSystem.actions.FindAction("Fire");

    }

    // Update is called once per frame
    void Update()
    {
        //if (fireAction.WasPerformedThisFrame())
        //{
        //    CreateNewCell(grid.WorldToCell(GetMousePos()));
        //}
    }

    void CreateNewCell(Vector3Int location, GameObject cell)
    {
        //SpawnPrefabAtPoint.instance.SpawnPrefab(testPrefab, grid.GetCellCenterWorld(cellPos), null);
        GameObject newCell = Instantiate(cell);
        newCell.transform.position = grid.GetCellCenterWorld(location);

        // Sets new cell in cell coordinates.
        CellCoordinates.Instance.cellCoordinates[location.x, location.y] = newCell;

        print("Cell Coordinates: ( " + location.x + " , " + location.y + " )");

        GenerateDoors(newCell, numDoorsToCreate);
    }

    private Vector3 GetMousePos()
    {
        Ray cameraRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(cameraRay, out var hitInfo, Mathf.Infinity, groundMask))
        {
            //print("Ray Hit: " + hitInfo.point);
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
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
            int doorDirection = GetRandomDoorDirection();

            while (generatedDoorDirections.Contains(doorDirection))
            {
                print("Door Direction (" + doorDirection + ") already generated, regenerating...");
                doorDirection = GetRandomDoorDirection();
            }

            generatedDoorDirections[i] = doorDirection;

            switch (doorDirection)
            {
                // North Direction
                case 1:
                    Debug.LogError("NORTH DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.North);
                    GenerateBranchOut(cardinalDirections.North);
                    break;

                // East Direction
                case 2:
                    Debug.LogError("EAST DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.East);
                    GenerateBranchOut(cardinalDirections.East);
                    break;

                // South Direction
                case 3:
                    Debug.LogError("SOUTH DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.South);
                    GenerateBranchOut(cardinalDirections.South);
                    break;

                // West Direction
                case 4:
                    Debug.LogError("WEST DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.West);
                    GenerateBranchOut(cardinalDirections.West);
                    break;

                default:
                    Debug.LogError("Door direction number generated not between 1 and 4...");
                    break;
            }
        }

        
    }

    void GenerateBranchOut(GridTesting.cardinalDirections direction)
    {

    }

    private int GetRandomDoorDirection()
    {
        return UnityEngine.Random.Range(1, 5);
    }



}
