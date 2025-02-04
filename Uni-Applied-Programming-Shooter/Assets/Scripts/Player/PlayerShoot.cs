using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ShootProjectile))]
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    bool hasFired;

    private InputAction fireAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
    }

    // Update is called once per frame
    void Update()
    {
        if (fireAction.IsPressed() == true && hasFired == false)
        {
            // fire
            hasFired = true;
        }
        if (fireAction.WasReleasedThisFrame())
        {
            hasFired = false;
        }
    }
}
