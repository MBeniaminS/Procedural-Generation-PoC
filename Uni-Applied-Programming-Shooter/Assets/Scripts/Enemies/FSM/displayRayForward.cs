using UnityEngine;

public class displayRayForward : MonoBehaviour
{

    public int rayLength = 5;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color fwdRayColor = Color.blue;
        float fwdRayLength = 5;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            float distanceToHit = hit.distance;
            Object itemHit = hit.collider;
            if (distanceToHit < fwdRayLength)
            {
                fwdRayColor = Color.red;
                fwdRayLength = distanceToHit;


            }
        }




        Debug.DrawRay(this.transform.position, fwdRayLength * this.transform.forward, fwdRayColor);


        Vector3 vLeft = Vector3.Cross(this.transform.forward, Vector3.up);
        Vector3 vRight = Vector3.Cross(this.transform.forward, Vector3.down);
        Vector3 v45Left = vLeft + this.transform.forward;
        v45Left.Normalize();

        Vector3 v45Right = vRight + this.transform.forward;
        v45Right.Normalize();

        Debug.DrawRay(this.transform.position, rayLength * vLeft);
        Debug.DrawRay(this.transform.position, rayLength * vRight);
        Debug.DrawRay(this.transform.position, rayLength * v45Left);
        Debug.DrawRay(this.transform.position, rayLength * v45Right);
    }
}
