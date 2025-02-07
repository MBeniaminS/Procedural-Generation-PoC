using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireProjectile(GameObject mProjectile, Transform mStartPoint, Vector3 mDirection, float mFireForce)
    {
        if (mProjectile.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Projectile fired does not contain a Rigidbody.");
            return;
        }

        GameObject firedProjectile = Instantiate(mProjectile);


    }
}
