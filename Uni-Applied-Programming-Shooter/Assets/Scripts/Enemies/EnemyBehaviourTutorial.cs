using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviourTutorial : MonoBehaviour
{
    public GameObject pl;

    private state mState;

    public enum state
    {
        eStateFollow,
        eStateEvade,
        eStateDisabled,
        eStateIdle
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mState = state.eStateIdle;

        pl = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        switch (mState)
        {
            case state.eStateIdle:
                FaceAwayFromPlayer();
                break;
            case state.eStateFollow:
                FacePlayer();
                MoveForwards();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (GetIfPlayerWithinRange(10f))
        {
            mState = state.eStateFollow;
        }
    }

    void FacePlayer()
    {
        float angle = GetAngleToPlayer();
        transform.Rotate(0, -angle, 0);
    }


    void FaceAwayFromPlayer()
    {
        float angle = GetAngleToPlayer();
        transform.Rotate(0, 180 - angle, 0);
    }


    Vector3 GetVectorToPlayer()
    {
        Vector3 targetDir = pl.transform.position - transform.position;
        targetDir.y = 0;
        return targetDir;
    }



    float GetAngleToPlayer()
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


    bool GetIfPlayerWithinRange(float range)
    {
        return (GetDistanceToPlayer() < range);
    }

    float GetDistanceToPlayer()
    {
        float distance = Vector3.Distance(pl.transform.position, transform.position);

        return distance;
    }
    void MoveForwards()
    {
        transform.Translate(0f, 0f, 0.05f, Space.Self);
    }

}
