using UnityEngine;

public class ShootProjectile : MonoBehaviour
{

    public void FireProjectile(GameObject mProjectile, Vector3 startPoint, Vector3 direction, float mFireForce)
    {
        // Checks for rigidbody of the projectile prefab given, without it the code won't work
        if (mProjectile.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Projectile fired does not contain a Rigidbody.");
            return;
        }

        GameObject firedProjectile = Instantiate(mProjectile);
        Rigidbody rb = firedProjectile.GetComponent<Rigidbody>();
        Transform projectileTrans = firedProjectile.transform;

        projectileTrans.position = startPoint;
        projectileTrans.forward = direction;

        rb.AddForce(direction * mFireForce);

    }
}
