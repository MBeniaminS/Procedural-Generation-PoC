using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    private Camera mainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = FindFirstObjectByType<Camera>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePosition = mainCam.ScreenToWorldPoint();
        Ray cameraRay = mainCam.ScreenPointToRay();
    }
}
