using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class LookAtMouse : MonoBehaviour
{

    [SerializeField] private LayerMask groundMask;

    private Camera mainCam;

    // #region is a good way of categorising bits of code, just don't do it too much or else you nullify its effectiveness
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
            // Draws a line ONLY IN THE SCENE OF THE UNITY EDITOR, NOT IN GAME
            Debug.DrawLine(position, new Vector3(position.x, 1, position.z));
            var direction = position - transform.position;

            direction.y = 0;

            transform.forward = direction;
        }
    }

    // A method that outputs both a success variable and a position output. It OUTPUTS these variables while not requiring any input,
    // keeping it a void method.
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
