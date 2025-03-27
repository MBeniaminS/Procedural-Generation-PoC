using UnityEngine;

public class CanSeePlayer : MonoBehaviour
{
    public float visionRadius;
    public float visionDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray visionRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + (transform.forward * visionDistance), Color.yellow);
        if (Physics.Raycast(visionRay, out hit, visionDistance))
        {
            print("Raycast Hit: " + hit.collider.name);
        }
    }
}
