using UnityEngine;

public class CanSeePlayer : MonoBehaviour
{
    public float visionRadius;
    public float visionDistance;
    public LayerMask layerMask;
    public bool canSeePlayer;

    // Update is called once per frame
    void Update()
    {
        Ray visionRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + (transform.forward * visionDistance), Color.yellow);
        if (Physics.Raycast(visionRay, out hit, 100f, layerMask))
        {
            if (hit.collider.tag == "Player")
            {
                canSeePlayer = true;
            }
        }
    }
}
