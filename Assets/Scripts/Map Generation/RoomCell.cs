using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CardinalDirection = MapGenerationManager.CardinalDirections;

public class RoomCell : MonoBehaviour
{
    public Vector3Int cellPosition;
    public int maxDoorsCreated = 2;
    public List<CardinalDirection> doorDirectionsBlacklist = new List<CardinalDirection>();


    WallDoorParent[] wallDoorParentsArray;
    CardinalDirection[] generatedDoorDirections;

    bool _isNorthCreated;

    #region Unity Methods
    void Awake()
    {
        wallDoorParentsArray = GetComponentsInChildren<WallDoorParent>();
    }

    #endregion

    #region Public Methods
    public CardinalDirection[] GenerateDoors(int amount)
    {
        print("Generating Doors...");

        // Error checking and fixing
        if (amount < 0 || amount > 4)
        {
            Debug.LogWarning("Amount of doors asked to be generated ( " + amount + " ) exceeds amount of walls available or is in negatives. Clamping the parameter...");
            amount = Mathf.Clamp(amount, 0, 4);
        }

        _isNorthCreated = false;
        generatedDoorDirections = new CardinalDirection[amount];
   
        // Generates a non-door wall to turn into a door and calls the method to do so
        for (int i = 0; i < amount; i++)
        {
            CardinalDirection doorDirection = GetRandomDoorDirection();

            /// Only allows for the Door Frame conversion to happen if it is a unique direction 
            /// and has not been generated already. 
            /// The array being an enum array creates an issue of the first enum always being "contained"
            /// inside of the array, so a bool has to be created to check if that direction enum is actually
            /// being used.
            while (CheckIfDirectionOccupied(doorDirection))
            {
                if (doorDirection == CardinalDirection.North && !_isNorthCreated)
                {
                    _isNorthCreated = true;
                    break;
                }
                Debug.LogWarning("Door Direction (" + doorDirection + ") is occupied, regenerating...");
                doorDirection = GetRandomDoorDirection();
            }
            generatedDoorDirections[i] = doorDirection;
            #region Console Logging
            print("Index: " + i + " = " +  doorDirection);
            #endregion

            TurnWallIntoDoorFrame(doorDirection);
        }

        return generatedDoorDirections;
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

    #endregion

    #region Private Methods
    /// <summary>
    /// Randomises a number between 1 and 4 for the index, and turns the int into a <see cref="MapGenerationManager.CardinalDirections"/>
    /// </summary>
    /// <returns>Returns the said <see cref="MapGenerationManager.CardinalDirections"/></returns>
    private CardinalDirection GetRandomDoorDirection()
    {
        int index = UnityEngine.Random.Range(1, 5);
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

    #endregion
}
