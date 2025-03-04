using System.Collections.Generic;
using UnityEngine;

public class CellCoordinatesSystem : MonoBehaviour
{
    public static CellCoordinatesSystem instance;

    Dictionary<Vector3Int, GameObject> cellCoordinates = new Dictionary<Vector3Int, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        IsCellInLocation(new Vector3Int(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AppendNewCell(Vector3Int cellLocation, GameObject cell)
    {
        cellCoordinates.Add(cellLocation, cell);
    }

    bool IsCellInLocation(Vector3Int location)
    {
        if (cellCoordinates.ContainsKey(location))
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
