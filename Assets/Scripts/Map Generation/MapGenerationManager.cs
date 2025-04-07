using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rand = UnityEngine.Random;

public class MapGenerationManager : MonoBehaviour
{
    #region Serialized and Public Variables

    public static MapGenerationManager Instance;
    public enum CardinalDirections
    {
        North,
        East,
        South,
        West
    }

    [Header("Set Up References")]
    [SerializeField] Transform procGenLayoutParent;
    [SerializeField] GameObject startingCellPrefab;
    [SerializeField] GameObject baseCellPrefab;
    [SerializeField] GameObject finalRoomCellPrefab;
    [SerializeField] int minimumAmountOfCells = 7;
    [SerializeField] int maximumAmountOfCells = 15;
    [SerializeField] float delayBetweenGenerationCommands = 0.5f;
    public IEnumerator delayedSceneRestart;

    [Header("Updating References")]
    [SerializeField] GameObject currentCell;
    [SerializeField] GameObject startBranchCell;
    [SerializeField] CardinalDirections[] currentDoorDirections;
    public bool allowGenerateDoors;
    public Vector3Int centerCellLocation;

    [Header("Statistics")]
    [SerializeField] int amountOfCellsCreated;



    #endregion

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
        centerCellLocation = new Vector3Int(midpointMapXSize, 0, midpointMapZSize);

