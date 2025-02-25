using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridTesting : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    public float fireCooldownTime;

    public GameObject testPrefab;
    Grid grid;
    Camera mainCam;

    InputAction fireAction;
    bool hasFired;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GetComponent<Grid>();
        mainCam = Camera.main;

        fireAction = InputSystem.actions.FindAction("Fire");
    }

    // Update is called once per frame
    void Update()
    {
        if (fireAction.IsPressed() == true && hasFired == false)
        {
            Vector3Int cellPos = grid.WorldToCell(GetMousePos());
            SpawnPrefabAtPoint.instance.SpawnPrefab(testPrefab, grid.GetCellCenterWorld(cellPos), null);
            //print("Cell Position: " + cellPos);
            hasFired = true;
            StartCoroutine(fireCooldown(fireCooldownTime));
        }
    }

    IEnumerator fireCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        hasFired = false;
        yield return null;
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

    private void GenerateDoors(int amount)
    {
        
    }
}
