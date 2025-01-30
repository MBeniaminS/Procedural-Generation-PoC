using NUnit.Framework;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] private LayerMask groundMask;

    private Camera mainCam;

    #region Methods

    #region Unity Callbacks

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Aim();

    }

    #endregion Unity Callbacks

    private void Aim()
    {
        var (success, position) = GetMousePos();
        if (success)
        {
            var direction = position - transform.position;

            direction.y = 0;

            transform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePos()
    {
        Ray cameraRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(cameraRay, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success:  false, position: Vector3.zero);
        }
    }

    #endregion Methods
}
