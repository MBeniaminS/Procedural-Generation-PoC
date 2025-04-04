using UnityEngine;
using Rand = UnityEngine.Random;

public class MapGenerationManager : MonoBehaviour
{
    public static MapGenerationManager Instance;
    public enum CardinalDirections
    {
        North,
        East,
        South,
        West
    }

    [SerializeField] GameObject startingCellPrefab;
    [SerializeField] GameObject baseCellPrefab;

    [SerializeField] GameObject currentCell;
    [SerializeField] CardinalDirections[] currentDoorDirections;


    #region Unity Methods
    // Sets instance
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        grid = GetComponent<Grid>();

        /// Finds midpoint of the map based on maximum map sizes provided, and sets the center cell
        /// coordinates and sets center cell to center of map.
        int midpointMapXSize = Mathf.FloorToInt(CellCoordinates.Instance.MaxMapXSize / 2);
        int midpointMapZSize = Mathf.FloorToInt(CellCoordinates.Instance.MaxMapZSize / 2);
        Vector3Int centerCellLocation = new Vector3Int(midpointMapXSize, 0, midpointMapZSize);

        CreateNewCell(centerCellLocation, startingCellPrefab, Rand.Range(2, 4), out currentDoorDirections);

        int randomDirectionIndex = Rand.Range(0, currentDoorDirections.Length);
        GenerateBranchOut(currentCell, currentDoorDirections[randomDirectionIndex]);
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Creates a new cell and gives out an array of the directions the cell may branch out of
    /// </summary>
    /// <param name="location">Where the cell should be created</param>
    /// <param name="cell">The cell prefab that is created</param>
    /// <param name="amountOfDoors">How many doors should the cell form</param>
    /// <param name="doorDirections">The array of directions that the cell is branching out to</param>
    void CreateNewCell(Vector3Int location, GameObject cell, int amountOfDoors, out CardinalDirections[] doorDirections)
    {
        GameObject newCell = Instantiate(cell);
        newCell.transform.position = grid.GetCellCenterWorld(location);

        // Sets new instantiated cell in cell coordinates
        CellCoordinates.Instance.cellCoordinates[location.x, location.z] = newCell;
        currentCell = newCell;

        #region Console Logging
        // Debug.Log("Cell Coordinates: ( " + location.x + " , " + location.z + " )");
        #endregion

        RoomCell roomCell = newCell.GetComponent<RoomCell>();
        roomCell.cellPosition = location;
        if (amountOfDoors > 0)
        {
            doorDirections = roomCell.GenerateDoors(amountOfDoors);
        }
        else
        {
            doorDirections = null;
        }
    }

    /// <summary>
    /// Creates a new cell and gives out an array of the directions the cell may branch out of.
    /// This cell is for cells that are branching out, as it allows for an originating direction 
    /// to be fed to blacklist for the new branching cell.
    /// </summary>
    /// <param name="location">Where the cell should be created</param>
    /// <param name="cell">The cell prefab that is created</param>
    /// <param name="amountOfDoors">How many doors should the cell form</param>
    /// <param name="doorDirections">The array of directions that the cell is branching out to</param>
    /// <param name="originatingDirection">If branching out, give the originating direction for the new cell to blacklist, to avoid doorframes being created there.</param>
    void CreateNewCell(Vector3Int location, GameObject cell, int amountOfDoors, out CardinalDirections[] doorDirections, CardinalDirections blacklistDirection)
    {
        GameObject newCell = Instantiate(cell);
        newCell.transform.position = grid.GetCellCenterWorld(location);

        // Sets new instantiated cell in cell coordinates
        CellCoordinates.Instance.cellCoordinates[location.x, location.z] = newCell;
        currentCell = newCell;

        #region Console Logging
        // Debug.Log("Cell Coordinates: ( " + location.x + " , " + location.z + " )");
        #endregion

        RoomCell roomCell = newCell.GetComponent<RoomCell>();
        roomCell.cellPosition = location;
        if (amountOfDoors > 0)
        {
            roomCell.doorDirectionsBlacklist.Add(blacklistDirection);
            doorDirections = roomCell.GenerateDoors(amountOfDoors);
        }
        else
        {
            doorDirections = null;
        }
    }

    /// <summary>
    /// Generates a single branch off of an existing cell, and in one direction.
    /// Also changes the blacklist direction based on the original direction that is branching out of,
    /// disallowing the branched out cell from creating a door in the same position.
    /// </summary>
    /// <param name="cell">The existing cell that will be branched out of.</param>
    /// <param name="direction">The direction the new cell should be created.</param>
    void GenerateBranchOut(GameObject cell, CardinalDirections direction)
    {
        RoomCell roomCell = cell.GetComponent<RoomCell>();
        Vector3Int newCellPosition = roomCell.cellPosition;
        CardinalDirections blacklistDirection = CardinalDirections.North;
        switch (direction)
        {
            case CardinalDirections.North:
                newCellPosition.z += 1;
                blacklistDirection = CardinalDirections.South;
                break;
            case CardinalDirections.East:
                newCellPosition.x += 1;
                blacklistDirection = CardinalDirections.West;
                break;
            case CardinalDirections.South:
                newCellPosition.z -= 1;
                blacklistDirection = CardinalDirections.North;
                break;
            case CardinalDirections.West:
                newCellPosition.x -= 1;
                blacklistDirection = CardinalDirections.East;
                break;
            default:
                Debug.LogError("The direction enum given is invalid (I don't know how this is possible)");
                break;
        }

        int amountOfDoors = Rand.Range(0, roomCell.maxDoorsCreated + 1);
        CreateNewCell(newCellPosition, baseCellPrefab, amountOfDoors, out currentDoorDirections, blacklistDirection);
    }
    #endregion

    #region Private Variables

    Grid grid;

    #endregion
}
