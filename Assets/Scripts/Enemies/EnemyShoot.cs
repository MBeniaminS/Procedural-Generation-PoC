using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] CanSeePlayer canSeePlayer;
    [SerializeField] Transform forwardPoint;
    [SerializeField] float fireCooldownTime;

    ShootProjectile shootScript;

    public float fireForce = 500f;
    bool hasFired;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootScript = GetComponent<ShootProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        // When player fires
        if (canSeePlayer.canSeePlayer && hasFired == false)
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