        proceduralGenCoroutine = StartProceduralGeneration();
        StartCoroutine(proceduralGenCoroutine);

    }
    #endregion

    #region Public Methods
    public void startDelayedSceneRestart(float delay)
    {
        delayedSceneRestart = DelayedSceneRestart(delay);
        StartCoroutine(delayedSceneRestart);
    }

    public IEnumerator DelayedSceneRestart(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region Private Methods
    IEnumerator StartProceduralGeneration()
    {
        allowGenerateDoors = true;
        isGeneratingLayout = true;

        CreateNewCell(centerCellLocation, startingCellPrefab, Rand.Range(2, 4), out currentDoorDirections, false);

        yield return new WaitForSeconds(delayBetweenGenerationCommands);

        while (isGeneratingLayout)
        {
            print("GENERATING LAYOUT");
            foreach (var direction in currentDoorDirections)
            {
                print("Generating: " + direction + " branch out...");
                GenerateBranchOut(startBranchCell, direction);
                yield return new WaitForSeconds(delayBetweenGenerationCommands);
            }
            if (amountOfCellsCreated > minimumAmountOfCells)
            {
                print("DISABLING DOOR GEN");
                allowGenerateDoors = false;
            }

            // Initially sets the bool to true, to be corrected to false if there are any cells that need branching.
            bool allBranchesComplete = true;

            foreach (GameObject cell in CellCoordinates.Instance.cellCoordinates)
            {
                if (cell != null)
                {
                    print("found room cell");
                    RoomCell roomCell = cell.GetComponent<RoomCell>();
                    // If branch is not complete
                    if (!roomCell.isBranchDone)
                    {
                        if (roomCell.amountOfDoorsCreated == 0)
                        {
                            print("Room " + roomCell.cellPosition + "saying it has incomplete branch has no more doors");
                            continue;
                        }
                        print("branch is not done, setting new branch start");
                        allBranchesComplete = false;
                        startBranchCell = cell;
                        currentDoorDirections = roomCell.generatedDoorDirections;
                        break;
                    }
                }
            }
            if (allBranchesComplete)
            {
                print("ALL BRANCHES COMPLETE");
                if (minimumAmountOfCells < amountOfCellsCreated && amountOfCellsCreated < maximumAmountOfCells)
                {
                    print("all branches complete and within cell amounts, disabling generation...");

                    List<RoomCell> cellList = new List<RoomCell>();

                    /// Below all of the available cells are added to cellList, and are sorted by distance away from the center location.
                    /// Since the sorting sorts the furthest away to be at the end of the list, it checks from the end of the list down whether
                    /// the cell is a deadend (has no doors generated), to which it will turn that into the final room.
                    foreach (GameObject cell in CellCoordinates.Instance.cellCoordinates)
                    {
                        if (cell != null)
                        {
                            RoomCell roomCell = cell.GetComponent<RoomCell>();
                            cellList.Add(roomCell);
                        }
                    }
                    DistanceCompare distanceCompare = new DistanceCompare(centerCellLocation);

                    // Sorts the list by distance from the center (spawn) location
                    cellList.Sort(distanceCompare);

                    #region Console Logging
                    foreach (var item in cellList)
                    {
                        print("Cell: " + item.cellPosition + ", Distance: " + Vector3Int.Distance(item.cellPosition, centerCellLocation));
                    }
                    #endregion

                    // Checks from the last entry down whether it is a dead end, if yes, turns it into the final room.
                    for (int i = 1; i < cellList.Count - 1; i++)
                    {
                        if (cellList[^i].amountOfDoorsCreated == 0)
                        {
                            Instantiate(finalRoomCellPrefab, cellList[^i].transform.position, cellList[^i].transform.rotation, procGenLayoutParent);
                            Destroy(cellList[^i].gameObject);
                            break;
                        }
                    }

                }
                else
                {
                    print("all branches complete but not within cell amounts, resetting generation...");
                    ResetProceduralGeneration();
                    yield break;
                }
                isGeneratingLayout = false;
            }
        }

        yield return null;
    }

    /// <summary>
    /// Creates a new cell and gives out an array of the directions the cell may branch out of
    /// </summary>
    /// <param name="location">Where the cell should be created</param>
    /// <param name="cell">The cell prefab that is created</param>
    /// <param name="amountOfDoors">How many doors should the cell form</param>
    /// <param name="doorDirections">The array of directions that the cell is branching out to</param>
    /// <param name="isBranching">Checks if the cell created is as a result of branching out from one cell. If yes, it does not set the cell as the "startBranchCell"</param>
    void CreateNewCell(Vector3Int location, GameObject cell, int amountOfDoors, out CardinalDirections[] doorDirections, bool isBranching)
    {
        GameObject newCell = Instantiate(cell);
        newCell.transform.position = grid.GetCellCenterWorld(location);
        newCell.transform.parent = procGenLayoutParent;
        newCell.name = cell.name;
        amountOfCellsCreated++;

        // Sets new instantiated cell in cell coordinates
        CellCoordinates.Instance.cellCoordinates[location.x, location.z] = newCell;
        currentCell = newCell;
        if (!isBranching)
        {
            startBranchCell = newCell;
        }

        #region Console Logging
        // Debug.Log("Cell Coordinates: ( " + location.x + " , " + location.z + " )");
        #endregion

        RoomCell roomCell = newCell.GetComponent<RoomCell>();
        roomCell.cellPosition = location;
        //roomCell.CheckForAdjacentWalls();
        if (amountOfDoors > 0)
        {
            doorDirections = roomCell.GenerateDoors(amountOfDoors);
        }
        else
        {
            doorDirections = null;

            // If there are no doors to be generated, set the cell to be finished with its branch to set a dead end.
            roomCell.isBranchDone = true;
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
    /// <param name="isBranching">Checks if the cell created is as a result of branching out from one cell. If yes, it does not set the cell as the "startBranchCell"</param>
    /// <param name="blacklistDirection">If branching out, give the originating direction for the new cell to blacklist, to avoid doorframes being created there.</param>
    void CreateNewCell(Vector3Int location, GameObject cell, int amountOfDoors, out CardinalDirections[] doorDirections, bool isBranching, CardinalDirections blacklistDirection)
    {
        GameObject newCell = Instantiate(cell);
        newCell.transform.position = grid.GetCellCenterWorld(location);
        newCell.transform.parent = procGenLayoutParent;
        newCell.name = cell.name;
        amountOfCellsCreated++;

        print("CELL CREATED AT " + location);

        // Sets new instantiated cell in cell coordinates
        CellCoordinates.Instance.cellCoordinates[location.x, location.z] = newCell;
        currentCell = newCell;
        if (!isBranching)
        {
            startBranchCell = newCell;
        }

        #region Console Logging
        // Debug.Log("Cell Coordinates: ( " + location.x + " , " + location.z + " )");
        #endregion

        RoomCell roomCell = newCell.GetComponent<RoomCell>();
        roomCell.cellPosition = location;
        if (amountOfDoors > 0)
        {
            roomCell.doorDirectionsBlacklist.Add(blacklistDirection);

            // Only calls method here to ensure that the original blacklist direction is added first.
            roomCell.CheckForAdjacentWalls();
            doorDirections = roomCell.GenerateDoors(amountOfDoors);
        }
        else
        {
            doorDirections = null;

            // If there are no doors to be generated, set the cell to be finished with its branch to set a dead end.
            roomCell.isBranchDone = true;
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
        // Sets current cell's branch as complete
        roomCell.isBranchDone = true;
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

        if (!CellCoordinates.Instance.GetCellInLocation(newCellPosition))
        {
            int amountOfDoors = Rand.Range(0, roomCell.maxDoorsCreated + 1);
            CreateNewCell(newCellPosition, baseCellPrefab, amountOfDoors, out currentDoorDirections, true, blacklistDirection);
        }
    }

    void ResetProceduralGeneration()
    {
        StopCoroutine(proceduralGenCoroutine);
        foreach (Transform child in procGenLayoutParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.FindWithTag("EnemyList").GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        GameObject enemyList = new GameObject("EnemyList");
        enemyList.tag = "EnemyList";

        amountOfCellsCreated = 0;
        allowGenerateDoors = true;
        isGeneratingLayout = true;
        proceduralGenCoroutine = null;
        proceduralGenCoroutine = StartProceduralGeneration();
        StartCoroutine(proceduralGenCoroutine);
    }

    #endregion

    public class DistanceCompare : IComparer<RoomCell>
    {
        Vector3Int reference;
        public DistanceCompare(Vector3Int referencePoint)
        {
            reference = referencePoint;
        }
        public int Compare(RoomCell a, RoomCell b)
        {
            var distanceA = Vector3Int.Distance(a.cellPosition, reference);
            var distanceB = Vector3Int.Distance(b.cellPosition, reference);
            if (distanceA < distanceB) { return -1; }
            if (distanceA > distanceB) { return 1; }
            return 0;
        }
    }

    #region Private Variables

    private IEnumerator proceduralGenCoroutine;

    private bool isGeneratingLayout;
    private Grid grid;

    #endregion
}
