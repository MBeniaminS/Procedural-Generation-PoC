using System;
using System.Collections.Generic;
using UnityEngine;
using static MapGenerationManager;
using CardinalDirection = MapGenerationManager.CardinalDirections;
using Rand = UnityEngine.Random;

public class RoomCell : MonoBehaviour
{
    public Vector3Int cellPosition;
    public int maxDoorsCreated = 2;
    public int amountOfDoorsCreated = 0;
    public List<CardinalDirection> doorDirectionsBlacklist = new List<CardinalDirection>();
    public CardinalDirection[] generatedDoorDirections;
        

    public bool isBranchDone = false;


    #region Unity Methods
    void Awake()
    {
        wallDoorParentsArray = GetComponentsInChildren<WallDoorParent>();
    }

    private void OnEnable()
    {
        ResetAllWalls();
    }

    #endregion

    #region Public Methods
    public CardinalDirection[] GenerateDoors(int amount)
    {
        print("Attempting to generate doors...");

        // Error checking and fixing
        if (amount < 0 || amount > 4)
        {
            Debug.LogWarning("Amount of doors asked to be generated ( " + amount + " ) exceeds amount of walls available or is in negatives. Clamping the parameter...");
            amount = Mathf.Clamp(amount, 0, 4);
        }

        isNorthCreated = false;
        generatedDoorDirections = new CardinalDirection[amount];

        if (MapGenerationManager.Instance.allowGenerateDoors)
        {
            // Generates a non-door wall to turn into a door and calls the method to do so
            for (int i = 0; i < amount; i++)
            {
                CardinalDirection doorDirection = GetRandomDoorDirection();

                /// Only allows for the Door Frame conversion to happen if it is a unique direction 
                /// and has not been generated already. 
                /// The array being an enum array creates an issue of the first enum (North Direction) always being "contained"
                /// inside of the array, so a bool has to be created to check if that direction enum is actually
                /// being used.
                while (CheckIfDirectionOccupied(doorDirection))
                {
                    // Failsafe so that if there are physically no more available doors to be possibly made, it breaks out of the generation method.
                    if (generatedDoorDirections.Length + doorDirectionsBlacklist.Count >= 4)
                    {
                        return generatedDoorDirections;
                    }
                    if (doorDirection == CardinalDirection.North && !isNorthCreated && !doorDirectionsBlacklist.Contains(CardinalDirection.North))
                    {
                        isNorthCreated = true;
                        break;
                    }
                    #region Console Logging
                    Debug.LogWarning("Door Direction (" + doorDirection + ") is occupied, regenerating...");
                    #endregion
                    doorDirection = GetRandomDoorDirection();
                }
                generatedDoorDirections[i] = doorDirection;
                #region Console Logging
                print("Index: " + i + " = " + doorDirection);
                #endregion

                amountOfDoorsCreated++;
                TurnWallIntoDoorFrame(doorDirection);
            }
            return generatedDoorDirections;
        }
        else
        {
            Debug.LogWarning("Generating Doors have been disabled, returning as null...");

            isBranchDone = true;

            // If cannot generate doors, return as null
            return null;
        }
    }

    /// <summary>
    /// Turns the side of wall given from a regular Wall into a Door Frame, allowing cells to
    /// branch out through that wall.
    /// </summary>
    /// <param name="direction">The direction/side of the room's Wall to turn into a Door Frame.</param>
    public void TurnWallIntoDoorFrame(CardinalDirection direction)
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
    /// <summary>
    /// Checks if adjacent cells are already placed, and randomises whether to place it in the blacklist or not. 
    /// If not blacklisted, it has a chance to branch the two rooms together.
    /// </summary>
    public void CheckForAdjacentWalls()
    {
        foreach (CardinalDirection direction in Enum.GetValues(typeof(CardinalDirection)))
        {
            // First checks if said direction is in the blacklist, in which case it is already blacklisted and can be skipped.
            if (!doorDirectionsBlacklist.Contains(direction))
            {
                Vector3Int offsettedPosition = cellPosition;
                // Offsets the positions of the cell coordinate based on the current direction.
                switch (direction)
                {
                    case CardinalDirections.North:
                        offsettedPosition.z += 1;
                        break;
                    case CardinalDirections.East:
                        offsettedPosition.x += 1;
                        break;
                    case CardinalDirections.South:
                        offsettedPosition.z -= 1;
                        break;
                    case CardinalDirections.West:
                        offsettedPosition.x -= 1;
                        break;
                    default:
                        Debug.LogError("The direction enum given is invalid (I don't know how this is possible)");
                        break;
                }

                // Checks if there is a cell already created in the coordinate.
                bool isCellInPosition = CellCoordinates.Instance.GetCellInLocation(offsettedPosition);
                if (isCellInPosition)
                {
                    print("Adding " + direction + " to blacklist...");
                    doorDirectionsBlacklist.Add(direction);
                }
            }
        }
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Randomises a number between 1 and 4 for the index, and turns the int into a <see cref="MapGenerationManager.CardinalDirections"/>
    /// </summary>
    /// <returns>Returns the said <see cref="MapGenerationManager.CardinalDirections"/></returns>
    private CardinalDirection GetRandomDoorDirection()
    {
        int index = Rand.Range(1, 5);
        switch (index)
        {
            // North Direction
            case 1:
                return CardinalDirection.North;

            // East Direction
            case 2:
                return CardinalDirection.East;

            // South Direction
            case 3:
                return CardinalDirection.South;

            // West Direction
            case 4:
                return CardinalDirection.West;

            default:
                Debug.LogError("Door direction number generated not between 1 and 4. Returning default of North...");
                return CardinalDirection.North;
        }
    }

    /// <summary>
    /// Checks if the direction to place the door is either in the blacklist or has already been generated.
    /// </summary>
    /// <param name="direction">The direction to check if occupied.</param>
    /// <returns></returns>
    private bool CheckIfDirectionOccupied(CardinalDirection direction)
    {
        // If the direction generated is already in the blacklist, return as false immediately
        if (doorDirectionsBlacklist.Contains(direction))
        {
            Debug.LogWarning("Direction " + direction + " is in the Blacklist, returning as true...");
            return true;
        }
        foreach (var dir in generatedDoorDirections)
        {
            if (dir.Equals(direction))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Ensures that all Wall's <see cref="WallDoorParent"/> are found, and disables all Door Frames and enables all Walls.
    /// </summary>
    private void ResetAllWalls()
    {
        if (wallDoorParentsArray.Length == 0)
        {
            wallDoorParentsArray = GetComponentsInChildren<WallDoorParent>();
        }

        foreach (var wallDoorParent in wallDoorParentsArray)
        {
            wallDoorParent.wall.SetActive(true);
            wallDoorParent.doorFrame.SetActive(false);
            break;
        }
    }

    #endregion

    #region Private Variables

    WallDoorParent[] wallDoorParentsArray;

    bool isNorthCreated;

    #endregion
}
