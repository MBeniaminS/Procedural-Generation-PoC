using UnityEngine;

public class CellCoordinates : MonoBehaviour
{
    public static CellCoordinates Instance;

    [SerializeField] int maxMapXSize = 10;
    [SerializeField] int maxMapZSize = 10;

    public int MaxMapXSize { get { return maxMapXSize; } }
    public int MaxMapZSize { get { return maxMapZSize; } }

    public GameObject[,] cellCoordinates;

    private void Awake()
    {
        Instance = this;
        // Error checking and reverting to default number
        if (maxMapXSize <= 0)
        {
            Debug.LogError("Max Map X Size has been set to 0 or below. Setting to default of 10.");
            maxMapXSize = 10;
        }
        if (maxMapZSize <= 0)
        {
            Debug.LogError("Max Map Z Size has been set to 0 or below. Setting to default of 10.");
            maxMapZSize = 10;
        }
        cellCoordinates = new GameObject[maxMapXSize, maxMapZSize];
    }

    /// <summary>
    /// Attempts to find the roomCell component in the coordinates specified. Returns as true of false. Also returns the RoomCell component (if cell is found) as variable.
    /// </summary>
    /// <para       m name="location">Coordinates/Location of the cell to be found</param>
    /// <param name="roomCell">The RoomCell component returned if cell is in location</param>
    /// <returns></returns>
    public bool GetCellInLocation(Vector3Int location, out RoomCell roomCell)
    {
        int indexX = location.x;
        int indexZ = location.z;
        if (cellCoordinates[indexX, indexZ] != null)
        {
            Debug.Log("Cell Location: " + location + " has a cell");
            roomCell = cellCoordinates[indexX, indexZ].GetComponent<RoomCell>();
            return true;
        }
        else
        {
            Debug.LogWarning("Cell Location: " + location + " does not exist in system");
            roomCell = null;
            return false;
        }
    }

    /// <summary>
    /// Attempts to find the roomCell component in the coordinates specified. Returns as true of false. Does not return component.
    /// </summary>
    /// <param name="location">Coordinates/Location of the cell to be found</param>
    /// <returns></returns>
    public bool GetCellInLocation(Vector3Int location)
    {
        int indexX = location.x;
        int indexZ = location.z;
        if (cellCoordinates[indexX, indexZ] != null)
        {
            Debug.Log("Cell Location: " + location + " has a cell");
            return true;
        }
        else
        {
            //Debug.LogWarning("Cell Location: " + location + " does not exist in system");
            return false;
        }
    }

}
