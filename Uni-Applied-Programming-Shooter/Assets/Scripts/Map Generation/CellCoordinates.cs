using System.Collections.Generic;
using UnityEngine;

public class CellCoordinates : MonoBehaviour
{
    public static CellCoordinates Instance;

    [SerializeField] int maxMapXSize;
    [SerializeField] int maxMapYSize;

    public int MaxMapXSize { get { return maxMapXSize; } }
    public int MaxMapYSize { get { return maxMapYSize; } }


    public GameObject[,] cellCoordinates;

    private void Awake()
    {
        Instance = this;
        cellCoordinates = new GameObject[maxMapXSize, maxMapYSize];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Attempts to find the roomCell component in the coordinates specified. Returns as true of false. Also returns the RoomCell component (if cell is found) as variable.
    /// </summary>
    /// <param name="location">Coordinates/Location of the cell to be found</param>
    /// <param name="roomCell">The RoomCell component returned if cell is in location</param>
    /// <returns></returns>
    bool GetCellInLocation(Vector3Int location, out RoomCell roomCell)
    {
        int indexX = location.x;
        int indexY = location.y;
        if (cellCoordinates[indexX, indexY].TryGetComponent<RoomCell>(out RoomCell cellComp))
        {
            Debug.Log("Cell Location: " + location + " has a cell");
            roomCell = cellComp;
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
    bool GetCellInLocation(Vector3Int location)
    {
        int indexX = location.x;
        int indexY = location.z;
        if (cellCoordinates[indexX, indexY].TryGetComponent<RoomCell>(out RoomCell cellComp))
        {
            Debug.Log("Cell Location: " + location + " has a cell");
            return true;
        }
        else
        {
            Debug.LogWarning("Cell Location: " + location + " does not exist in system");
            return false;
        }
    }
}
