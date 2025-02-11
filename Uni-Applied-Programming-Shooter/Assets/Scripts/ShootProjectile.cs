using UnityEngine;

public class ShootProjectile : MonoBehaviour
{

    public void FireProjectile(GameObject mProjectile, Vector3 startPoint, Vector3 direction, float m_FireForce)
    {
        // Checks for rigidbody of the projectile prefab given, without it the code won't work
        if (mProjectile.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Projectile fired does not contain a Rigidbody.");
            return;
        }

        GameObject firedProjectile = Instantiate(mProjectile);
        firedProjectile.transform.parent = GameObject.FindWithTag("ProjectileList").transform;
        Rigidbody rb = firedProjectile.GetComponent<Rigidbody>();
        Transform projectileTrans = firedProjectile.transform;

        projectileTrans.position = startPoint;
        projectileTrans.forward = direction;

        rb.AddForce(direction * m_FireForce);

    }
}
