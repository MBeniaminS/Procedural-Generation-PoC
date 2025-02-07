using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject pl;
    public Path mypath;
    private int targetPoint = 0;
    // Use this for initialization
    void Start()
    {
        pl = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void FollowPath(Path p)
    {

        //get current target point pt
        Vector3 currentTargetPt = p.GetPositionOfWP(targetPoint);
        //face point pt
        FacePoint(currentTargetPt);
        //move towards point pt
        float speed = 5.0f;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (GetDistanceToPoint(currentTargetPt) < 0.5f)
        {
            targetPoint++;
        }

        //if we have reached end of array then wrap back to 0
        //}
    }

    bool GetIfPlayerWithinRange(float range)
    {
        return (GetDistanceToPlayer() < range);
    }

    public float GetDistanceToPlayer()
    {

        float distance = Vector3.Distance(pl.transform.position, transform.position);
        //Debug.Log(distance);
        return distance;
    }

    void FacePlayer()
    {
        float angle = GetAngleToPlayer();
        transform.Rotate(0, -GetAngleToPlayer(), 0);
    }

    void FaceAwayFromPlayer()
    {
        float angle = 180.0f + GetAngleToPlayer();
        transform.Rotate(0, -angle, 0);
    }

    public Vector3 GetVectorToPlayer()
    {
        Vector3 targetDir = pl.transform.position - transform.position;
        targetDir.y = 0;
        return targetDir;
    }

    public float GetAngleToPlayer()
    {
        //get relative position of player
        Vector3 targetDir = GetVectorToPlayer();

        //forward vector of NPC
        Vector3 forward = transform.forward;
        forward.y = 0;
        //calc cangle between 
        float angle = Vector3.Angle(forward, targetDir);

        //is angle positive?
        //compute cross product
        Vector3 cross = Vector3.Cross(targetDir, forward);
        if (cross.y < 0) angle *= -1.0f;
        return angle;
    }
    void MoveTowardsPlayer()
    {
        //Vector3 newpos=Vector3.MoveTowards (transform.position, pl.transform.position, maxDistanceDelta);
        //transform.Translate(newpos)  ;
    }

    void FacePoint(Vector3 pt)
    {
        float angle = GetAngleToPoint(pt);
        transform.Rotate(0, -angle, 0);
    }

    public Vector3 GetVectorToPoint(Vector3 pt)
    {
        Vector3 targetDir = pt - transform.position;
        targetDir.y = 0;
        return targetDir;
    }

    public float GetDistanceToPoint(Vector3 pt)
    {

        float distance = Vector3.Distance(pt, transform.position);
        //Debug.Log(distance);
        return distance;
    }

    public float GetAngleToPoint(Vector3 pt)
    {
        //get relative position of player
        Vector3 targetDir = GetVectorToPoint(pt);

        //forward vector of NPC
        Vector3 forward = transform.forward;
        forward.y = 0;
        //calc cangle between 
        float angle = Vector3.Angle(forward, targetDir);

        //is angle positive?
        //compute cross product
        Vector3 cross = Vector3.Cross(targetDir, forward);
        if (cross.y < 0) angle *= -1.0f;
        return angle;
    }
}
