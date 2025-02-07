using UnityEngine;


//used as a base class for all behaviors which implement individual states
public class NPCState : MonoBehaviour
{

    protected Transform pl;
    protected float mSpeed = 3.0f;
    public string strStateName = "state";
    public NPCwithFSM myParentFSM;

    void OnEnable()
    {
        pl = GameObject.FindWithTag("Player").transform;
        //initialize 'player' variable	
        Debug.Log("++OnEnabled func:player found");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void FaceTowards(Vector3 targetPoint)
    {
        //face target
        this.transform.LookAt(targetPoint);
    }

    protected void FaceAwayFromPlayer()
    {
        //face target
        float angle = GetAngleToPlayer();
        transform.Rotate(0, 180 - angle, 0);
    }

    protected void MoveForwards()
    {
        this.transform.Translate(0, 0, mSpeed * Time.deltaTime, Space.Self);
    }

    protected float GetDistanceToTarget(Vector3 targetPoint)
    {
        Vector3 myPos = transform.position;
        Vector3 vecFromMeToTarget = targetPoint - myPos;
        float dist = vecFromMeToTarget.magnitude;
        return dist;
    }

    protected Vector3 GetVectorToPlayer()
    {
        Vector3 targetDir = pl.transform.position - transform.position;
        targetDir.y = 0;
        return targetDir;
    }

    protected float GetAngleToPlayer()
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


    protected bool GetIfPlayerWithinRange(float range)
    {
        return (GetDistanceToPlayer() < range);
    }


    protected float GetDistanceToPlayer()
    {
        float distance = Vector3.Distance(pl.transform.position, transform.position);
        return distance;
    }

}
