using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;


    [Range(1, 4)]
    public int numDoorsToCreate;

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



        fireAction = InputSystem.actions.FindAction("Fire");

        GenerateDoors(testPrefab, numDoorsToCreate);
    }

    // Update is called once per frame
    void Update()
    {
        if (fireAction.WasPerformedThisFrame())
        {
            CreateNewCell(GetMousePos());
        }
    }

    void CreateNewCell(Vector3 location)
    {
        Vector3Int cellPos = grid.WorldToCell(GetMousePos());
        SpawnPrefabAtPoint.instance.SpawnPrefab(testPrefab, grid.GetCellCenterWorld(cellPos), null);
        print("Cell Position: " + cellPos);
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
                    break;

                // East Direction
                case 2:
                    Debug.LogError("EAST DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.East);
                    break;

                // South Direction
                case 3:
                    Debug.LogError("SOUTH DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.South);
                    break;

                // West Direction
                case 4:
                    Debug.LogError("WEST DOOR GENERATED");
                    roomCellScript.TurnWallIntoDoorFrame(cardinalDirections.West);
                    break;

                default:
                    Debug.LogError("Door direction number generated not between 1 and 4...");
                    break;
            }
        }
    }

    private int GetRandomDoorDirection() 
    {
        return Random.Range(1, 5);
    }



}
