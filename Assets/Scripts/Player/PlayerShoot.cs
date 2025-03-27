using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ShootProjectile))]
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform forwardPoint;
    public float fireCooldownTime;

    ShootProjectile shootScript;

    public float fireForce;
    bool hasFired;

    private InputAction fireAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireAction = InputSystem.actions.FindAction("Fire");
        shootScript = GetComponent<ShootProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        // When player fires
        if (fireAction.IsPressed() == true && hasFired == false)
        {
            // fire
            shootScript.FireProjectile(projectilePrefab, forwardPoint.position, forwardPoint.forward, fireForce);
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
}
